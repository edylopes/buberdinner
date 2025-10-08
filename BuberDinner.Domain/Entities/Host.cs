namespace BuberDinner.Domain.Entities.Users
{
    public class Host
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } // Navigation property to User entity
        public List<Dinner> Dinners { get; set; } = new List<Dinner>();
    }
}
