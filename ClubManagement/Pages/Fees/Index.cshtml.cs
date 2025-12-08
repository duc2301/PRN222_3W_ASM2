using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.Fees
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class IndexModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public IndexModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public IEnumerable<FeeResponseDTO> Fees { get; set; } = new List<FeeResponseDTO>();

        [BindProperty(SupportsGet = true)]
        public int? ClubId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        public async Task OnGetAsync()
        {
            Fees = await _serviceProviders.FeeService.GetAllAsync();
        }
    }
}
