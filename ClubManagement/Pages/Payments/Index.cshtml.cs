using ClubManagement.Repository.Models;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubManagement.Pages.Payments
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class IndexModel : PageModel
    {
        private readonly IServiceProviders _services;

        public IndexModel(IServiceProviders services)
        {
            _services = services;
        }

        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Kiểm tra và cập nhật các payment quá hạn
            await _services.PaymentService.CheckAndUpdateExpiredPaymentsAsync();

            var allPayments = await _services.PaymentService.GetAllAsync();

            // Nếu là ClubManager, chỉ hiển thị payments của clubs mà họ là leader
            if (User.IsInRole("ClubManager"))
            {
                var username = User.Identity?.Name;
                var user = await _services.UserService.GetByUsernameAsync(username);

                if (user == null) return Unauthorized();

                var clubs = await _services.ClubService.GetAllAsync();
                var myClubIds = clubs.Where(c => c.LeaderId == user.UserId).Select(c => c.ClubId).ToList();

                // Lấy fees của các clubs mà user là leader
                var allFees = new List<ClubManagement.Service.DTOs.ResponseDTOs.FeeResponseDTO>();
                foreach (var clubId in myClubIds)
                {
                    var fees = await _services.FeeService.GetByClubAsync(clubId);
                    allFees.AddRange(fees);
                }
                var myFeeIds = allFees.Select(f => f.FeeId).ToList();

                // Filter payments theo feeIds
                allPayments = allPayments.Where(p => myFeeIds.Contains(p.FeeId));
            }

            Payments = allPayments
    .OrderBy(p => p.Status == "Pending" ? 0 :
                  p.Status == "Unpaid" ? 1 :
                  p.Status == "Paid" ? 2 :
                  p.Status == "Expired" ? 3 : 4)
    .ThenBy(p => p.Fee.DueDate)
    .ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostConfirmPaymentAsync(int id)
        {
            var payment = await _services.PaymentService.GetByIdAsync(id);

            if (payment == null)
            {
                TempData["error"] = "Không tìm thấy thanh toán này.";
                return RedirectToPage();
            }

            // Kiểm tra quyền: ClubManager chỉ xác nhận payment của clubs mà họ là leader
            if (User.IsInRole("ClubManager"))
            {
                var username = User.Identity?.Name;
                var user = await _services.UserService.GetByUsernameAsync(username);

                if (user == null) return Unauthorized();

                var clubs = await _services.ClubService.GetAllAsync();
                var myClubIds = clubs.Where(c => c.LeaderId == user.UserId).Select(c => c.ClubId).ToList();

                // Lấy fee của payment này
                var allFees = new List<ClubManagement.Service.DTOs.ResponseDTOs.FeeResponseDTO>();
                foreach (var clubId in myClubIds)
                {
                    var fees = await _services.FeeService.GetByClubAsync(clubId);
                    allFees.AddRange(fees);
                }
                var myFeeIds = allFees.Select(f => f.FeeId).ToList();

                if (!myFeeIds.Contains(payment.FeeId))
                {
                    TempData["error"] = "Bạn không có quyền xác nhận thanh toán này.";
                    return RedirectToPage();
                }
            }

            if (payment.Status != "Pending")
            {
                TempData["error"] = "Chỉ có thể xác nhận các thanh toán đang chờ xác nhận.";
                return RedirectToPage();
            }

            try
            {
                await _services.PaymentService.MarkAsPaidAsync(id);
                TempData["msg"] = "Xác nhận thanh toán thành công!";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            // Kiểm tra xem có referrer từ Fees/Details không
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/Fees/Details"))
            {
                // Lấy feeId từ payment để redirect về đúng trang Details
                var feeId = payment.FeeId;
                return RedirectToPage("/Fees/Details", new { id = feeId });
            }

            return RedirectToPage();
        }
    }
}

