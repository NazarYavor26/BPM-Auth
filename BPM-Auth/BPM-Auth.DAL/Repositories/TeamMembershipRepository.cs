using BPM_Auth.DAL.DbContexts;
using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public class TeamMembershipRepository : ITeamMembershipRepository
    {
        private readonly AppDbContext _db;

        public TeamMembershipRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(TeamMembership teamMembership)
        {
            _db.TeamMemberships.Add(teamMembership);
            _db.SaveChanges();
        }
    }
}
