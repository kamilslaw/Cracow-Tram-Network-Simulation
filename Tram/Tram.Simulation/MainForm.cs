using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Drawing;
using System.Windows.Forms;
using Tram.Common.Consts;
using Tram.Simulation.Properties;
using Tram.Controller.Controllers;
using Tram.Common.Models;
using System.Collections.Generic;
using Tram.Simulation.Forms;

namespace Tram.Simulation
{
    public partial class MainForm : Form
    {
        private MainController controller;
        private DirectxController directxController;
        private Device device;

        private Vector3 cameraPosition, cameraTarget;
        private Point lastClickedMouseLocation;

        private DateTime lastUpdateTime;

        private Vehicle selectedVehicle;
        private List<Vehicle> vehicles;

        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            SetLanguage();

            // Set controls
            playButton.Visible = false;
            pauseButton.Location = playButton.Location;

            // Set handlers
            renderPanel.MouseWheel += RenderPanel_MouseWheel;
            speedCustomTrackBar.OnBarValueChanged += SpeedCustomTrackBar_OnBarValueChanged;
            playButton.Click += PlayButton_Click;
            pauseButton.Click += PauseButton_Click;
            vehiclesGridView.CellClick += vehiclesGridView_CellClick;
            aboutUsButton.Click += AboutUsButton_Click;

            // Set variables
            cameraPosition = new Vector3(0, 0, ViewConsts.START_CAMERA_Z);
            cameraTarget = new Vector3(0, 0, 0);
            vehicles = new List<Vehicle>();
            lastUpdateTime = DateTime.Now;
        }

        public void Init(MainController controller, DirectxController directxController)
        {
            InitializeGraphics();
            this.controller = controller;
            this.directxController = directxController;
        }

        #region Public Methods

        public void SetLanguage()
        {
            Text = Resources.Window_Title;
        }

        public void UpdateForm()
        {
            if ((DateTime.Now - lastUpdateTime).TotalSeconds > CalculationConsts.INTERFACE_REFRESH_TIME_INTERVAL)
            {
                lastUpdateTime = DateTime.Now;

                int selectedRowIndex = -1;
                
                vehicles.Clear();
                vehiclesGridView.Rows.Clear();
                controller.Vehicles.ForEach(vehicle =>
                {
                    vehicles.Add(vehicle);
                    vehiclesGridView.Rows.Add(vehicle.Id);
                    if (vehicle.Equals(selectedVehicle))
                    {
                        selectedRowIndex = vehiclesGridView.Rows.Count - 1;
                    }
                });

                if (selectedRowIndex >= 0)
                {
                    vehiclesGridView.Rows[selectedRowIndex].Selected = true;
                }
            }
            
            if (selectedVehicle != null)
            {
                cameraPosition = new Vector3(
                    directxController.CalculateXPosition(selectedVehicle.Position.Coordinates.X),
                    directxController.CalculateYPosition(selectedVehicle.Position.Coordinates.Y), 
                    ViewConsts.SELECTED_VEHICLE_ZOOM_OFFSET);
                cameraTarget.X = cameraPosition.X;
                cameraTarget.Y = cameraPosition.Y;
            }
        }

        public void Render(Action<Device, Vector3, Vehicle> renderAction)
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, renderPanel.Width / renderPanel.Height, 1f, 1000f);
            device.Transform.View = Matrix.LookAtLH(cameraPosition, cameraTarget, new Vector3(0, 1, 0));
            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.None;

            device.Clear(ClearFlags.Target, Color.WhiteSmoke, 1.0f, 0);
            
            device.BeginScene();

            device.VertexFormat = CustomVertex.PositionColored.Format;

            //Invoke render action
            renderAction(device, cameraPosition, selectedVehicle);

            device.EndScene();
            device.Present();
            Invalidate();
        }

        #endregion Public Methods

        #region Utils

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xf020;

        // cancel a form minimize
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt32() == SC_MINIMIZE)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        #endregion Utils

        #region Private Methods

        private bool InitializeGraphics()
        {
            try
            {
                PresentParameters presentParams = new PresentParameters();
                presentParams.Windowed = true;
                presentParams.SwapEffect = SwapEffect.Discard;
                device = new Device(0, DeviceType.Hardware, renderPanel, CreateFlags.MixedVertexProcessing, presentParams);
                return true;
            }
            catch (DirectXException)
            {
                return false;
            }
        }

        #endregion Private Methods

        #region Private Handlers

        private void SpeedCustomTrackBar_OnBarValueChanged(object sender, int e)
        {
            if (controller.SimulationSpeed > 0)
            {
                controller.SimulationSpeed = e;
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            controller.SimulationSpeed = 0;
            pauseButton.Visible = false;
            playButton.Visible = true;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            controller.SimulationSpeed = speedCustomTrackBar.Value;
            playButton.Visible = false;
            pauseButton.Visible = true;
        }

        private void vehiclesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (vehiclesGridView.CurrentCell.ColumnIndex.Equals(1))
                {
                    selectedVehicle = vehicles[e.RowIndex];
                }
                else if (vehiclesGridView.CurrentCell.ColumnIndex.Equals(2))
                {
                    VehicleForm vehicleForm = new VehicleForm(vehicles[e.RowIndex]);
                    vehicleForm.Init();
                    vehicleForm.Show();
                }
            }
        }
        
        private void AboutUsButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Modelowanie i symulacja systemów 2017" + Environment.NewLine
                + Environment.NewLine + "Katarzyna Burczyk" 
                + Environment.NewLine + "Zuzanna Drwiła" 
                + Environment.NewLine + "Kamil Gębarowski",
                "Symulacja krakowskiej sieci tramwajowej");
        }

        #endregion Private Handlers

        #region Map Transformation Methods

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            selectedVehicle = null;
            if (cameraPosition.Z > ViewConsts.ZOOM_OFFSET)
            {
                cameraPosition.Z -= ViewConsts.ZOOM_OFFSET;
            }
            else if (cameraPosition.Z > 1 + (ViewConsts.ZOOM_OFFSET / 20))
            {
                cameraPosition.Z -= ViewConsts.ZOOM_OFFSET / 20;
            }
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            selectedVehicle = null;
            if (cameraPosition.Z < ViewConsts.ZOOM_OFFSET)
            {
                cameraPosition.Z += ViewConsts.ZOOM_OFFSET / 20;
            }
            else if (cameraPosition.Z < ViewConsts.START_CAMERA_Z * 1.5)
            {
                cameraPosition.Z += ViewConsts.ZOOM_OFFSET;
            }

            if (cameraPosition.Z > ViewConsts.START_CAMERA_Z * 1.5)
            {
                cameraPosition.Z = ViewConsts.START_CAMERA_Z * 1.5f;
            }
        }

        private void RenderPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            selectedVehicle = null;
            if (e.Delta > 0)
            {
                ZoomInButton_Click(this, new EventArgs());
            }
            else if (e.Delta < 0)
            {
                ZoomOutButton_Click(this, new EventArgs());
            }
        }

        private void zoomOriginalButton_Click(object sender, EventArgs e)
        {
            selectedVehicle = null;
            cameraPosition.Z = ViewConsts.START_CAMERA_Z;
        }

        private void centerScreenButton_Click(object sender, EventArgs e)
        {
            selectedVehicle = null;
            cameraPosition.X = cameraPosition.Y = cameraTarget.X = cameraTarget.Y = 0;
        }

        private void renderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                selectedVehicle = null;
                if (!lastClickedMouseLocation.IsEmpty && lastClickedMouseLocation != e.Location)
                {
                    float xDiff = Math.Abs(lastClickedMouseLocation.X - e.Location.X);
                    if (lastClickedMouseLocation.X > e.Location.X)
                    {
                        cameraPosition.X -= ViewConsts.SWIPE_OFFSET * cameraPosition.Z * xDiff;
                    }
                    else if (lastClickedMouseLocation.X < e.Location.X)
                    {
                        cameraPosition.X += ViewConsts.SWIPE_OFFSET * cameraPosition.Z * xDiff;
                    }

                    float yDiff = Math.Abs(lastClickedMouseLocation.Y - e.Location.Y);
                    if (lastClickedMouseLocation.Y > e.Location.Y)
                    {
                        cameraPosition.Y -= ViewConsts.SWIPE_OFFSET * cameraPosition.Z * yDiff;
                    }
                    else if (lastClickedMouseLocation.Y < e.Location.Y)
                    {
                        cameraPosition.Y += ViewConsts.SWIPE_OFFSET * cameraPosition.Z * yDiff;
                    }

                    cameraTarget.X = cameraPosition.X;
                    cameraTarget.Y = cameraPosition.Y;
                }

                lastClickedMouseLocation = e.Location;
            }
            else
            {
                lastClickedMouseLocation = Point.Empty;
            }
        }

        #endregion Map Transformation Methods
    }
}
