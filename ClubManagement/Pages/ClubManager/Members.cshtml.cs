using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.ClubManager
{
    [Authorize(Roles = "ClubManager")]
    public class MembersModel : PageModel
    {
        private readonly IServiceProviders _services;

        public MembersModel(IServiceProviders services)
        {
            _services = services;
        }

        // ----- FILTER & PHÂN TRANG -----
        [BindProperty(SupportsGet = true)]
        public int ClubId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? RoleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public ClubResponseDTO? Club { get; set; }

        public PagedResult<ClubMemberResponseDTO> Members { get; set; }
            = new PagedResult<ClubMemberResponseDTO>();

        // ----- INPUT CHỈNH SỬA THÀNH VIÊN -----
        public class EditMemberInput
        {
            public int MembershipId { get; set; }
            public string Role { get; set; } = null!;
            public string Status { get; set; } = null!;
        }

        [BindProperty]
        public EditMemberInput EditMember { get; set; } = new();

        // GET: hiển thị danh sách thành viên
        public async Task<IActionResult> OnGetAsync()
        {
            if (ClubId <= 0)
                return BadRequest("ClubId is required");

            await LoadDataAsync();
            if (Club == null)
                return NotFound();

            return Page();
        }

        // POST: cập nhật Role + Status
        public async Task<IActionResult> OnPostUpdateMemberAsync()
        {
            if (ClubId <= 0)
                return BadRequest("ClubId is required");

            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                if (Club == null)
                    return NotFound();

                return Page();
            }

            var updated = await _services.MembershipService.UpdateMemberRoleStatusAsync(
                EditMember.MembershipId,
                ClubId,
                EditMember.Role,
                EditMember.Status);

            if (!updated)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy thành viên hoặc không thuộc CLB này.");
                await LoadDataAsync();
                if (Club == null)
                    return NotFound();

                return Page();
            }

            // Quay lại đúng trạng thái filter + trang hiện tại
            return RedirectToPage("./Members", new
            {
                ClubId,
                Search,
                RoleFilter,
                StatusFilter,
                PageIndex
            });
        }

        // Hàm dùng chung để load header + danh sách
        private async Task LoadDataAsync()
        {
            Club = await _services.ClubService.GetByIdAsync(ClubId);

            if (PageIndex < 1) PageIndex = 1;
            const int pageSize = 10;

            Members = await _services.MembershipService.GetClubMembersAsync(
                ClubId,
                Search,
                RoleFilter,
                StatusFilter,
                PageIndex,
                pageSize);
        }
    }
}
