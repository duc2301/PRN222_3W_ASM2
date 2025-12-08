using ClubManagement.Service.DTOs.RequestDTOs;
using ClubManagement.Service.DTOs.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Service.Services.Interfaces
{
    public interface IMembershipService
    {
        Task UpdateMemberAsync(UpdateMemberRequestDTO request);

        Task<PagedResult<ClubMemberResponseDTO>> GetClubMembersAsync(
            int clubId,
            string? search,
            string? role,
            string? status,
            int pageIndex,
            int pageSize);

        Task<bool> UpdateMemberRoleStatusAsync(
           int membershipId,
           int clubId,
           string role,
           string status);
    }
}


    
