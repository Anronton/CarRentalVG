using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{


    public Motorcycle(string regNo, string make, int odometer, double costKm, VehicleTypes vehicleType, VehicleStatuses vehicleStatus) :
        base(regNo, make, odometer, costKm, vehicleType, vehicleStatus)
    { }


    public override double DayCost( VehicleTypes vehicleType)
    {
        switch (vehicleType)
        {
            case VehicleTypes.Sedan:
                return 100;
            case VehicleTypes.Combi:
                return 120;
            case VehicleTypes.Van:
                return 150;
            case VehicleTypes.Motorcycle:
                return 50;
            default:
                throw new ArgumentException(nameof(vehicleType));
        }
    }
}
