namespace CarRental.Common.Interfaces;

public interface IPerson : IId
{
    string SocialSecurityNumber { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
}
