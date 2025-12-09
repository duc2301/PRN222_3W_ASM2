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
    public class JoinRequestRepository
    : GenericRepository<JoinRequest>, IJoinRequestRepository
    {
        public JoinRequestRepository(ClubManagementContext context) : base(context) { }

        // Sửa lại GetAllAsync để include User và Club
        public new async Task<IEnumerable<JoinRequest>> GetAllAsync()
        {
            return await _context.JoinRequests
                .Include(j => j.User)
                .Include(j => j.Club)
                .ToListAsync();
        }

        public async Task<List<JoinRequest>> GetByUserAsync(int userId)
        {
            return await _context.JoinRequests
                .Include(j => j.User)
                .Include(j => j.Club)
                .Where(j => j.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<JoinRequest>> GetByClubAsync(int clubId)
        {
            return await _context.JoinRequests
                .Include(j => j.User)
                .Include(j => j.Club)
                .Where(j => j.ClubId == clubId)
                .ToListAsync();
        }

        public async Task<JoinRequest?> GetByIdAsync(int requestId)
        {
            return await _context.JoinRequests
                .Include(j => j.User)
                .Include(j => j.Club)
                .FirstOrDefaultAsync(j => j.RequestId == requestId);
        }

        public async Task<JoinRequest?> GetPendingRequestAsync(int userId, int clubId)
        {
            return await _context.JoinRequests
                .Include(j => j.User)
                .Include(j => j.Club)
                .FirstOrDefaultAsync(jr => jr.UserId == userId
                                        && jr.ClubId == clubId
                                        && jr.Status == "Pending");
        }
    }
}