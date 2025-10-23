namespace BuberDinner.Contracts.Dinners;


public record DinnerRequest(string Name, string Description, DateTime Date, decimal Price, string Address);