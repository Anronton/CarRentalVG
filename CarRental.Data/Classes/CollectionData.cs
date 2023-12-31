﻿using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Common.Extensions;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;

namespace CarRental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new();
    readonly List<IVehicle> _vehicles = new();
    readonly List<IBooking> _bookings = new();

   
    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(v => v.Id) + 1;
    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(p => p.Id) + 1;
    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(b => b.Id) + 1;

    public CollectionData() => SeedData();

    void SeedData()
    {
        //Customers
        _persons.Add(new Customer(1, "960321", "Jan", "Deg"));
        _persons.Add(new Customer(2, "690101", "JSON", "Derulo"));
        _persons.Add(new Customer(3, "721111", "Jane", "Smith"));

        //Vehicles
        _vehicles.Add(new Vehicle(1, "SIX666", "Volvo", 1000, 2.5, VehicleTypes.Combi, VehicleStatuses.Available));
        _vehicles.Add(new Vehicle(2, "ABC123", "Yamaha", 3000, 1, VehicleTypes.Motorcycle, VehicleStatuses.Available));
        _vehicles.Add(new Vehicle(3, "BKJ142", "Ford", 2500, 2, VehicleTypes.Van, VehicleStatuses.Available));
        _vehicles.Add(new Vehicle(4, "BLZ420", "BMW", 5000, 5, VehicleTypes.Sedan, VehicleStatuses.Available));
        _vehicles.Add(new Vehicle(5, "ORT141", "Saab", 10000, 1.5, VehicleTypes.Combi, VehicleStatuses.Available));

        //bookings

        IBooking? booking = RentVehicle(2, 3);

        if (booking != null)
        {
            booking.Distance = 350;
            ReturnVehicle(2);
        }
    }

    public IBooking? RentVehicle(int vehicleId, int customerId)
    {
        IVehicle? vehicle = _vehicles.SingleOrDefault(v => v.Id == vehicleId);
        IPerson? customer = _persons.SingleOrDefault(p => p.Id == customerId);

        if (vehicle != null && customer != null)
        {
            if(vehicle.VehicleStatus == VehicleStatuses.Available)
            {
                int initialOdometer = (int)vehicle.Odometer;

                IBooking booking = new Booking(vehicle, customer, initialOdometer, DateTime.Now, VehicleBookingStatuses.Open);

                vehicle.VehicleStatus = VehicleStatuses.Booked;

                booking.Id = NextBookingId;
                _bookings.Add(booking);
                return booking;
            }
            else
            {
                throw new Exception("Vehicle not available for Renting/booking");
            }
        }
        return null;
    }

    public IBooking? ReturnVehicle(int vehicleId)
    {
        IVehicle? vehicle = _vehicles.SingleOrDefault(v => v.Id == vehicleId);

        if (vehicle != null)
        {
            if (vehicle.VehicleStatus == VehicleStatuses.Booked)
            {
                IBooking? booking = _bookings.SingleOrDefault(b => b.Vehicle == vehicle && b.VehicleBookingStatus == VehicleBookingStatuses.Open);

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
                else throw new Exception("Booking not found.");
            }
            else {
                throw new Exception("Vehicle is not booked and cannot be returned.");
            }
        }
        return null;
    }

    public void Add<T>(T item) where T : IId
    {
        if (item != null)
        {

            item.Id = GetNextId<T>();
        
            if (item is IVehicle)
            {
                _vehicles.Add((IVehicle)item);
            }
            if (item is IPerson)
            {
                _persons.Add((IPerson)item);
            }
            else if (item is IBooking)
            {
                _bookings.Add((IBooking)item);
            }
        }
    }

    private int GetNextId<T>() where T : IId
    {
        int maxId = 0;

        if (typeof(T) == typeof(IVehicle))
        {
            maxId = _vehicles.Count > 0 ? _vehicles.Max(v => v.Id) : 0;
        }
        else if (typeof(T) == typeof(IPerson))
        {
            maxId = _persons.Count > 0 ? _persons.Max(p => p.Id) : 0;
        }
        else if (typeof(T) == typeof(IBooking))
        {
            maxId = _bookings.Count > 0 ? _bookings.Max(b => b.Id) : 0;
        }

        return maxId + 1;
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if(status != default)
        {
            return _vehicles.Where(v => v.VehicleStatus == status);
        }
        return _vehicles;
    }

    public string[] VehicleBookingStatusNames => Enum.GetNames(typeof(VehicleBookingStatuses));
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

    public List<T> Get<T>(Expression<Func<T, bool>>? expression)
    {
        FieldInfo? info = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(f => f.FieldType == typeof(List<T>));
        if (info != null)
        {
            var list = (List<T>)info.GetValue(this);
            if(expression != null)
            {
                list = list.Where(expression.Compile()).ToList();
            }
            return list;
        }
        else
        {
            throw new InvalidOperationException("No private List<T> field found in the current instance.");
        }
    }

    public T? Single<T>(Expression<Func<T, bool>>? expression)
    {
        FieldInfo? info = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(f => f.FieldType == typeof(List<T>));
        if(info != null)
        {
            var list = (List<T>)info.GetValue(this);
            var item = list.SingleOrDefault(expression.Compile());

            return item;
        }
        else 
        { 
            throw new InvalidOperationException("No private List<T> field found in the current instance."); 
        }
    }
}
