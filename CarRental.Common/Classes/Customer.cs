using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    public int Id { get; set; }
    public string SocialSecurityNumber { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public Customer(int id, string socialSecurityNumber, string firstName, string lastName)
    {
        Id = id;
        SocialSecurityNumber = socialSecurityNumber;
        FirstName = firstName;
        LastName = lastName;
    }
}
