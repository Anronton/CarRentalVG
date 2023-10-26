using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Classes;

public class Vehicle : IVehicle
{
    public int Id { get; set; }
    public string RegNo { get; set; }
    public string Make { get; set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    public VehicleTypes VehicleType { get; set; }
    public VehicleStatuses VehicleStatus { get; set; }

    public Vehicle(int id, string regNo, string make, int? odometer, double? costKm, VehicleTypes vehicleType, VehicleStatuses vehicleStatus)
    {  
        Id = id;
        RegNo = regNo;
        Make = make;
        Odometer = odometer;
        CostKm = costKm;
        VehicleType = vehicleType;
        VehicleStatus = vehicleStatus;
    }
    public Vehicle()
    {
        
    }
}
