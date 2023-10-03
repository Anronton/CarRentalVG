﻿using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    public int CustomerId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public Customer(int customerId, string firstName, string lastName) =>
        (CustomerId, FirstName, LastName) = (customerId, firstName, lastName);
    
}
