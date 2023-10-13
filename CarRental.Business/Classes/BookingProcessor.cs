using CarRental.Common.Classes;
using CarRental.Common.Interfaces;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;

    public BookingProcessor(IData data)
    {
        _data = data;
    }

    //Här kommer vi att behöva lägga till några fler metoder, dessa kan vara kvar och vi kan lägga till tex varje single också
    //Men här ska vi anropa de tre generiska metoderna i CollectionData med LinQ-uttrycken.
    //Så här är det fritt fram att ha flera metoder. Så här visar vi hur vi jobbar med de genriska metoderna i datalagret.
    //Sen så är det valfritt att även göra dessa generiska.
    public IEnumerable<IPerson> GetPersons() => _data.GetPersons();
    //public IPerson? GetPerson(string customerId){}
    public IEnumerable<IVehicle> GetVehicles() => _data.GetVehicles();
    //public IVehicle? GetVehicle(int vehicleId){}
    //public IVehicle? GetVehicle(string regNo){}
    public IEnumerable<IBooking> GetBookings() => _data.GetBookings();
    //public IBooking GetBooking(int, vehicleId){}

    //public async Task<IBooking> RentVehicle(int vehicleIdm int customerId){}
    //public IBooking ReturnVehicle(int vehicleId, double distance){}

    //public void AddVehicle(string make, string regNo, double odomer osv.osv..)
    public void AddCustomer(int customerId, string firstName, string lastName)
    {
        var customer = new Customer(customerId, firstName, lastName);

        _data.AddCustomer(customer);
    }
}
