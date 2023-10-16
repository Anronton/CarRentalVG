using CarRental.Common.Classes;
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

    //Här kommer vi att behöva lägga till några fler metoder, dessa kan vara kvar och vi kan lägga till tex varje single också
    //Men här ska vi anropa de tre generiska metoderna i CollectionData med LinQ-uttrycken.
    //Så här är det fritt fram att ha flera metoder. Så här visar vi hur vi jobbar med de genriska metoderna i datalagret.
    //Sen så är det valfritt att även göra dessa generiska.
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
    //public IPerson? GetPerson(string customerId){}
    public IEnumerable<IVehicle> GetVehicles(Expression<Func<IVehicle, bool>>? expression = null)
    {
        return GetItems(expression);
    } 


    //public IVehicle? GetVehicle(int vehicleId){}
    //public IVehicle? GetVehicle(string regNo){}
    public IEnumerable<IBooking> GetBookings(Expression<Func<IBooking, bool>>? expression = null)
    {
        return GetItems(expression);
    }
    //public IBooking GetBooking(int, vehicleId){}

    //public async Task<IBooking> RentVehicle(int vehicleIdm int customerId){}
    //public IBooking ReturnVehicle(int vehicleId, double distance){}

    //public void AddVehicle(string make, string regNo, double odomer osv.osv..)
    public void AddCustomer(int customerId, string firstName, string lastName)
    {
        IPerson customer = new Customer(customerId, firstName, lastName);
        AddItem(customer);
    }


}
