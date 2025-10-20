namespace BuberDinner.Domain.Entities
{
    public class Host
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; } // Navigation property to User entity
        public List<Dinner> Dinners { get; set; } = new List<Dinner>();
    }
}
