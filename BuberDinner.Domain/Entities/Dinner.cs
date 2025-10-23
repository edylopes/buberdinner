


using System.Text.Json.Serialization;
using BuberDinner.Domain.Common;

namespace BuberDinner.Domain.Entities
{
    public class Dinner : AggregateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public int MaxGuests { get; private set; }
        public Guid HostId { get; private set; }

        [JsonIgnore]
        public Host Host { get; private set; } = new();
        protected Dinner() { }
        public List<Guest> Guests { get; private set; } = new();
        public int CurrentGuests { get; private set; }
        public string ImageUrl { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty; // Address where the dinner
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime Date { get; private set; }

        private Dinner(string name, string description, DateTime date, decimal price, string address)
        {
            Name = name;
            Description = description;
            Date = date;
            Price = price;
            Address = address;

        }

        public Dinner Create(string name, string description, DateTime date, decimal price, string address)
          => new(name, description, date, price, address);
        
        public void Desactive()
        {
            IsActive = false;
            MarkUpdated(); 
        }


    }
}
