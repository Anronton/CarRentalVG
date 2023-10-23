using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

 public interface IBooking : IId
{
    
    public IVehicle Vehicle { get; init; }
    public IPerson Person { get; init; }
    public int Odometer { get; set; }
    public int? Distance { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public double? TotalCost { get; set; }
    public VehicleBookingStatuses VehicleBookingStatus { get; set; }
}
