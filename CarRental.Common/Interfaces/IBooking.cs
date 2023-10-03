using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

 public interface IBooking
{
    IVehicle Vehicle { get; init; }
    IPerson Person { get; init; }
    int InitialOdometer { get; init; }
    int? ReturnOdometer { get; set; }
    DateTime BookingDate { get; init; }
    DateTime? ReturnDate { get; set; }
    double? TotalCost { get; set; }
    VehicleBookingStatuses VehicleBookingStatus { get; set; }   
}
