using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ClubManagement.Pages.Payments
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IServiceProviders _services;

        public CreateModel(IServiceProviders services)
        {
            _services = services;
        }

        [BindProperty]
        public int FeeId { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);

            // Không cho đóng 2 lần
            if (await _services.PaymentService.HasPaidAsync(userId, FeeId))
            {
                ModelState.AddModelError("", "Bạn đã đóng phí này rồi.");
                return Page();
            }

            try
            {
                await _services.PaymentService.CreatePaymentAsync(userId, FeeId, Amount);
                TempData["msg"] = "Tạo thanh toán thành công!";
                return RedirectToPage("MyPayments");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }
}

