using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            public void OnGet()
            {
            }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                    return Page();

                var userId = int.Parse(User.FindFirst("UserId")!.Value);

            await _serviceProviders.FeeService.CreateAsync(Fee);

            return RedirectToPage("Index");
            }
        }
    }

