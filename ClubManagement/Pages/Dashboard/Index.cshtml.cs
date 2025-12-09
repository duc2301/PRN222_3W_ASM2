using ClubManagement.Service.DTOs.ResponseDTOs.Dashboard;
using ClubManagement.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Dashboard
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class IndexModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public IndexModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public CompleteDashboardDTO Dashboard { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            Dashboard = await _dashboardService.GetCompleteDashboardAsync();
            return Page();
        }
    }
}
