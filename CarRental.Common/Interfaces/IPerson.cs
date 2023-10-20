namespace CarRental.Common.Interfaces;

public interface IPerson : IEntity
{
    public string SocialSecurityNumber { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
