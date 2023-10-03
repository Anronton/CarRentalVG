using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    public CollectionData() => SeedData();

    void SeedData()
    {
        //Customers
        _persons.Add(new Customer(960321, "Jan", "Deg"));
        _persons.Add(new Customer(690101, "John", "Doe"));
        _persons.Add(new Customer(721111, "Jane", "Smith"));

        //Vehicles
        _vehicles.Add(new Car("SIX666", "Volvo", 1000, 2.5, Common.Enums.VehicleTypes.Combi, Common.Enums.VehicleStatuses.Available));
        _vehicles.Add(new Motorcycle("ABC123", "Yamaha", 3000, 1, Common.Enums.VehicleTypes.Motorcycle, Common.Enums.VehicleStatuses.Available));
        _vehicles.Add(new Car("BKJ142", "Ford", 2500, 2, Common.Enums.VehicleTypes.Van, Common.Enums.VehicleStatuses.Available));
        _vehicles.Add(new Car("BLZ420", "BMW", 5000, 5, Common.Enums.VehicleTypes.Sedan, Common.Enums.VehicleStatuses.Available));
        _vehicles.Add(new Car("ORT141", "Saab", 10000, 1.5, Common.Enums.VehicleTypes.Combi, Common.Enums.VehicleStatuses.Available));

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
        


        var motorcycleToBook = _vehicles.SingleOrDefault(v => v.RegNo == "ABC123");
        var customerJane = _persons.SingleOrDefault(p => p.CustomerId == 721111);
        if (motorcycleToBook is not null && customerJane is not null)
        {
            motorcycleToBook.Odometer = 3350;

            DateTime bookingDate = DateTime.Now.AddDays(-1);

            Booking booking2 = new Booking(
                motorcycleToBook,
                customerJane,
                3000,
                bookingDate,
                VehicleBookingStatuses.Closed
            );
            booking2.CloseBooking(3350, DateTime.Now);

            _bookings.Add(booking2);
        }
        */
    }
    
    public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IBooking> GetBookings() => _bookings;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _vehicles;
}
