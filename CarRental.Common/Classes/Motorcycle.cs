using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{


    public Motorcycle(string regNo, string make, int odometer, double costKm, VehicleTypes vehicleType, VehicleStatuses vehicleStatus) :
        base(regNo, make, odometer, costKm, vehicleType, vehicleStatus)
    { }
}
