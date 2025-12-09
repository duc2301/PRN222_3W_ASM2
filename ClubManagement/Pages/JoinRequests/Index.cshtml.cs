using AutoMapper;
using ClubManagement.Repository.Models;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.JoinRequests
{
    public class IndexModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public IndexModel(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProviders = serviceProviders;
            _mapper = mapper;
        }

       public IEnumerable<JoinRequest> Requests { get; set; } = new List<JoinRequest>();
    
    [TempData]
    public string? SuccessMessage { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
                Requests = await _serviceProviders.JoinRequestService.GetAllAsync();
            }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi khi tải dữ liệu: {ex.Message}";
            Requests = new List<JoinRequest>();
        }
    }
    }
}

