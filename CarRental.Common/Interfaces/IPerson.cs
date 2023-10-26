namespace CarRental.Common.Interfaces;

public interface IPerson : IId
{
    public string SocialSecurityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
