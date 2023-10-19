﻿using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Classes;

public abstract class Vehicle : IVehicle
{
    public int Id { get; set; }
    public string RegNo { get; init; }
    public string Make { get; init; }
    public double Odometer { get; set; }
    public double CostKm { get; init; }
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatuses VehicleStatus { get; set; }

    public Vehicle(string regNo, string make, double odometer, double costKm, VehicleTypes vehicleType, VehicleStatuses vehicleStatus)
    {
        Id = 0;
        RegNo = regNo;
        Make = make;
        Odometer = odometer;
        CostKm = costKm;
        VehicleType = vehicleType;
        VehicleStatus = vehicleStatus;
    }
}
