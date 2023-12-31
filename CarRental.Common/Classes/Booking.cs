﻿using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public int Id { get; set; }
    public IVehicle Vehicle { get; init; }
    public IPerson Person { get; init; }
    public int Odometer { get; set; }
    public int? Distance { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public double? TotalCost { get; set; }
    public VehicleBookingStatuses VehicleBookingStatus { get; set; }

    public Booking(IVehicle vehicle, IPerson person, int odometer, DateTime bookingDate, VehicleBookingStatuses vehicleBookingStatus)
    {
        Vehicle = vehicle;
        Person = person;
        Odometer = odometer;
        BookingDate = bookingDate;
        VehicleBookingStatus = vehicleBookingStatus;
    }
    public Booking()
    {

    }
}
