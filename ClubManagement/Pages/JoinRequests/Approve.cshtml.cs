using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.JoinRequests
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class ApproveModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public ApproveModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var username = User.Identity!.Name;
                var user = await _serviceProviders.UserService.GetByUsernameAsync(username);

                if (user == null)
                    return Unauthorized();

                await _serviceProviders.JoinRequestService.ApproveAsync(id, user.UserId);

                TempData["SuccessMessage"] = "Đã duyệt yêu cầu và thêm thành viên vào câu lạc bộ!";
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

