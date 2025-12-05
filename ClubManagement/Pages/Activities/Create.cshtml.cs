using ClubManagement.Service.DTOs.RequestDTOs.Activity;
using ClubManagement.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.Activities
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IActivityService _activityService;
        private readonly IClubService _clubService;

        public CreateModel(IActivityService activityService, IClubService clubService)
        {
            _activityService = activityService;
            _clubService = clubService;
        }

        [BindProperty]
        public ActivityCreateDTO ActivityCreate { get; set; } = null!;

        public SelectList ClubSelectList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            var clubs = await _clubService.GetAllAsync();
            ClubSelectList = new SelectList(clubs, "ClubId", "ClubName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var clubs = await _clubService.GetAllAsync();
                ClubSelectList = new SelectList(clubs, "ClubId", "ClubName");
                return Page();
            }

            // Validate dates
            if (ActivityCreate.EndDate <= ActivityCreate.StartDate)
            {
                ModelState.AddModelError("ActivityCreate.EndDate", "End date must be after start date.");
                var clubs = await _clubService.GetAllAsync();
                ClubSelectList = new SelectList(clubs, "ClubId", "ClubName");
                return Page();
            }

            await _activityService.CreateAsync(ActivityCreate);
            TempData["SuccessMessage"] = "Activity created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
