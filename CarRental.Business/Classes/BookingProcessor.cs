using CarRental.Common.Classes;
using CarRental.Common.Enums;
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

    public Vehicle Vehicle { get; } = new();
    public Customer Customer { get; } = new();
    public Booking Booking { get; } = new();

    public bool IsTaskDelayInProgress { get; private set; } = false;
    public string ErrorMessage { get; private set; } = "";


    public IEnumerable<T> GetItems<T>(Expression<Func<T, bool>>? expression = null)
    {
        return _data.Get(expression);
    }

    public void AddItem<T>(T item) where T : IId
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
            if (vehicleId != 0 && customerId != 0)
            {
                IsTaskDelayInProgress = true;
                
                var vehicleTask = Task.Run(() => GetVehicle(vehicleId));
                var customerTask = Task.Run(() => GetPerson(customerId));

                var vehicle = await vehicleTask;
                var customer = await customerTask;

                if (vehicle != null && customer != null)
                {
                    var initialOdometer = vehicle.Odometer;

                    var booking = new Booking(vehicle, customer, (int)initialOdometer, DateTime.Now, VehicleBookingStatuses.Open);
                    vehicle.VehicleStatus = VehicleStatuses.Booked;

                    await Task.Delay(5000);


                    AddItem(booking);
                    Customer.Id = 0;
                    ErrorMessage = "";

                    return booking;
                }
                else
                {
                    ErrorMessage = "Booking failed";
                }
            }
            else
            {
                ErrorMessage = "Please select a customer";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error while renting vehicle: {ex.Message}";
        }
        finally
        {
            IsTaskDelayInProgress = false;
        }
        return null;
    }

    public void ReturnVehicle(int vehicleId, int? distance)
    {
        try
        {
            var booking = GetBookingByVehicleId(vehicleId) ?? throw new Exception("Booking not found");
            var vehicle = GetVehicle(vehicleId) ?? throw new Exception("Vehicle not found");

            if (booking.VehicleBookingStatus == VehicleBookingStatuses.Open)
            {
                if (distance.HasValue && distance.Value >= 0)
                {
                    booking.VehicleBookingStatus = VehicleBookingStatuses.Closed;
                    vehicle.VehicleStatus = VehicleStatuses.Available;
                    vehicle.Odometer += distance.Value;
                    booking.ReturnDate = DateTime.Now;
                    booking.Distance = distance.Value;

                    double dayCost = vehicle.DayCost();
                    booking.CalculateTotalCost(dayCost, vehicle.CostKm);

                    Booking.Distance = null;
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Please enter a valid distance before returning the vehicle.";
                }
            }
            else
            {
                ErrorMessage = "Booking is not open and cannot be returned.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void AddVehicle()
    {
            if (!string.IsNullOrWhiteSpace(this.Vehicle.RegNo) 
                && !string.IsNullOrWhiteSpace(this.Vehicle.Make)
                && this.Vehicle.Odometer.HasValue && this.Vehicle.Odometer >= 0
                && this.Vehicle.CostKm.HasValue && this.Vehicle.CostKm >= 0
                && !string.IsNullOrEmpty(this.Vehicle.VehicleType.ToString())
                )
            {
                var vehicleTypeEnum = GetVehicleType(this.Vehicle.VehicleType.ToString());
                int nextVehicleId = _data.NextVehicleId;
                IVehicle vehicle = new Vehicle(default, this.Vehicle.RegNo, this.Vehicle.Make, (int)this.Vehicle.Odometer.Value, this.Vehicle.CostKm.Value, vehicleTypeEnum, VehicleStatuses.Available)
                {
                    Id = nextVehicleId,
                };
                AddItem(vehicle);
                ErrorMessage = "";

                this.Vehicle.RegNo = "";
                this.Vehicle.Make = "";
                this.Vehicle.Odometer = null;
                this.Vehicle.CostKm = null;
                this.Vehicle.VehicleType = default;
            }
            else
            {
                ErrorMessage = "Please fill in all required fields with valid values for adding a vehicle.";
            }
    }

    public void AddCustomer()
    {
        if (!string.IsNullOrWhiteSpace(this.Customer.SocialSecurityNumber) && 
            !string.IsNullOrWhiteSpace(this.Customer.FirstName) &&
            !string.IsNullOrWhiteSpace(this.Customer.LastName))
        {
            int nextPersonId = _data.NextPersonId;
            IPerson? customer = new Customer(default, this.Customer.SocialSecurityNumber, this.Customer.FirstName, this.Customer.LastName)
            {
                Id = nextPersonId
            };
            AddItem(customer);
            ErrorMessage = "";

            this.Customer.SocialSecurityNumber = "";
            this.Customer.FirstName = "";
            this.Customer.LastName = "";
        }
        else
        {
            ErrorMessage = "Please fill in all required fields for adding a customer.";
        }
    }

    //public string[] VehicleBookingStatusNames => _data.VehicleBookingStatusNames;
    //public string[] VehicleStatusNames => _data.VehicleStatusNames;
    public string[] VehicleTypeNames => _data.VehicleTypeNames; 
    public VehicleTypes GetVehicleType(string name) => _data.GetVehicleType(name);
    
}
