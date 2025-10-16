


using System.Text.Json.Serialization;
using BuberDinner.Domain.Common;

namespace BuberDinner.Domain.Entities
{
    public class Dinner : AggregateRoot
    {
        public string Name { get; set; } = string.Empty;
        public int MaxGuests { get; set; }
        public Guid HostId { get; set; }

        [JsonIgnore]
        public Host Host { get; set; } // Navigation property to Host 
        public Dinner() { }
        public List<Guest> Guests { get; set; } = new List<Guest>();
        public int CurrentGuests { get; set; }
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the dinner
        public string Address { get; set; } = string.Empty; // Address where the dinner
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } // Price per guest
        public bool IsActive { get; set; } = true; // Indicates if the dinner is currently active
        public DateTime Date { get; set; }
    }
}
