using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public interface ITeamRepository
    {
        void Add(Team team);

        Team? GetById(Guid teamId);
    }
}
