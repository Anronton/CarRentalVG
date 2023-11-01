using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IVehicle : IId
{
    
    string RegNo { get; set; }
    string Make { get; set; }
    int? Odometer { get; set; }
    double? CostKm { get; set; }
    VehicleTypes VehicleType { get; set; }
    VehicleStatuses VehicleStatus { get; set; }
}
