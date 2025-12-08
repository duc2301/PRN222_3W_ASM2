using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ClubManagement.Pages.JoinRequests
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class RejectModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public RejectModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [BindProperty]
        public RejectJoinRequestDTO Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != "ClubManager" && role != "Admin")
                return Forbid();

            var username = User.Identity!.Name;
            var user = await _serviceProviders.UserService.GetByUsernameAsync(username);
            if (user == null) return Unauthorized();

            var request = await _serviceProviders.JoinRequestService.GetByIdAsync(id);
            if (request == null) return NotFound();

            Input = new RejectJoinRequestDTO
            {
                RequestId = id,
                LeaderId = user.UserId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var role = User.FindFirstValue(ClaimTypes.Role);
                if (role != "ClubManager" && role != "Admin")
                    return Forbid();

                var username = User.Identity!.Name;
                var user = await _serviceProviders.UserService.GetByUsernameAsync(username);

                if (user == null)
                    return Unauthorized();

                await _serviceProviders.JoinRequestService
                    .RejectAsync(Input.RequestId, user.UserId, Input.Reason);

                TempData["SuccessMessage"] = "Đã từ chối yêu cầu tham gia!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
    }
}
