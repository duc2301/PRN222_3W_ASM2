using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Activities
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IActivityService _activityService;
        private readonly IClubService _clubService;

        public IndexModel(IActivityService activityService, IClubService clubService)
        {
            _activityService = activityService;
            _clubService = clubService;
        }

        public IEnumerable<ActivityResponseDTO> Activities { get; set; } = new List<ActivityResponseDTO>();
        public IEnumerable<ClubResponseDTO> Clubs { get; set; } = new List<ClubResponseDTO>();

        public async Task OnGetAsync()
        {
            Activities = await _activityService.GetAllAsync();
            Clubs = await _clubService.GetAllAsync();
        }
    }
}
