using ClubManagement.Repository.Basic;
using ClubManagement.Repository.DbContexts;
using ClubManagement.Repository.Models;
using ClubManagement.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Repository.Repositories
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(ClubManagementContext context) : base(context)
        {
        }

        public async Task<(List<Membership> Items, int TotalCount)> GetMembersByClubAsync(
          int clubId,
          string? search,
          string? role,
          string? status,
          int pageIndex,
          int pageSize)
        {
            var query = _context.Memberships
                .Include(m => m.User)
                .Where(m => m.ClubId == clubId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(m =>
                    m.User.FullName.ToLower().Contains(search) ||
                    m.User.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(m => m.Role == role);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(m => m.Status == status);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(m => m.User.FullName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
