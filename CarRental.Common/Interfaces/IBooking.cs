using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

 public interface IBooking
{
    public int Id { get; set; }
    public IVehicle Vehicle { get; init; }
    public IPerson Person { get; init; }
    public int InitialOdometer { get; init; }
    public int? ReturnOdometer { get; set; }
    public DateTime BookingDate { get; init; }
    public DateTime? ReturnDate { get; set; }
    public double? TotalCost { get; set; }
    public VehicleBookingStatuses VehicleBookingStatus { get; set; }

    public void RentVehicle(IVehicle vehicle, IPerson person, int initialOdometer, DateTime bookingDate);
    public void ReturnVehicle(int returnOdometer, DateTime returnDate);
}
