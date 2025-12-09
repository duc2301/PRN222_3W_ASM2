
using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Pages.Fees
{
    [Authorize(Roles = "Admin,ClubManager")]
    public class CreateModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;

        public CreateModel(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [BindProperty]
        public CreateFeeRequestDTO Fee { get; set; } = new();

        public SelectList ClubList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public async Task OnGetAsync()
        {
            // Lấy danh sách club để fill dropdown
            var clubs = await _serviceProviders.ClubService.GetAllAsync() 
                        ?? new List<ClubResponseDTO>();

            ClubList = new SelectList(clubs, "ClubId", "ClubName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // load lại dropdown
                return Page();
            }

            await _serviceProviders.FeeService.CreateAsync(Fee);

            TempData["msg"] = "Tạo phí thành công!";
            return RedirectToPage("Index");
        }
    }
}
