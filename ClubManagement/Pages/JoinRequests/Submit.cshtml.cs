using AutoMapper;
using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.JoinRequests
{
    public class SubmitModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public SubmitModel(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProviders = serviceProviders;
            _mapper = mapper;
        }

        [BindProperty]
        public SubmitJoinRequestDTO Input { get; set; }

        public SelectList Clubs { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var username = User.Identity?.Name;
            var user = await _serviceProviders.UserService.GetByUsernameAsync(username);

            if (user == null)
                return Unauthorized();

            Input = new SubmitJoinRequestDTO
            {
                UserId = user.UserId,
                UserEmail = user.Email
            };

            Clubs = new SelectList(
                await _serviceProviders.ClubService.GetAllAsync(),
                "ClubId",
                "ClubName"
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Clubs = new SelectList(
                        await _serviceProviders.ClubService.GetAllAsync(),
                        "ClubId",
                        "ClubName"
                    );
                    return Page();
                }

                await _serviceProviders.JoinRequestService.SubmitAsync(Input.UserId, Input.ClubId, Input.Note);

                TempData["SuccessMessage"] = "?ã g?i yêu c?u tham gia thành công!";
                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Có l?i x?y ra: {ex.Message}";
                return RedirectToPage();
            }
        }
    }
}

