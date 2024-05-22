namespace BPM_Auth.DAL.Entities
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public List<TeamMembership> TeamMemberships { get; set; } = new();
    }
}
