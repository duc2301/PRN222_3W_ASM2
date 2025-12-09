using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Clubs
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public DetailsModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public ClubResponseDTO Club { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var club = await _serviceProviders.ClubService.GetByIdAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            Club = club;
            return Page();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var club = await _serviceProviders.ClubService.GetByIdAsync(id);
            if (club != null)
            {
                await _serviceProviders.ClubService.DeleteAsync(id);
            }
            return RedirectToPage("MyClubs");
        }
    }
}
