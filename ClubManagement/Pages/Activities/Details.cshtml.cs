using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Activities
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IActivityService _activityService;

        public DetailsModel(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public ActivityResponseDTO Activity { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _activityService.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            Activity = activity;
            return Page();
        }
    }
}
