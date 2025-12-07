using AutoMapper;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.Pages.Clubs
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public DeleteModel(IServiceProviders serviceProviders, IMapper mapper)
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
            else
            {
                Club = club;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _serviceProviders.ClubService.GetByIdAsync(id);
            if (club != null)
            {
                await _serviceProviders.ClubService.DeleteAsync(id);
            }

            return RedirectToPage("./Index");
        }
    }
}
