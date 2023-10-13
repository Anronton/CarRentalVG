using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IData
{
    public IEnumerable<IPerson> GetPersons();
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default);
    public IEnumerable<IBooking> GetBookings();
    void AddCustomer(IPerson customer);
}
