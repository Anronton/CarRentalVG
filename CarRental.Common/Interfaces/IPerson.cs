namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public int CustomerId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    
}
