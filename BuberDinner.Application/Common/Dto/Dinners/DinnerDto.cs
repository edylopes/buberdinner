namespace BuberDinner.Application.Common.Dto.Dinners;



public record DinnerDto
{
    public string Name { get; set; } = string.Empty;
    public int MaxGuests { get; set; }
    public int CurrentGuests { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool Active { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
    public string HostName { get; set; } = string.Empty;
    public Guid HostId { get; set; }
    public Guid Id { get; set; }
    public List<GuestDto> Guests { get; set; } = new();
}
public record GuestDto(string Name);
