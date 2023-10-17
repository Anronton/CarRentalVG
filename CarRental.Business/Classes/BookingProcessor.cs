using CarRental.Common.Classes;
using CarRental.Common.Enums;
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

    public IEnumerable<T> GetItems<T>(Expression<Func<T, bool>>? expression = null)
    {
        return _data.Get(expression);
    }

    public void AddItem<T>(T item)
    {
        _data.Add(item);
    }

    public IEnumerable<IPerson> GetPersons(Expression<Func<IPerson, bool>>? expression = null)
    {
        return GetItems(expression);
    } 

    public IPerson? GetPerson(int customerId)
    {
        var persons = _data.Get<IPerson>(p => p.CustomerId == customerId);
        return persons.SingleOrDefault();

    }
    public IEnumerable<IVehicle> GetVehicles(Expression<Func<IVehicle, bool>>? expression = null)
    {
        return GetItems(expression);
    } 


    public IVehicle? GetVehicle(int vehicleId)
    {
        var vehicles = _data.Get<IVehicle>(v => v.Id == vehicleId);
        return vehicles.SingleOrDefault();
    }

    public IVehicle? GetVehicle(string regNo)
    {
        var vehicles = _data.Get<IVehicle>(v => v.RegNo == regNo);
        return vehicles.SingleOrDefault();
    }
    public IEnumerable<IBooking> GetBookings(Expression<Func<IBooking, bool>>? expression = null)
    {
        return GetItems(expression);
    }
    /* public IBooking GetBooking(int vehicleId)
    {

    } */

    //public async Task<IBooking> RentVehicle(int vehicleIdm int customerId){}
    //public IBooking ReturnVehicle(int vehicleId, double distance){}


    public void AddVehicle(string regNo, string make, double odometer, double costKm, VehicleTypes vehicleType)
    {
        IVehicle vehicle;

        if (vehicleType == VehicleTypes.Sedan)
        {
            vehicle = new Car(regNo, make, odometer, costKm, VehicleTypes.Sedan, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Combi)
        {
            vehicle = new Car(regNo, make, odometer, costKm, VehicleTypes.Combi, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Van)
        {
            vehicle = new Car(regNo, make, odometer, costKm, VehicleTypes.Van, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Motorcycle)
        {
            vehicle = new Motorcycle(regNo, make, odometer, costKm, VehicleTypes.Motorcycle, VehicleStatuses.Available);
        }
        else
        {
            throw new Exception();
        }
        AddItem(vehicle);
        
    }
    public void AddCustomer(int customerId, string firstName, string lastName)
    {
        IPerson customer = new Customer(customerId, firstName, lastName);
        AddItem(customer);
    }

    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));

    
    public string[] GetVehicleTypeNames()
    {
        return _data.VehicleTypeNames;
    }

    public VehicleTypes GetVehicleType(string name)
    {
        if (Enum.TryParse(name, true, out VehicleTypes vehicleType))
        {
            return vehicleType;
        }
        return default; //throw new ArgumentException("Invalid vehicle type name")
    }

    public VehicleTypes[] GetVehicleTypes()
    {
        return Enum.GetValues(typeof(VehicleTypes)).Cast<VehicleTypes>().ToArray();
    }
    

    //public VehicleTypes GetVehicleType(string name) => _data.GetVehicleType(name);
        

}
