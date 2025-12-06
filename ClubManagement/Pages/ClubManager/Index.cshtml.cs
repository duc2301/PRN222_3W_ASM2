using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.ClubManager
{
    [Authorize(Roles = "ClubManager")]
    public class IndexModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public IndexModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public List<ClubResponseDTO> ManagedClubs { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                ManagedClubs = new List<ClubResponseDTO>();
                return;
            }

           
            var username = User.Identity.Name;

            ManagedClubs = await _serviceProviders.ClubService.GetClubsForLeaderAsync(username);
        }
    }
}
