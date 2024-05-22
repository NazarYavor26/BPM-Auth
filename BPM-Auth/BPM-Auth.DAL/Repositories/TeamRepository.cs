using BPM_Auth.DAL.DbContexts;
using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _db;

        public TeamRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Team team)
        {
            _db.Teams.Add(team);
            _db.SaveChanges();
        }

        public Team? GetById(Guid teamId)
        {
            return _db.Teams.FirstOrDefault(x => x.TeamId == teamId);
        }
    }
}
