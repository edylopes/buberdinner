namespace BuberDinner.Domain.Entities
{
    public class Host
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; } // Foreign key to User entity
        public User User { get; set; } // Navigation property to User entity
        public List<Dinner> Dinners { get; set; } = new List<Dinner>();

        // Additional properties and methods can be added as needed
    }
}
