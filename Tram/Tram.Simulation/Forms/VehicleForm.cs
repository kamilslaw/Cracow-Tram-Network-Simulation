using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Tram.Common.Helpers;
using Tram.Common.Models;

namespace Tram.Simulation.Forms
{
    public partial class VehicleForm : Form
    {
        public Vehicle Vehicle { get; set; }

        public VehicleForm(Vehicle vehicle)
        {
            InitializeComponent();
            Vehicle = vehicle;
        }

        public void Init()
        {
            Text = Vehicle.Id;
            InitSummary();
            InitCapacity();
            InitDelay();
        }

        private void InitSummary()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Linia: ");
            sb.Append(Vehicle.Line.Id);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("Godzina startu: ");
            sb.Append(TimeHelper.GetTimeStr(Vehicle.StartTime));
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("Zapełnienie: ");
            sb.Append(Vehicle.Passengers);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("Szybkość: ");
            sb.Append(Vehicle.Speed.ToString("N2"));
            sb.Append("km/h");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("Współrzędne: ");
            sb.Append("N: ");
            sb.Append(Vehicle.Position.Coordinates.Y.ToString("N4"));
            sb.Append("   E: ");
            sb.Append(Vehicle.Position.Coordinates.X.ToString("N4"));
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("Ilość odwiedzonych przystanków: ");
            sb.Append(Vehicle.LastVisitedStops.Count);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            propertiesLabel.Text = sb.ToString();
        }

        private void InitCapacity()
        {
            capacityChart.Series[0].ChartType = SeriesChartType.Line;
            capacityChart.ChartAreas[0].AxisX.ScaleView.Zoom(0, Vehicle.PassengersHistory.Count);
            capacityChart.ChartAreas[0].AxisY.ScaleView.Zoom(0, Vehicle.PassengersHistory.Max() + 5);
            capacityChart.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0}";
            capacityChart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0}";
            capacityChart.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            capacityChart.ChartAreas[0].AxisY.ScrollBar.Enabled = false;

            for (int i = 0; i < Vehicle.PassengersHistory.Count; i++)
            {
                capacityChart.Series[0].Points.AddXY(i, Vehicle.PassengersHistory[i]);
            }
        }

        private void InitDelay()
        {
            delayChart.Series[0].ChartType = SeriesChartType.Line;
            delayChart.ChartAreas[0].AxisX.ScaleView.Zoom(0, Vehicle.DelaysHistory.Count);
            delayChart.ChartAreas[0].AxisY.ScaleView.Zoom(0, Vehicle.DelaysHistory.Max() + 1);
            delayChart.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0}";
            delayChart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0}";
            delayChart.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            delayChart.ChartAreas[0].AxisY.ScrollBar.Enabled = false;

            for (int i = 0; i < Vehicle.PassengersHistory.Count; i++)
            {
                delayChart.Series[0].Points.AddXY(i, Vehicle.DelaysHistory[i]);
            }
        }
    }
}
