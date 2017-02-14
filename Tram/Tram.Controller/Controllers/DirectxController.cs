using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tram.Common.Consts;
using Tram.Common.Enums;
using Tram.Common.Extensions;
using Tram.Common.Helpers;
using Tram.Common.Models;

namespace Tram.Controller.Controllers
{
    public class DirectxController
    {
        private bool isDeviceInit;

        private MainController mainController;
        private CapacityController capacityController;
        private List<CustomVertex.PositionColored[]> vertexes;
        private List<CustomVertex.PositionColored[]> edges;

        private Microsoft.DirectX.Direct3D.Font text;
        private Line line;
        private Vector2[] lineVertexes;
        private float minX, maxX, minY, maxY;

        public DirectxController()
        {
            isDeviceInit = false;
            vertexes = new List<CustomVertex.PositionColored[]>();
            edges = new List<CustomVertex.PositionColored[]>();
        }

        #region Public Methods

        public void InitMap()
        {
            if (mainController == null)
            {
                mainController = Kernel.Get<MainController>();
            }
            if (capacityController == null)
            {
                capacityController = Kernel.Get<CapacityController>();
            }

            minX = mainController.Map.Min(n => n.Coordinates.X);
            maxX = mainController.Map.Max(n => n.Coordinates.X);
            minY = mainController.Map.Min(n => n.Coordinates.Y);
            maxY = mainController.Map.Max(n => n.Coordinates.Y);

            foreach (var node in mainController.Map.OrderBy(n => !n.IsUnderground))
            {
                float pX = CalculateXPosition(node.Coordinates.X); 
                float pY = CalculateYPosition(node.Coordinates.Y);
                if (node.Type != NodeType.CarCross)
                {
                    vertexes.Add(
                        DirectxHelper.CreateCircle(
                            pX,
                            pY,
                            node.Type == NodeType.CarCross ? ViewConsts.GREEN_LIGHT_COLOR.ToArgb() :
                                node.Type == NodeType.TramStop ? ViewConsts.STOP_COLOR.ToArgb() : ViewConsts.POINT_NORMAL_COLOR.ToArgb(),
                            ViewConsts.POINT_RADIUS,
                            ViewConsts.POINT_PRECISION));
                }

                if (node.Child != null)
                {
                    float pX2 = CalculateXPosition(node.Child.Node.Coordinates.X);
                    float pY2 = CalculateYPosition(node.Child.Node.Coordinates.Y);
                    edges.Add(DirectxHelper.CreateLine(pX, pY, pX2, pY2, GetLineColor(node, node.Child.Node).ToArgb(), ViewConsts.POINT_RADIUS));
                }
                else if (node.Children != null)
                {
                    foreach (var child in node.Children)
                    {
                        float pX2 = CalculateXPosition(child.Node.Coordinates.X);
                        float pY2 = CalculateYPosition(child.Node.Coordinates.Y);
                        edges.Add(DirectxHelper.CreateLine(pX, pY, pX2, pY2, GetLineColor(node, child.Node).ToArgb(), ViewConsts.POINT_RADIUS));
                    }
                }
            }
        }

        public void Render(Device device, Vector3 cameraPosition, Vehicle selectedVehicle, string time)
        {
            if (!isDeviceInit)
            {
                InitDevice(device);
            }

            //DRAW EDGES
            foreach (var edge in edges)
            {
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, edge);
            }

            //DRAW POINTS
            foreach (var vertex in vertexes)
            {
                device.DrawUserPrimitives(PrimitiveType.TriangleFan, ViewConsts.POINT_PRECISION, vertex);
            }

            DrawVehicles(device, cameraPosition, selectedVehicle);

            DrawCarInstersections(device);

            //DRAW TIME
            text.DrawText(null, time, new Point(12, 11), Color.Black);
            line.Draw(lineVertexes, Color.Black);
        }

        public float CalculateXPosition(float originalX)
        {
            return (100 - (originalX - minX) * 100 / (maxX - minX)) - 50; // X axis is swapped
        }

        public float CalculateYPosition(float originalY)
        {
            return (originalY - minY) * 100 / (maxX - minX) - (50 * (minY - maxY)) / (minX - maxX);
        }

        #endregion Public Methods

        #region Private Methods
        
        private void InitDevice(Device device)
        {
            System.Drawing.Font systemfont = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Regular);
            text = new Microsoft.DirectX.Direct3D.Font(device, systemfont);
            line = new Line(device);
            lineVertexes = new Vector2[] { new Vector2(8, 8), new Vector2(77, 8), new Vector2(77, 31), new Vector2(8, 31), new Vector2(8, 8) };
            isDeviceInit = true;
        }

        private void DrawVehicles(Device device, Vector3 cameraPosition, Vehicle selectedVehicle)
        {
            foreach (var vehicle in mainController.Vehicles)
            {
                Color tramColor = capacityController.GetTramColor(vehicle.Passengers);
                float pX = CalculateXPosition(vehicle.Position.Coordinates.X);
                float pY = CalculateYPosition(vehicle.Position.Coordinates.Y);
                float thickness = GetPointRadius(cameraPosition.Z);
                float selectedThickness = thickness * 1.7f;

                if (vehicle.Equals(selectedVehicle))
                {
                    device.DrawUserPrimitives(PrimitiveType.TriangleFan, ViewConsts.POINT_PRECISION, DirectxHelper.CreateCircle(pX, pY, ViewConsts.SELECTED_COLOR.ToArgb(), selectedThickness, ViewConsts.POINT_PRECISION));
                }

                device.DrawUserPrimitives(PrimitiveType.TriangleFan, ViewConsts.POINT_PRECISION, DirectxHelper.CreateCircle(pX, pY, tramColor.ToArgb(), thickness, ViewConsts.POINT_PRECISION));

                float length = VehicleConsts.LENGTH;
                int actualNodeIndex = vehicle.VisitedNodes.Count - 2;
                Vector2 prevCoordinates = vehicle.Position.Coordinates;
                float pX2, pY2;
                while (length > 0)
                {
                    if (actualNodeIndex >= 0)
                    {
                        Node actualNode = vehicle.VisitedNodes[actualNodeIndex--];
                        float distance = prevCoordinates.RealDistanceTo(actualNode);
                        pX = CalculateXPosition(prevCoordinates.X);
                        pY = CalculateYPosition(prevCoordinates.Y);
                        if (distance >= length)
                        {
                            float displacement = (distance - length) * 100 / distance;
                            var pos = GeometryHelper.GetLocactionBetween(displacement, actualNode.Coordinates, prevCoordinates);
                            pX2 = CalculateXPosition(pos.X);
                            pY2 = CalculateYPosition(pos.Y);
                        }
                        else
                        {
                            pX2 = CalculateXPosition(actualNode.Coordinates.X);
                            pY2 = CalculateYPosition(actualNode.Coordinates.Y);
                            prevCoordinates = actualNode.Coordinates;
                        }

                        if (vehicle.Equals(selectedVehicle))
                        {
                            var selectedTramTail = DirectxHelper.CreateLine(pX, pY, pX2, pY2, ViewConsts.SELECTED_COLOR.ToArgb(), selectedThickness);
                            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, selectedTramTail);
                        }

                        var tramTail = DirectxHelper.CreateLine(pX, pY, pX2, pY2, tramColor.ToArgb(), thickness);
                        device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, tramTail);

                        length -= distance;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void DrawCarInstersections(Device device)
        {
            foreach (var intersection in mainController.CarIntersections)
            {
                float x = CalculateXPosition(intersection.Node.Coordinates.X);
                float y = CalculateYPosition(intersection.Node.Coordinates.Y);
                Color color = intersection.Node.LightState == LightState.Green ? Color.Green :
                              intersection.Node.LightState == LightState.Yellow ? Color.Gold : Color.Red;

                device.DrawUserPrimitives(
                    PrimitiveType.TriangleFan,
                    ViewConsts.POINT_PRECISION,
                    DirectxHelper.CreateCircle(
                        x,
                        y,
                        color.ToArgb(),
                        ViewConsts.POINT_RADIUS * 2,
                        ViewConsts.POINT_PRECISION));
            }
        }

        private float GetPointRadius(float cameraHeight)
        {
            return (cameraHeight * (19f / 99) + (80f / 99)) * ViewConsts.POINT_RADIUS;
        }
        
        private static Color GetLineColor(Node node1, Node node2)
        {
            return node1 != null && node1.IsUnderground && node2 != null && node2.IsUnderground ? ViewConsts.LINE_UNDERGROUND_COLOR : ViewConsts.LINE_BASIC_COLOR;
        }

        #endregion Private Methods
    }
}
