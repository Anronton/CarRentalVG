using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    public int Id { get; set; }
    public string SocialSecurityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Customer(int id, string socialSecurityNumber, string firstName, string lastName)
    {
        Id = id;
        SocialSecurityNumber = socialSecurityNumber;
        FirstName = firstName;
        LastName = lastName;
    }
    public Customer()
    {
        
    }
}
