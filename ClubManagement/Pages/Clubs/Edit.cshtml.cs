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
    [Authorize(Roles = "Admin,ClubManager")]
    public class EditModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public EditModel(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProviders = serviceProviders;
            _mapper = mapper;
        }

        [BindProperty]
        public ClubResponseDTO Club { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var club = await _serviceProviders.ClubService.GetByIdAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            Club = club;

            if (!User.IsInRole("ClubManager"))
            {
                var leaders = await _serviceProviders.UserService.GetLeadersAsync();
                if (!leaders.Any())
                {
                    ModelState.AddModelError(string.Empty, "No leaders are available to assign.");
                    return Page();
                }
                ViewData["LeaderId"] = new SelectList(leaders, "UserId", "Email", Club.LeaderId);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var allClubs = await _serviceProviders.ClubService.GetAllAsync();
            if (allClubs.Any(c => c.ClubId != Club.ClubId && c.ClubName.Trim().ToLower() == Club.ClubName.Trim().ToLower()))
            {
                ModelState.AddModelError("Club.ClubName", "Tên câu lạc bộ đã tồn tại.");
            }

            if (User.IsInRole("ClubManager"))
            {
                var originalClub = await _serviceProviders.ClubService.GetByIdAsync(Club.ClubId);
                if (originalClub == null)
                {
                    return NotFound();
                }
                Club.LeaderId = originalClub.LeaderId;
            }

            if (!ModelState.IsValid)
            {
                if (!User.IsInRole("ClubManager"))
                {
                    var leaders = await _serviceProviders.UserService.GetLeadersAsync();
                    ViewData["LeaderId"] = new SelectList(leaders, "UserId", "Email", Club.LeaderId);
                }
                return Page();
            }

            var ClubRequest = _mapper.Map<UpdateClubRequestDTO>(Club);
            await _serviceProviders.ClubService.UpdateAsync(ClubRequest);
            if (User.IsInRole("ClubManager"))
            {
                return RedirectToPage("/ClubManager/Index");
            }
            else
            {
                return RedirectToPage("/Clubs/MyClubs");
            }
        }
    }
}
