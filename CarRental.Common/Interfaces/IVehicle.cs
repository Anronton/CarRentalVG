using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IVehicle : IId
{
    
    public string RegNo { get; set; }
    public string Make { get; set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    public VehicleTypes VehicleType { get; set; }
    public VehicleStatuses VehicleStatus { get; set; }
}
