using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
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
        _persons.Add(new Customer(960321, "Jan", "Deg"));
        _persons.Add(new Customer(690101, "John", "Doe"));
        _persons.Add(new Customer(721111, "Jane", "Smith"));

        //Vehicles
        _vehicles.Add(new Car("SIX666", "Volvo", 1000, 2.5, VehicleTypes.Combi, VehicleStatuses.Available));
        _vehicles.Add(new Motorcycle("ABC123", "Yamaha", 3000, 1, VehicleTypes.Motorcycle, VehicleStatuses.Available));
        _vehicles.Add(new Car("BKJ142", "Ford", 2500, 2, VehicleTypes.Van, VehicleStatuses.Available));
        _vehicles.Add(new Car("BLZ420", "BMW", 5000, 5, VehicleTypes.Sedan, VehicleStatuses.Available));
        _vehicles.Add(new Car("ORT141", "Saab", 10000, 1.5, VehicleTypes.Combi, VehicleStatuses.Available));

        //Bookings


        //var carToBook = _vehicles.SingleOrDefault(c => c.RegNo == "BKJ142");
        //var customerJan = _persons.SingleOrDefault(p => p.CustomerId == 960321);
        //if (carToBook is not null && customerJan is not null)
        //{
        //    carToBook.Odometer = 2500;
        //    DateTime bookingDate1 = DateTime.Now;

        //    Booking booking1 = new Booking(
        //        carToBook,
        //        customerJan,
        //        2500,
        //        bookingDate1,
        //        VehicleBookingStatuses.Open
        //        );
            
        //    booking1.RentVehicle(carToBook, customerJan, 2500, bookingDate1);
        //    _bookings.Add(booking1);
        //}

        //var motorcycleToBook = _vehicles.SingleOrDefault(v => v.RegNo == "ABC123");
        //var customerJane = _persons.SingleOrDefault(p => p.CustomerId == 721111);
        //if (motorcycleToBook is not null && customerJane is not null)
        //{
        //    motorcycleToBook.Odometer = 3350;

        //    DateTime bookingDate = DateTime.Now.AddDays(-1);

        //    Booking booking2 = new Booking(
        //        motorcycleToBook,
        //        customerJane,
        //        3000,
        //        bookingDate,
        //        VehicleBookingStatuses.Closed
        //    );
        //    booking2.ReturnVehicle(3350, DateTime.Now);

        //    _bookings.Add(booking2);
        //}

    }

   
     
    public IBooking RentVehicle(int VehicleId, int customerId)
    {
        IVehicle vehicle = _vehicles.SingleOrDefault(v => v.Id == VehicleId);
        IPerson customer = _persons.SingleOrDefault(p => p.Id == customerId);

        if (vehicle != null && customer != null)
        {
            if(vehicle.VehicleStatus == VehicleStatuses.Available)
            {
                IBooking booking = new Booking(vehicle, customer, (int)vehicle.Odometer, DateTime.Now, VehicleBookingStatuses.Open);

                vehicle.VehicleStatus = VehicleStatuses.Booked;

                _bookings.Add(booking);
            }
            else
            {
                throw new Exception("Vehicle not available for rent");
            }
        }
        return null;
    } // Denna

    public IBooking ReturnVehicle(int VehicleId)
    {
        IVehicle vehicle = _vehicles.SingleOrDefault(v => v.Id == VehicleId);

        if (vehicle != null)
        {
            if (vehicle.VehicleStatus == VehicleStatuses.Booked)
            {
                IBooking booking = _bookings.SingleOrDefault(b => b.Vehicle ==  vehicle && b.VehicleBookingStatus == VehicleBookingStatuses.Open);

                if (booking != null)
                {
                    booking.RentVehicle(vehicle, booking.Person, (int)vehicle.Odometer, DateTime.Now);

                    vehicle.VehicleStatus = VehicleStatuses.Available;

                    vehicle.Odometer = booking.Distance;

                    return booking;
                }
                else throw new Exception("Booking not found");
            }
            else {
                throw new Exception();
            }
            
        }
        return null;
    } // Och denna är viktiga att fortsätta med!


    public void AddCustomer(IPerson customer)
    {
        _persons.Add(customer);
    }

    public void AddVehicle(IVehicle vehicle)
    {
        _vehicles.Add(vehicle);
    }

    public void AddBooking(IBooking booking)
    {
        _bookings.Add(booking);
    } //hade klassen Booking innan!


    public void Add<T>(T item)
    {
        if (item is IVehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }
        if (item is IPerson person)
        {
            _persons.Add(person);
        }
        if (item is IBooking booking)
        {
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
