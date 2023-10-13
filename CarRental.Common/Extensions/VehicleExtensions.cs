﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Extensions;

//public static int Duration(this DateTime startDate, DateTime endDate) { }
//vi fixar denna sen när det blir relevant, denna ska beräkna datumen mellan bokningen och return.

public static class VehicleExtensions
{
    public static double DayCost(this IVehicle vehicle)
    {
        if (vehicle == null) 
            throw new ArgumentException(nameof(vehicle));
        
        switch (vehicle.VehicleType)
        { 
            case Enums.VehicleTypes.Sedan:
            return 100;
        case Enums.VehicleTypes.Combi:
            return 120;
        case Enums.VehicleTypes.Van:
            return 150;
        case Enums.VehicleTypes.Motorcycle:
            return 50;
        default:
            throw new ArgumentException(nameof(vehicle.VehicleType));
        }
    }
}
