using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ClubManagement.Pages.Clubs
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class MyClubsModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public MyClubsModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public List<ClubResponseDTO> MyClubs { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allUsers = await _serviceProviders.UserService.GetAllAsync();
            var current = allUsers.FirstOrDefault(u => u.Username == User.Identity!.Name);

            if (current == null)
            {
                MyClubs = new List<ClubResponseDTO>();
                return;
            }

            var allClubs = await _serviceProviders.ClubService.GetAllAsync();

            if (User.IsInRole("Admin"))
            {
                MyClubs = allClubs.ToList();
            }
            else
            {
                MyClubs = allClubs.Where(c => c.LeaderId == current.UserId).ToList();
            }
        }
    }
}