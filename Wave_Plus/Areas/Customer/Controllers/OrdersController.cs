using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wave.DataAccess.Repository;
using Wave.DataAccess.Repository.IRepository;
using Wave.Models;
using Wave.Models.viewModels;
using Wave.Utility;

namespace Wave_Plus.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(userId == null)
            {
                return NotFound();
            }

            IEnumerable<OrderHeader> orderHeader = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, 
                includeProperties: "ApplicationUser").OrderByDescending(o=>o.OrderDate);
                

            return View(orderHeader);
        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult HistoryOrders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(userId == null)
            {
                return NotFound();
            }

            IEnumerable<OrderHeader> orderHeader = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, 
                includeProperties: "ApplicationUser")
                .OrderByDescending(o=>o.OrderDate);
            return View(orderHeader);
        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult Details(int? orderId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (orderId == null || orderId == 0)
            {
                return NotFound();
            }

            
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product"),
            };

            if (OrderVM == null)
            {
                return NotFound();
            }

            //Check for access URL
            if (OrderVM.OrderHeader.ApplicationUserId != userId)
            {
                return Unauthorized(); 
            }

            return View(OrderVM);
        }
    }
}
