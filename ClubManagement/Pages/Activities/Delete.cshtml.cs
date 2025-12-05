using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Activities
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IActivityService _activityService;

        public DeleteModel(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (Activity.ActivityId <= 0)
            {
                return NotFound();
            }

            await _activityService.DeleteAsync(Activity.ActivityId);
            TempData["SuccessMessage"] = "Activity deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
