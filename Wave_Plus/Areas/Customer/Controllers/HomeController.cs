using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;
using Wave.DataAccess.Repository.IRepository;
using Wave.Models;
using Wave.Models.viewModels;
using Wave.Utility;

namespace Wave_Plus.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int page = 1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count()); ;
            }

            int pageSize = 9; // Number of products per page

            // Retrieve all products
            List<Product> allProducts = _unitOfWork.Product
                .GetAll(includeProperties: "Category,ProductImages")
                .ToList();

            // Filter the products to exclude the ones with "unavailable" availability
            List<Product> availableProducts = allProducts
                .Where(p => p.ProductAvailbality != SD.ProductUnAvailable)
                .ToList();

            // Calculate the total number of products
            int totalProducts = availableProducts.Count;

            // Calculate the total number of pages
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Validate the requested page number
            if (page < 1 || page > totalPages)
            {
                return NotFound();
            }

            // Calculate the number of products to skip based on the page number
            int skip = (page - 1) * pageSize;

            // Retrieve the subset of products based on the page number and page size
            List<Product> productList = availableProducts
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Create the view model and pass it to the view
            var viewModel = new ProductVM
            {
                Products = productList,
                PageNumber = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u=>u.Id == productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId,

            };

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //For ProductVM
            int pageSize = 12;
            int totalProducts = _unitOfWork.Product.Count();

            // Calculate the total number of pages
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Assuming you have access to the product data, you can create a ProductVM object here
            var productVM = new ProductVM
            {
                Products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList(),
                PageNumber = 1, // Set the initial page number
                TotalPages = totalPages
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if(cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart Updated Successfully";


            return View("Index", productVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}