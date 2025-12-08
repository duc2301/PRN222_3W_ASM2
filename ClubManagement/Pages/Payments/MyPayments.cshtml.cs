using ClubManagement.Repository.Models;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ClubManagement.Pages.Payments
{
    
    public class MyPaymentsModel : PageModel
    {
        private readonly IServiceProviders _services;

        public MyPaymentsModel(IServiceProviders services)
        {
            _services = services;
        }

        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);

            // Kiểm tra và cập nhật các payment quá hạn
            await _services.PaymentService.CheckAndUpdateExpiredPaymentsAsync();

            Payments = await _services.PaymentService.GetByUserAsync(userId);

            return Page();
        }

        public async Task<IActionResult> OnPostRequestPaymentAsync(int paymentId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);

            // Kiểm tra payment có thuộc về user này không
            var payment = await _services.PaymentService.GetByIdAsync(paymentId);
            if (payment == null || payment.UserId != userId)
            {
                return Forbid();
            }

            try
            {
                await _services.PaymentService.RequestPaymentAsync(paymentId);
                TempData["msg"] = "Đóng phí thành công! Khoản phí đã được xác nhận.";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}

