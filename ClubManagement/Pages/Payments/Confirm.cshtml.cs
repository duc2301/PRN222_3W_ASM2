using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Payments
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class ConfirmModel : PageModel
    {
        private readonly IServiceProviders _services;

        public ConfirmModel(IServiceProviders services)
        {
            _services = services;
        }

        public ClubManagement.Repository.Models.Payment Payment { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Payment = (await _services.PaymentService.GetAllAsync())
                        .FirstOrDefault(p => p.PaymentId == id);

            if (Payment == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            await _services.PaymentService.MarkAsPaidAsync(id);
            TempData["msg"] = "Thanh toán đã được xác nhận!";

            return RedirectToPage("Index");
        }
    }
}
