using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Fees
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class DetailsModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public DetailsModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public FeeResponseDTO Fee { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Fee = await _serviceProviders.FeeService.GetByIdAsync(id);

            if (Fee == null)
                return NotFound();

            return Page();
        }
    }
}
