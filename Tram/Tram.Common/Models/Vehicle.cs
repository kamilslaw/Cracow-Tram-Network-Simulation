using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using Tram.Common.Interfaces;

namespace Tram.Common.Models
{
    public class Vehicle : ModelBase, IObjWithCoordinates
    {
        public TramLine Line { get; set; }
        
        public int Passengers { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime LastDepartureTime { get; set; }

        public TramLine.Departure Departure { get; set; }

        public bool IsOnStop { get; set; }

        public Location Position { get; set; }

        public float Speed { get; set; }

        public float? MaxSpeed { get; set; }

        public Node LastVisitedStop { get; set; }

        public List<Node> LastVisitedStops { get; set; }

        public TramIntersection CurrentIntersection { get; set; }

        public TramIntersection LastIntersection { get; set; }

        public List<Node> VisitedNodes { get; set; }

        public List<int> PassengersHistory { get; set; }

        public List<double> DelaysHistory { get; set; }

        Vector2 IObjWithCoordinates.Coordinates
        {
            get { return Position.Coordinates; }
        }

        public class Location
        {
            public Node Node1 { get; set; }

            public Node Node2 { get; set; }

            // 0 - 100%
            public float Displacement { get; set; }

            public Vector2 Coordinates { get; set; }
        }
    }
}
