using System;
using System.Drawing;
using Tram.Common.Consts;
using Tram.Common.Helpers;
using Tram.Common.Models;

namespace Tram.Controller.Controllers
{
    public class CapacityController
    {
        private MainController mainController;

        // sets the new capacity of vehicle, based on actual time, line and current stop; returns the time of boarding (in seconds)
        public double SetTramCapacity(Vehicle vehicle)
        {
            if (mainController == null)
            {
                mainController = Kernel.Get<MainController>();
            }
            
            int passengersIn = 0, passengersOut = 0;
            if (vehicle.LastVisitedStops.Count < vehicle.Line.Capacity[TimeHelper.GetTimeStr(vehicle.StartTime)].CurrentState.Count)
            {
                vehicle.Passengers = vehicle.Line.Capacity[TimeHelper.GetTimeStr(vehicle.StartTime)].CurrentState[vehicle.LastVisitedStops.Count];
                vehicle.PassengersHistory.Add(vehicle.Passengers);

                passengersIn = vehicle.Line.Capacity[TimeHelper.GetTimeStr(vehicle.StartTime)].GotIn[vehicle.LastVisitedStops.Count];
                passengersOut = vehicle.Line.Capacity[TimeHelper.GetTimeStr(vehicle.StartTime)].GotOut[vehicle.LastVisitedStops.Count];
            }


            int passengersChange = passengersIn + passengersOut;
            //10 + 1/-e^x
            return 10 + 1 / -Math.Pow(Math.E, (passengersChange + 2) / 2 - 3);
        }

        public Color GetTramColor(int capacity)
        {
            int red = Math.Min(capacity, VehicleConsts.MAX_CAPACITY) * 255 / VehicleConsts.MAX_CAPACITY;
            int green = 255 - red;
            int blue = 0; // 255 - Math.Max(red, green);

            return Color.FromArgb(red, green, blue);
        }
    }
}
