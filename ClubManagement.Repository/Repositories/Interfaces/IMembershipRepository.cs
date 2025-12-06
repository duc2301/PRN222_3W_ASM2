using ClubManagement.Repository.Basic.Interfaces;
using ClubManagement.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Repository.Repositories.Interfaces
{
    public interface IMembershipRepository :IGenericRepository<Membership>
    {
        Task<(List<Membership> Items, int TotalCount)> GetMembersByClubAsync(
           int clubId,
           string? search,
           string? role,
           string? status,
           int pageIndex,
           int pageSize);
    }
}
