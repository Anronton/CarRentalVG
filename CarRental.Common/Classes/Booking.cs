using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public IVehicle Vehicle { get; init; }
    public IPerson Person { get; init; }
    public int InitialOdometer { get; init; }
    public int? ReturnOdometer { get; set; }
    public DateTime BookingDate { get; init; }
    public DateTime? ReturnDate { get; set; }
    public double? TotalCost { get; set; }
    public VehicleBookingStatuses VehicleBookingStatus { get; set; }

    public Booking(IVehicle vehicle, IPerson person, int initialOdometer, DateTime bookingDate, VehicleBookingStatuses vehicleBookingStatus)
    {
        Vehicle = vehicle;
        Person = person;
        InitialOdometer = initialOdometer;
        BookingDate = bookingDate;
        VehicleBookingStatus = vehicleBookingStatus;
    }

   

    public void CalculateTotalCost()
    {
        if (ReturnDate.HasValue)
        {
            double odometerDifference = ReturnOdometer.HasValue ? ReturnOdometer.Value - InitialOdometer : 0;
            TotalCost = (ReturnDate.Value - BookingDate).TotalDays * Vehicle.DayCost(Vehicle.VehicleType) + odometerDifference * Vehicle.CostKm;
        }
        else
        {
            TotalCost = null;
        }
    }

    public void CloseBooking(int returnOdometer, DateTime returnDate)
    {
        ReturnOdometer = returnOdometer;
        ReturnDate = returnDate;
        CalculateTotalCost();
        VehicleBookingStatus = VehicleBookingStatuses.Closed;
    }
}
