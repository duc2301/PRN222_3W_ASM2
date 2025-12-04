using AutoMapper;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Clubs
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public IndexModel(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProviders = serviceProviders;
            _mapper = mapper;
        }

        public List<ClubResponseDTO> Club { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Club = await _serviceProviders.ClubService.GetAllAsync();
        }
    }
}
