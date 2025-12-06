using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.ClubManager
{
    [Authorize(Roles = "ClubManager")]
    public class DetailsModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public DetailsModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // Id CLB (bind từ route / query)
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        // Dữ liệu CLB lấy từ DB
        public ClubResponseDTO? Club { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Id = id;

            Club = await _serviceProviders.ClubService.GetByIdAsync(id);

            if (Club == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
