﻿using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Car : Vehicle
{


    public Car(int id, string regNo, string make, int odometer, double costKm, VehicleTypes vehicleType, VehicleStatuses vehicleStatus) :
       base (id, regNo, make, odometer, costKm, vehicleType, vehicleStatus) {}
}
