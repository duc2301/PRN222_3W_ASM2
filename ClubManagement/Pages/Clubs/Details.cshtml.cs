using ClubManagement.Hubs;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace ClubManagement.Pages.Clubs
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IHubContext<ClubHub> _hubContext;

        public DetailsModel(IServiceProviders serviceProviders, IHubContext<ClubHub> hubContext)
        {
            _serviceProviders = serviceProviders;
            _hubContext = hubContext;
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
                // Notify all clients about the change
                await _hubContext.Clients.All.SendAsync("ClubChanged");
            }
            return RedirectToPage("MyClubs");
        }
    }
}
