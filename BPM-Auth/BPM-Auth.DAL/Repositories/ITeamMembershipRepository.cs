using BPM_Auth.DAL.Entities;

namespace BPM_Auth.DAL.Repositories
{
    public interface ITeamMembershipRepository
    {
        void Add(TeamMembership teamMembership);
    }
}
