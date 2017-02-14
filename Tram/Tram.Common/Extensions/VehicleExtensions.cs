using System;
using System.Collections.Generic;
using System.Linq;
using Tram.Common.Consts;
using Tram.Common.Enums;
using Tram.Common.Helpers;
using Tram.Common.Interfaces;
using Tram.Common.Models;

namespace Tram.Common.Extensions
{
    public static class VehicleExtensions
    {
        public static float RealDistanceTo(this Vehicle vehicle, IObjWithCoordinates point) => GeometryHelper.GetRealDistance(vehicle.Position.Coordinates, point.Coordinates);

        public static bool IsBusStopReached(this Vehicle vehicle)
        {
            var node1 = vehicle.Position.Node1;
            var node2 = vehicle.Position.Node2;
            return (node1 != null && node1.Type == NodeType.TramStop && node1 != vehicle.LastVisitedStop && vehicle.RealDistanceTo(node1) <= CalculationConsts.DISTANCE_EPSILON && vehicle.Line.MainNodes.Any(mn => mn.Equals(node1))) ||
                   (node2 != null && node2.Type == NodeType.TramStop && node2 != vehicle.LastVisitedStop && vehicle.RealDistanceTo(node2) <= CalculationConsts.DISTANCE_EPSILON && vehicle.Line.MainNodes.Any(mn => mn.Equals(node2)));
        }

        public static bool IsIntersectionReached(this Vehicle vehicle, out TramIntersection intersection)
        {
            var node1 = vehicle.Position.Node1;
            var node2 = vehicle.Position.Node2;
            if (node1 != null && node1.Type == NodeType.TramCross && !node1.Intersection.Equals(vehicle.LastIntersection))
            {
                intersection = node1.Intersection;
                return true;
            }
            else if (node2 != null && node2.Type == NodeType.TramCross && !node2.Intersection.Equals(vehicle.LastIntersection))
            {
                intersection = node2.Intersection;
                return true;
            }

            intersection = null;
            return false;
        }

        public static bool IsOnLights(this Vehicle vehicle) => vehicle.Position.Node2.Type == NodeType.CarCross;

        public static bool IsOnLightsAndHasRedLight(this Vehicle vehicle, float deltaTime)
        {
            if (vehicle.Position.Node2.Type == NodeType.CarCross && vehicle.Position.Node2.LightState != LightState.Green)
            {
                float speed = PhysicsHelper.GetNewSpeed(vehicle.Speed, deltaTime);
                float brakingDistance = PhysicsHelper.GetBrakingDistance(speed);
                float distance = vehicle.RealDistanceTo(vehicle.Position.Node2) - 1;
                if (vehicle.Position.Node2.LightState == LightState.Red)
                {
                    return distance <= brakingDistance;
                }
                else if (vehicle.Position.Node2.LightState == LightState.Yellow)
                {
                    return distance >= brakingDistance;
                }
            }

            return false;
        }

        public static bool IsStillOnIntersection(this Vehicle vehicle)
        {
            return vehicle.CurrentIntersection.Nodes.Any(n => (vehicle.RealDistanceTo(n) - VehicleConsts.LENGTH) < 0) ||
                   vehicle.CurrentIntersection.Equals(vehicle.Position.Node2?.Intersection);
        }

        public static void NormalizeSpeed(this Vehicle vehicle)
        {
            if (!vehicle.IsOnStop)
            {
                if (vehicle.Speed < 0)
                {
                    vehicle.Speed = 0;
                }
                else if (vehicle.Speed > (vehicle.MaxSpeed ?? VehicleConsts.MAX_SPEED))
                {
                    vehicle.Speed = vehicle.MaxSpeed ?? VehicleConsts.MAX_SPEED;
                }
            }
        }

        public static bool IsStraightRoad(this Vehicle vehicle, float deltaTime)
        {
            var node1 = vehicle.Position.Node1;
            if (node1 != null && CorrectStopPredicate(vehicle, node1))
            {
                return false;
            }

            var node = vehicle.Position.Node2;
            if (node == null)
            {
                return true;
            }

            float speed = PhysicsHelper.GetNewSpeed(vehicle.Speed, deltaTime);
            float distance = vehicle.RealDistanceTo(node) - 1;
            float brakingDistance = PhysicsHelper.GetBrakingDistance(speed);
            
            if (IsNotStraightRoadPredicate(vehicle, node, distance, brakingDistance))
            {
                return false;
            }

            while (distance <= brakingDistance)
            {
                var newNode = vehicle.Line.GetNextNode(node);
                if (newNode == null)
                {
                    return true;
                }

                distance += newNode.Distance;
                node = newNode.Node;
                if (IsNotStraightRoadPredicate(vehicle, node, distance, brakingDistance))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsNotStraightRoadPredicate(Vehicle vehicle, Node node, float distance, float brakingDistance) => 
                            distance <= brakingDistance && 
                            node.Type != NodeType.Normal && 
                            (CorrectTramCrossPredicate(vehicle, node) || CorrectStopPredicate(vehicle, node) || CorrectNotGreenCarCrossPredicate(node));

        public static bool IsAnyVehicleClose(this Vehicle vehicle, float deltaTime)
        {
            float speed = PhysicsHelper.GetNewSpeed(vehicle.Speed, deltaTime);
            float brakingDistance = PhysicsHelper.GetBrakingDistance(speed);

            return IsAnyVehicleClose(vehicle, speed, brakingDistance, vehicle.Position.Node1, 0);
        }

        private static bool IsAnyVehicleClose(Vehicle vehicle, float speed, float brakingDistance, Node node, float distance)
        {
            if (distance > brakingDistance + VehicleConsts.SAFE_SPACE)
            {
                return false;
            }

            bool isFirstComparation = !(distance > 0);

            foreach (Vehicle neighbor in node.VehiclesOn)
            {
                if (!neighbor.Equals(vehicle) &&
                     vehicle.RealDistanceTo(neighbor) <= (brakingDistance + VehicleConsts.SAFE_SPACE) && // check distance between vehicles
                     (!isFirstComparation ||
                     neighbor.RealDistanceTo(neighbor.Position.Node2) <= vehicle.RealDistanceTo(neighbor.Position.Node2) &&
                     vehicle.RealDistanceTo(vehicle.Position.Node1) <= neighbor.RealDistanceTo(vehicle.Position.Node1))) // check if object is ahead of vehicle)
                {
                    return true;
                }
            }

            return node.GetAllNextNodes()
                       .Any(nn => IsAnyVehicleClose(vehicle, speed, brakingDistance, nn.Node, isFirstComparation ? vehicle.RealDistanceTo(nn.Node) : distance + nn.Distance));
        }

        public static bool CanArleadyStart(this Vehicle vehicle, DateTime actualRealTime)
        {
            int interval = vehicle.Departure.NextStopIntervals.Count > vehicle.LastVisitedStops.Count ? (int)vehicle.Departure.NextStopIntervals[vehicle.LastVisitedStops.Count] : 0;
            return TimeHelper.CompareTimes(vehicle.LastDepartureTime + new TimeSpan(0, interval, 0), actualRealTime) <= 0;
        }

        private static bool CorrectStopPredicate(Vehicle vehicle, Node node)
        {
            return node.Type == NodeType.TramStop && 
                   !vehicle.LastVisitedStop.Equals(node) &&
                   !vehicle.LastVisitedStops.Any(n => n.Equals(node)) &&
                   vehicle.Line.MainNodes.Any(n => n.Equals(node));
        }

        private static bool CorrectTramCrossPredicate(Vehicle vehicle, Node node)
        {
            return node.Type == NodeType.TramCross && 
                   (vehicle.CurrentIntersection == null || !vehicle.CurrentIntersection.Equals(node.Intersection));
        }

        private static bool CorrectNotGreenCarCrossPredicate(Node node)
        {
            return node.Type == NodeType.CarCross && node.LightState != LightState.Green;
        }
    }
}
