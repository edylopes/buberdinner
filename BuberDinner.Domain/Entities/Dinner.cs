namespace BuberDinner.Domain.Entities
{
    public class Dinner
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxGuests { get; set; }
        public Guid HostId { get; set; } // Foreign key to Host entity
        public Host Host { get; set; } // Navigation property to Host entity

        // public List<Guest> Guests { get; set; } = new List<Guest>();
        public int CurrentGuests { get; set; }
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the dinner
        public string Address { get; set; } = string.Empty; // Address where the dinner
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } // Price per guest
        public bool IsActive { get; set; } = true; // Indicates if the dinner is currently active
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // When the dinner was created
        public DateTime Date { get; set; }
    }
}
