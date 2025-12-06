using ClubManagement.Repository.UnitOfWork.Interface;
using ClubManagement.Service.DTOs.ResponseDTOs;
using ClubManagement.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Service.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembershipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<ClubMemberResponseDTO>> GetClubMembersAsync(
            int clubId,
            string? search,
            string? role,
            string? status,
            int pageIndex,
            int pageSize)
        {
            var (memberships, totalCount) = await _unitOfWork.MembershipRepository
                .GetMembersByClubAsync(clubId, search, role, status, pageIndex, pageSize);

            var items = memberships.Select(m => new ClubMemberResponseDTO
            {
                MembershipId = m.MembershipId,
                UserId = m.UserId,
                FullName = m.User.FullName,
                Email = m.User.Email,
                Department = m.User.Department,
                Role = m.Role,
                Status = m.Status,
                JoinedAt = m.JoinedAt
            }).ToList();

            return new PagedResult<ClubMemberResponseDTO>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateMemberRoleStatusAsync(
           int membershipId,
           int clubId,
           string role,
           string status)
        {
            var membership = await _unitOfWork.MembershipRepository.GetByIdAsync(membershipId);
            if (membership == null || membership.ClubId != clubId)
            {
                return false;
            }

            membership.Role = role;
            membership.Status = status;

            _unitOfWork.MembershipRepository.Update(membership);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
