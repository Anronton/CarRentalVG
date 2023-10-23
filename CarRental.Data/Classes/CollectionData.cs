using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Common.Extensions;
using System.Linq.Expressions;

namespace CarRental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new();
    readonly List<IVehicle> _vehicles = new();
    readonly List<IBooking> _bookings = new();

   
    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(b => b.Id) + 1;
    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(b => b.Id) + 1;
    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(b => b.Id) + 1;

    public CollectionData() => SeedData();

    void SeedData()
    {
        //Customers
        _persons.Add(new Customer(1, "960321", "Jan", "Deg"));
        _persons.Add(new Customer(2, "690101", "John", "Doe"));
        _persons.Add(new Customer(3, "721111", "Jane", "Smith"));

        //Vehicles
        _vehicles.Add(new Car(1, "SIX666", "Volvo", 1000, 2.5, VehicleTypes.Combi, VehicleStatuses.Available));
        _vehicles.Add(new Motorcycle(2, "ABC123", "Yamaha", 3000, 1, VehicleTypes.Motorcycle, VehicleStatuses.Available));
        _vehicles.Add(new Car(3, "BKJ142", "Ford", 2500, 2, VehicleTypes.Van, VehicleStatuses.Available));
        _vehicles.Add(new Car(4, "BLZ420", "BMW", 5000, 5, VehicleTypes.Sedan, VehicleStatuses.Available));
        _vehicles.Add(new Car(5, "ORT141", "Saab", 10000, 1.5, VehicleTypes.Combi, VehicleStatuses.Available));

        //bookings

        IBooking? booking = RentVehicle(2, 3);
        IVehicle? vehicle = booking.Vehicle;
        double dayCost = vehicle.DayCost();
        booking.Distance = 350;

        ReturnVehicle(2);
    }



    public IBooking? RentVehicle(int VehicleId, int customerId)
    {
        IVehicle? vehicle = _vehicles.SingleOrDefault(v => v.Id == VehicleId);
        IPerson? customer = _persons.SingleOrDefault(p => p.Id == customerId);

        if (vehicle != null && customer != null)
        {
            if(vehicle.VehicleStatus == VehicleStatuses.Available)
            {
                int initialOdometer = vehicle.Odometer;

                IBooking booking = new Booking(vehicle, customer, initialOdometer, DateTime.Now, VehicleBookingStatuses.Open);

                vehicle.VehicleStatus = VehicleStatuses.Booked;

                booking.Id = NextBookingId;
                _bookings.Add(booking);
                return booking;
            }
            else
            {
                throw new Exception("Vehicle not available for rent");
            }
        }
        return null;
    }

    public IBooking? ReturnVehicle(int VehicleId)
    {
        IVehicle? vehicle = _vehicles.SingleOrDefault(v => v.Id == VehicleId);

        if (vehicle != null)
        {
            if (vehicle.VehicleStatus == VehicleStatuses.Booked)
            {
                IBooking? booking = _bookings.SingleOrDefault(b => b.Vehicle ==  vehicle && b.VehicleBookingStatus == VehicleBookingStatuses.Open);

                if (booking != null)
                {
                    booking.VehicleBookingStatus = VehicleBookingStatuses.Closed;

                    vehicle.VehicleStatus = VehicleStatuses.Available;

                    int distance = booking.Distance ?? 0;
                    vehicle.Odometer += distance;
                    booking.ReturnDate = DateTime.Now;

                    double dayCost = vehicle.DayCost();
                    booking.CalculateTotalCost(dayCost, vehicle.CostKm);

                    return booking;
                }
                else throw new Exception("Booking not found");
            }
            else {
                throw new Exception();
            }
            
        }
        return null;
    }

    public void Add<T>(T item)
    {
        if (item is IVehicle vehicle)
        {
            vehicle.Id = NextVehicleId;
            _vehicles.Add(vehicle);
        }
        if (item is IPerson person)
        {
            person.Id = NextPersonId;
            _persons.Add(person);
        }
        if (item is IBooking booking)
        {
            booking.Id = NextBookingId;
            _bookings.Add(booking);
        }
    }

    public List<T> Get<T>(Expression<Func<T, bool>>? expression) // tänk över Expression, jonas använder inte den, det har med laddningstid/kompilering att göra, mest praktiskt med väldigt mycket kod, reflektera över det!
    {
        if (typeof(T) == typeof(IPerson))
        {
            if (expression != null)
            {
                return _persons.OfType<T>().Where(expression.Compile()).ToList();
            }
            return _persons.OfType<T>().ToList();
        }
        else if (typeof(T) == typeof(IVehicle))
        {
            if (expression != null)
            {
                return _vehicles.OfType<T>().Where(expression.Compile()).ToList();
            }
            return _vehicles.OfType<T>().ToList();
        }
        else if (typeof(T) == typeof(IBooking))
        {
            if (expression != null)
            {
                return _bookings.OfType<T>().Where(expression.Compile()).ToList();
            }
            return _bookings.OfType<T>().ToList();
        }
        return new List<T>();
    }

    public T? Single<T>(Expression<Func<T, bool>>? expression) // tänk över Expression, jonas använder inte den.
    {
        if (typeof(T) == typeof(IPerson))
        {
            if (expression != null)
            {
                return _persons.OfType<T>().SingleOrDefault(expression.Compile());
            }
            return default;
        }
        else if (typeof(T) == typeof(IVehicle))
        {
            if (expression != null)
            {
                return _vehicles.OfType<T>().SingleOrDefault(expression.Compile());
            }
            return default;
        }
        else if (typeof(T) == typeof(IBooking))
        {
            if(expression != null)
            {
                return _bookings.OfType<T>().SingleOrDefault(expression.Compile());
            }
            return default;
        }
        return default;
    }
        
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if(status != default)
        {
            return _vehicles.Where(v => v.VehicleStatus == status);
        }
        return _vehicles;
    }

    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name)
    {
        if(Enum.TryParse<VehicleTypes>(name, out var result))
        {
            return result;
        }
        throw new ArgumentException("Invalid Vehicle Type");
    }
        
}
