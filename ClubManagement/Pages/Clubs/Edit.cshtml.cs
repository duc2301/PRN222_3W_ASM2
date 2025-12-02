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
            ViewData["LeaderId"] = new SelectList(await _serviceProviders.UserService.GetAllAsync(), "UserId", "Email");
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
