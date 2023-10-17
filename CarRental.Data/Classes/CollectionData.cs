using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System.Linq.Expressions;

namespace CarRental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    

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
        /*
        var vehicleToBook = _vehicles.SingleOrDefault(v => v.RegNo == "SIX666");
        var customerToBook = _persons.SingleOrDefault(p => p.FirstName == "John");

        if (vehicleToBook is not null && customerToBook is not null)
        {
            Booking booking = new Booking(
                vehicleToBook,
                customerToBook,
                vehicleToBook.Odometer,
                DateTime.Now,
                VehicleBookingStatuses.Open
            );

            vehicleToBook.VehicleStatus = Common.Enums.VehicleStatuses.Booked;

            _bookings.Add(booking);
        }
        
        */

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
        //    booking2.CloseBooking(3350, DateTime.Now);

        //    _bookings.Add(booking2);
        //}
        
    }

   
    
     
    public IBooking RentVehicle(int VehicleId, int customerId)
    {
        throw new NotImplementedException();
    }

    public IBooking ReturnVehicle(int VehicleId)
    {
        throw new NotImplementedException();
    }

    public void AddCustomer(IPerson customer)
    {
        _persons.Add(customer);
    }

    public void AddVehicle(IVehicle vehicle)
    {
        _vehicles.Add(vehicle);
    }

    public void AddBooking(Booking booking)
    {
        _bookings.Add(booking);
    }


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

    public List<T> Get<T>(Expression<Func<T, bool>>? expression)
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

    public T? Single<T>(Expression<Func<T, bool>>? expression)
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
