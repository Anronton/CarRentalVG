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

    public IEnumerable<T> GetItems<T>(Expression<Func<T, bool>>? expression = null) //where T : class
    {
        return _data.Get(expression);
    }

    public void AddItem<T>(T item) //where T : class
    {
        _data.Add(item);
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

    public IVehicle? GetVehicle(string regNo)
    {
        var vehicles = _data.Get<IVehicle>(v => v.RegNo == regNo);
        return vehicles.FirstOrDefault();
    }
    public IEnumerable<IBooking> GetBookings(Expression<Func<IBooking, bool>>? expression = null)
    {
        return GetItems(expression);
    }

    public IBooking? GetBooking(int bookingId)
    {
        var bookings = _data.Get<IBooking>(b => b.Id == bookingId);
        return bookings.FirstOrDefault();
    }


    //Denna ska vi jobba med näst! Testar med att göra den icke async för att testa så att all logik fungerar.
    //public async Task<IBooking> RentVehicle(int vehicleId int customerId)
    //{
    //  await Task.Delay(10000) // fakeväntar 10sekunder
    //}

    //Vi gör en temporär Dictionary
    //private Dictionary<IVehicle, IPerson?> vehicleCustomerDictionary = new Dictionary<IVehicle, IPerson?>();
    //

    //Temporär metod för dictionaryn
    //public Dictionary<IVehicle, IPerson?> GetVehicleCustomerDictionary()
    //{
    //    return vehicleCustomerDictionary;
    //}
    //

    //gör en temporär RentVehicle innan vi gör async metoden.
    //public void RentVehicle(int vehicleId, int customerId)
    //{
    //    var vehicle = GetVehicle(vehicleId);
    //    var customer = GetPerson(customerId);

    //    if (vehicle != null && customer != null)
    //    {

    //        vehicleCustomerDictionary[vehicle] = customer;


    //        var booking = new Booking(vehicle, customer, (int)vehicle.Odometer, DateTime.Now, VehicleBookingStatuses.Open);
    //        vehicle.VehicleStatus = VehicleStatuses.Booked;
    //        AddItem(booking);
    //    }

    //}
    //

    /*public IBooking? ReturnVehicle(int vehicleId, int distance)
    {

        var booking = _data.Get<IBooking>(b => b.Id == vehicleId).FirstOrDefault();

        if (booking == null)
        {
            throw new Exception("Booking not found");
        }

        if (booking.VehicleBookingStatus != VehicleBookingStatuses.Open)
        {
            throw new Exception("Booking is not in an open state");
        }

        booking.Distance = distance;

        _data.Add(booking);

        var vehicle = _data.Get<IVehicle>(v => v.Id == vehicleId).FirstOrDefault();

        if (vehicle != null)
        {
            vehicle.VehicleStatus = VehicleStatuses.Available;
            vehicle.Odometer = (double)(booking.Distance ?? vehicle.Odometer);

            _data.Add(vehicle);
        }
        else
        {
            throw new Exception("Vehicle not found");
        }
        return booking;
    }*/

    public void AddVehicle(string regNo, string make, double odometer, double costKm, VehicleTypes vehicleType)
    {
        int nextVehicleId = _data.NextVehicleId;
        IVehicle? vehicle;

        if (vehicleType == VehicleTypes.Sedan)
        {
            vehicle = new Car(default, regNo, make, odometer, costKm, VehicleTypes.Sedan, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Combi)
        {
            vehicle = new Car(default, regNo, make, odometer, costKm, VehicleTypes.Combi, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Van)
        {
            vehicle = new Car(default, regNo, make, odometer, costKm, VehicleTypes.Van, VehicleStatuses.Available);
        }
        else if (vehicleType == VehicleTypes.Motorcycle)
        {
            vehicle = new Motorcycle(default, regNo, make, odometer, costKm, VehicleTypes.Motorcycle, VehicleStatuses.Available);
        }
        else
        {
            throw new Exception();
        }
        //if (customerId.HasValue)
        //{
        //    var customer = GetPerson(customerId.Value);
        //}
        vehicle.Id = nextVehicleId;
        AddItem(vehicle);
        
    }
    public void AddCustomer(string sociaSecurityNumber, string firstName, string lastName)
    {
        int nextPersonId = _data.NextPersonId;
        IPerson? customer = new Customer(default, sociaSecurityNumber, lastName, firstName);
        customer.Id = nextPersonId;
        AddItem(customer);
    }

    public string[] VehicleStatusNames => _data.VehicleStatusNames;
    public string[] VehicleTypeNames => _data.VehicleTypeNames;
    public VehicleTypes GetVehicleType(string name) => _data.GetVehicleType(name);

    public string[] GetVehicleTypeNames()
    {
        return _data.VehicleTypeNames;
    }

    public VehicleTypes[] GetVehicleTypes()
    {
        return Enum.GetValues(typeof(VehicleTypes)).Cast<VehicleTypes>().ToArray();
    }

}
