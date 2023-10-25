using CarRental.Common.Classes; // ph
using CarRental.Common.Enums; // ph
using CarRental.Common.Extensions;
using CarRental.Common.Interfaces;
using System.Linq.Expressions;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;
    
    public BookingProcessor(IData data)
    {
        _data = data;
    }

    public IEnumerable<T> GetItems<T>(Expression<Func<T, bool>>? expression = null) //where T : class
    {
        return _data.Get(expression);
    }

    public void AddItem<T>(T item) //where T : class
    {
        _data.Add(item);
    }

    public IEnumerable<IBooking> GetBookings(Expression<Func<IBooking, bool>>? expression = null)
    {
        return GetItems(expression);
    }

    private IBooking? GetBookingByVehicleId(int vehicleId)
    {
        return _data.Get<IBooking>(b => b.Vehicle.Id == vehicleId && b.VehicleBookingStatus == VehicleBookingStatuses.Open).FirstOrDefault();
    }
    
    public IEnumerable<IPerson> GetPersons(Expression<Func<IPerson, bool>>? expression = null)
    {
        return GetItems(expression);
    } 

    public IPerson? GetPerson(int customerId)
    {
        return _data.Get<IPerson>(p => p.Id == customerId).SingleOrDefault();
    }

    public IEnumerable<IVehicle> GetVehicles(Expression<Func<IVehicle, bool>>? expression = null)
    {
        return GetItems(expression);
    } 

    public IVehicle? GetVehicle(int vehicleId)
    {
        var vehicles = _data.Get<IVehicle>(v => v.Id == vehicleId);
        return vehicles.FirstOrDefault();
    }

    public async Task<IBooking?> RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            var vehicle = await Task.Run(() => GetVehicle(vehicleId));
            var customer = await Task.Run(() => GetPerson(customerId));

            if (vehicle != null && customer != null)
            {
                var initialOdometer = vehicle.Odometer;

                var booking = new Booking(vehicle, customer, initialOdometer, DateTime.Now, VehicleBookingStatuses.Open);
                vehicle.VehicleStatus = VehicleStatuses.Booked;

                await Task.Delay(5000);

                AddItem(booking);
                return booking;
            }

            return null;
        }
        catch (Exception ex)
        {
             throw new Exception("Rental failed", ex);
        }
    }

    public void ReturnVehicle(int vehicleId, int distance)
    {
        var booking = GetBookingByVehicleId(vehicleId) ?? throw new Exception("Booking not found");
        var vehicle = GetVehicle(vehicleId) ?? throw new Exception("Vehicle not found");

        if (booking.VehicleBookingStatus == VehicleBookingStatuses.Open)
        {
            booking.VehicleBookingStatus = VehicleBookingStatuses.Closed;
            vehicle.VehicleStatus = VehicleStatuses.Available;
            vehicle.Odometer += distance;
            booking.ReturnDate = DateTime.Now;
            booking.Distance = distance;

            double dayCost = vehicle.DayCost();
            booking.CalculateTotalCost(dayCost, vehicle.CostKm);
        }
        else
        {
            throw new Exception("Booking is not open and cannot be returned.");
        }

    }

    public void AddVehicle(string regNo, string make, int odometer, double costKm, VehicleTypes vehicleType)
    {
        int nextVehicleId = _data.NextVehicleId;
        IVehicle? vehicle;

        if (vehicleType == VehicleTypes.Sedan)
        {
            vehicle = new Vehicle(default, regNo, make, odometer, costKm, VehicleTypes.Sedan, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Combi)
        {
            vehicle = new Vehicle(default, regNo, make, odometer, costKm, VehicleTypes.Combi, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Van)
        {
            vehicle = new Vehicle(default, regNo, make, odometer, costKm, VehicleTypes.Van, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Motorcycle)
        {
            vehicle = new Vehicle(default, regNo, make, odometer, costKm, VehicleTypes.Motorcycle, VehicleStatuses.Available);
        }
        else
        {
            throw new Exception();
        }
        
        vehicle.Id = nextVehicleId;
        AddItem(vehicle);
        
    }
    public void AddCustomer(string sociaSecurityNumber, string firstName, string lastName)
    {
        int nextPersonId = _data.NextPersonId;
        IPerson? customer = new Customer(default, sociaSecurityNumber, lastName, firstName)
        {
            Id = nextPersonId
        };
        AddItem(customer);
    }

    public string[] VehicleBookingStatusNames => _data.VehicleBookingStatusNames;
    public string[] VehicleStatusNames => _data.VehicleStatusNames;
    public string[] VehicleTypeNames => _data.VehicleTypeNames; // vill använda dessa 3
    public VehicleTypes GetVehicleType(string name) => _data.GetVehicleType(name);
    
    public VehicleTypes[] GetVehicleTypes()
    {
        return Enum.GetValues(typeof(VehicleTypes)).Cast<VehicleTypes>().ToArray();
    }

}
