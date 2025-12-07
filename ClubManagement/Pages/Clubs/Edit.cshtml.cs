using AutoMapper;
using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.Clubs
{
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
            if (id == null)
            {
                return NotFound();
            }

            var club = await _serviceProviders.ClubService.GetByIdAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            Club = club;

            // Only show leader dropdown for non-clubmanagers
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent clubmanager from changing LeaderId
            if (User.IsInRole("ClubManager"))
            {
                // Reload the original LeaderId from DB
                var originalClub = await _serviceProviders.ClubService.GetByIdAsync(Club.ClubId);
                if (originalClub == null)
                {
                    return NotFound();
                }
                Club.LeaderId = originalClub.LeaderId;
            }

            var ClubRequest = _mapper.Map<UpdateClubRequestDTO>(Club);
            await _serviceProviders.ClubService.UpdateAsync(ClubRequest);

            return RedirectToPage("./Index");
        }

        private async Task<bool> ClubExists(int id)
        {
            return await _serviceProviders.ClubService.GetByIdAsync(id) != null;
        }
    }
}
