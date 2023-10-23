using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Extensions;

public static class BookingExtensions
{
    public static void CalculateTotalCost(this IBooking booking, double dayCost, double costKm)
    {
        if (booking.ReturnDate.HasValue)
        { 
            double days = (booking.ReturnDate.Value - booking.BookingDate).TotalDays;
            if (days < 1)
            {
                days = 1;
            }

            int distance = booking.Distance ?? 0;

            double firstDayCost = Math.Max(1, dayCost);

            booking.TotalCost = (firstDayCost + (days - 1) * dayCost) + distance * costKm;
        }
        else
        {
            booking.TotalCost = null;
        }
    }
}
