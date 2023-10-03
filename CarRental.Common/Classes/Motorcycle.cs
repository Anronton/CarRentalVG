using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Motorcycle : IVehicle
{
    public string RegNo { get; init; }
    public string Make { get; init; }
    public int Odometer { get; set; }
    public double CostKm { get; init; }
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatuses VehicleStatus { get; set; }

    public Motorcycle(string regNo, string make, int odometer, double costKm, VehicleTypes vehicleType, VehicleStatuses vehicleStatus) =>
        (RegNo, Make, Odometer, CostKm, VehicleType, VehicleStatus) = (regNo, make, odometer, costKm, vehicleType, vehicleStatus);


    public double DayCost(VehicleTypes vehicleType)
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
