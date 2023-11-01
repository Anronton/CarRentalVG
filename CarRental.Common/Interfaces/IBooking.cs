using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

 public interface IBooking : IId
{
    
    IVehicle Vehicle { get; init; }
    IPerson Person { get; init; }
    int Odometer { get; set; }
    int? Distance { get; set; }
    DateTime BookingDate { get; set; }
    DateTime? ReturnDate { get; set; }
    double? TotalCost { get; set; }
    VehicleBookingStatuses VehicleBookingStatus { get; set; }
}
