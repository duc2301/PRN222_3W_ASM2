using ClubManagement.Service.DTOs.RequestDTOs.Activity;
using ClubManagement.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.Activities
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IActivityService _activityService;
        private readonly IClubService _clubService;

        public EditModel(IActivityService activityService, IClubService clubService)
        {
            _activityService = activityService;
            _clubService = clubService;
        }

        [BindProperty]
        public ActivityUpdateDTO ActivityUpdate { get; set; } = null!;

        public SelectList ClubSelectList { get; set; } = null!;

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

            ActivityUpdate = new ActivityUpdateDTO
            {
                ActivityId = activity.ActivityId,
                ActivityName = activity.ActivityName,
                ClubId = activity.Club.ClubId,
                Description = activity.Description,
                Location = activity.Location,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate
            };

            var clubs = await _clubService.GetAllAsync();
            ClubSelectList = new SelectList(clubs, "ClubId", "ClubName", ActivityUpdate.ClubId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var clubs = await _clubService.GetAllAsync();
                ClubSelectList = new SelectList(clubs, "ClubId", "ClubName", ActivityUpdate.ClubId);
                return Page();
            }

            // Validate dates
            if (ActivityUpdate.EndDate <= ActivityUpdate.StartDate)
            {
                ModelState.AddModelError("ActivityUpdate.EndDate", "End date must be after start date.");
                var clubs = await _clubService.GetAllAsync();
                ClubSelectList = new SelectList(clubs, "ClubId", "ClubName", ActivityUpdate.ClubId);
                return Page();
            }

            await _activityService.UpdateAsync(ActivityUpdate);
            TempData["SuccessMessage"] = "Activity updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
