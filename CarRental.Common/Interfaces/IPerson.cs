namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public int Id { get; set; }
    public int CustomerId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    
}
