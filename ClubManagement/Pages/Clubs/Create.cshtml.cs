using AutoMapper;
using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.Clubs
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public CreateModel(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProviders = serviceProviders;
            _mapper = mapper;
        }

        [BindProperty]
        public ClubResponseDTO Club { get; set; } = default!;

        public SelectList LeaderList { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            var leaders = await _serviceProviders.UserService.GetLeadersAsync();
            if (!leaders.Any())
            {
                ModelState.AddModelError(string.Empty, "No leaders are available to assign.");
            }
            ViewData["LeaderId"] = new SelectList(leaders, "UserId", "Email");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var allClubs = await _serviceProviders.ClubService.GetAllAsync();
            if (allClubs.Any(c => c.ClubName.Trim().ToLower() == Club.ClubName.Trim().ToLower()))
            {
                ModelState.AddModelError("Club.ClubName", "Tên câu lạc bộ đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                var leaders = await _serviceProviders.UserService.GetLeadersAsync();
                ViewData["LeaderId"] = new SelectList(leaders, "UserId", "Email", Club.LeaderId);
                return Page();
            }

            var clubRequest = _mapper.Map<CreateClubRequestDTO>(Club);
            await _serviceProviders.ClubService.CreateAsync(clubRequest);

            return RedirectToPage("./MyClubs");
        }
    }
}
