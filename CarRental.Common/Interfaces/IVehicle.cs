using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IVehicle
{
    public string RegNo { get; init; }
    public string Make { get; init; }
    public int Odometer { get; set; }
    public double CostKm { get; init; }
    public VehicleTypes VehicleType { get; init; }
    public double DayCost(VehicleTypes vehicleType);
    public VehicleStatuses VehicleStatus { get; set; }
}
