using AutoMapper;
using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.ServiceProviders.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubManagement.Pages.Clubs
{
    public class CreateModel : PageModel
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly IMapper _mapper;

        public CreateModel(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProviders = serviceProviders;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["LeaderId"] = new SelectList(await _serviceProviders.UserService.GetAllAsync(), "UserId", "Email");
            return Page();
        }

        [BindProperty]
        public ClubResponseDTO Club { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var clubRequest = _mapper.Map<CreateClubRequestDTO>(Club);

            await _serviceProviders.ClubService.CreateAsync(clubRequest);

            return RedirectToPage("./Index");
        }
    }
}
