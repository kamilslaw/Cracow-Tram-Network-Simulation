using System;
using System.Windows.Forms;
using Tram.Common.Helpers;
using Tram.Controller;
using Tram.Controller.Controllers;

namespace Tram.Simulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainController controller = Kernel.Get<MainController>();
            DirectxController directxController = Kernel.Get<DirectxController>();
            controller.StartSimulation(TimeHelper.GetTime("05:30"));

            using (MainForm form = new MainForm())
            {
                int screenHeight = Screen.PrimaryScreen.Bounds.Height - 60;
                form.Size = new System.Drawing.Size(screenHeight * form.Width / form.Height, screenHeight);
                form.Init(controller, directxController);
                form.Show();
                
                while (form.Created)
                {
                    controller.Update(); // UPDATE SIMULATION
                    form.Render(controller.Render); //RENDER SIMULATION
                    form.UpdateForm(); // UPDATE WINDOW
                    Application.DoEvents();
                }
            }
        }
    }
}
