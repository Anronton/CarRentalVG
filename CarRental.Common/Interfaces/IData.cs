using CarRental.Common.Classes;
using CarRental.Common.Enums;
using System.Linq.Expressions;

namespace CarRental.Common.Interfaces;

public interface IData
{
    List<T> Get<T>(Expression<Func<T, bool>>? expression);
    T? Single<T>(Expression<Func<T, bool>>? expression);
    void Add<T>(T item);

    int NextVehicleId { get; }
    int NextPersonId { get; }
    int NextBookingId { get; }
    IBooking? RentVehicle(int vehicleId, int customerId);
    IBooking? ReturnVehicle(int vehicleId);

    public string[] VehicleBookingStatusNames => Enum.GetNames(typeof(VehicleBookingStatuses));
    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name) =>
        (VehicleTypes)Enum.Parse(typeof(VehicleTypes), name, true);
}
