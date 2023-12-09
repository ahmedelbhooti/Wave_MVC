using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using Wave.DataAccess.Repository.IRepository;
using Wave.Models;
using Wave.Models.viewModels;
using Wave.Utility;

namespace Wave_Plus.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                cart.Price = GetPrice(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                ShoppingCartVM.OrderHeader.OrderTotal = Math.Round(ShoppingCartVM.OrderHeader.OrderTotal, 2);
                cart.TotalProductPrice = (cart.Price * cart.Count);
                cart.TotalProductPrice = Math.Round(cart.TotalProductPrice, 2);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //Remove item from cart
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
                _unitOfWork.ShoppingCart.Remove(cartFromDb);

            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            HttpContext.Session.SetInt32(SD.SessionCart,_unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count()-1);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            if (ShoppingCartVM.OrderHeader.ApplicationUser != null)
            {
                ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
                ShoppingCartVM.OrderHeader.Email = ShoppingCartVM.OrderHeader.ApplicationUser.Email;
                ShoppingCartVM.OrderHeader.Country = ShoppingCartVM.OrderHeader.ApplicationUser.Country;
                ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
                ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
                ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAdress;
                ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
                ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            }


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPrice(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                ShoppingCartVM.OrderHeader.OrderTotal = Math.Round(ShoppingCartVM.OrderHeader.OrderTotal, 2);
                cart.TotalProductPrice = (cart.Price * cart.Count);
                cart.TotalProductPrice = Math.Round(cart.TotalProductPrice, 2);
            }
            return View(ShoppingCartVM);

        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(string payment)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPrice(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                ShoppingCartVM.OrderHeader.OrderTotal = Math.Round(ShoppingCartVM.OrderHeader.OrderTotal, 2);
                cart.TotalProductPrice = (cart.Price * cart.Count);
                cart.TotalProductPrice = Math.Round(cart.TotalProductPrice, 2);
            }

            if (string.IsNullOrEmpty(ShoppingCartVM.OrderHeader.Country))
            {
                ShoppingCartVM.OrderHeader.Country = "Swiss";
            }

            // Check the selected payment option
            if (payment == "paynow")
            {
                // Handle PayNow payment
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            else if (payment == "directcheck")
            {
                // Handle Direct Check payment
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusOnDelivery;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            else
            {
                // Handle error or default case
                TempData["error"] = "Please Choose the payment option";
                return View(ShoppingCartVM);
            }
           
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (payment == "paynow")
            {
                // Handle paynow payment
                //Stripe payment
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/StripeCancelled?id={ShoppingCartVM.OrderHeader.Id}",
                    LineItems = new List<SessionLineItemOptions>(),
                      
                    Mode = "payment",
                };

                foreach(var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sesstionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), //CHF 20.50 => 2050
                            Currency = "CHF",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sesstionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);

            }
            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });

        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if(orderHeader.PaymentStatus != SD.PaymentStatusOnDelivery)
            {
                //This is an order Paynow
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                    _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Wava_Plus", 
                        $"<h6>New Order Created - {orderHeader.Id}</h6>" +
                        $"<p>The payment process was completed successfully.</p>" +
                        $"<p>Order Total: CHF {orderHeader.OrderTotal}</p>" +
                        $"<p>Track your order status on the website.</p>" +
                        $"<p>Thank you for your order</p>");
                    HttpContext.Session.Clear();
                }
                if(session.PaymentStatus.ToLower() != "paid"){
                    _unitOfWork.OrderHeader.Remove(orderHeader);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(SomePaymentNotCompletedAction));
                }

            }
            if(orderHeader.PaymentStatus == SD.PaymentStatusOnDelivery)
            {
                _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Wava_Plus",
                        $"<h6>New Order Created - {orderHeader.Id}</h6>" +
                        $"<p>The payment process will be on delivery.</p>" +
                        $"<p>Order Total: CHF {orderHeader.OrderTotal}</p>" +
                        $"<p>Track your order status on the website.</p>" +
                        $"<p>Thank you for your order</p>");
                HttpContext.Session.Clear();
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult SomePaymentNotCompletedAction()
        {
            return View();
        }

        public IActionResult StripeCancelled(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            List<OrderDetail> orderDetails = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == id, includeProperties: "Product").ToList();

            if (orderHeader != null)
            {
                // Remove OrderHeader and associated OrderDetails
                _unitOfWork.OrderHeader.Remove(orderHeader);
                _unitOfWork.OrderDetail.RemoveRange(orderDetails);
                _unitOfWork.Save();
            }

            // Redirect the user back to the cart page or display an appropriate view
            return RedirectToAction(nameof(Index));
        }

        private double GetPrice(ShoppingCart shoppingCart)
        {
            return shoppingCart.Product.Price;
        }
    }
}
