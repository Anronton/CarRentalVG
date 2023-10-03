using CarRental.Common.Interfaces;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;

    public BookingProcessor(IData data)
    {
        _data = data;
    }

    public IEnumerable<IPerson> GetPersons() => _data.GetPersons();
    public IEnumerable<IVehicle> GetVehicles() => _data.GetVehicles();
    public IEnumerable<IBooking> GetBookings() => _data.GetBookings();
    
}
