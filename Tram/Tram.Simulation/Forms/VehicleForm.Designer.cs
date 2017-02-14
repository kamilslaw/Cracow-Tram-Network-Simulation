namespace Tram.Simulation.Forms
{
    partial class VehicleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.summaryTabPage = new System.Windows.Forms.TabPage();
            this.capacityTabPage = new System.Windows.Forms.TabPage();
            this.delayTabPage = new System.Windows.Forms.TabPage();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.capacityChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.delayChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl.SuspendLayout();
            this.summaryTabPage.SuspendLayout();
            this.capacityTabPage.SuspendLayout();
            this.delayTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.capacityChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.summaryTabPage);
            this.tabControl.Controls.Add(this.capacityTabPage);
            this.tabControl.Controls.Add(this.delayTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(501, 400);
            this.tabControl.TabIndex = 0;
            // 
            // summaryTabPage
            // 
            this.summaryTabPage.BackColor = System.Drawing.Color.White;
            this.summaryTabPage.Controls.Add(this.propertiesLabel);
            this.summaryTabPage.Location = new System.Drawing.Point(4, 22);
            this.summaryTabPage.Name = "summaryTabPage";
            this.summaryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.summaryTabPage.Size = new System.Drawing.Size(493, 374);
            this.summaryTabPage.TabIndex = 0;
            this.summaryTabPage.Text = "Podsumowanie";
            // 
            // capacityTabPage
            // 
            this.capacityTabPage.BackColor = System.Drawing.Color.White;
            this.capacityTabPage.Controls.Add(this.capacityChart);
            this.capacityTabPage.Location = new System.Drawing.Point(4, 22);
            this.capacityTabPage.Name = "capacityTabPage";
            this.capacityTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.capacityTabPage.Size = new System.Drawing.Size(493, 374);
            this.capacityTabPage.TabIndex = 1;
            this.capacityTabPage.Text = "Zapełnienie";
            // 
            // delayTabPage
            // 
            this.delayTabPage.BackColor = System.Drawing.Color.White;
            this.delayTabPage.Controls.Add(this.delayChart);
            this.delayTabPage.Location = new System.Drawing.Point(4, 22);
            this.delayTabPage.Name = "delayTabPage";
            this.delayTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.delayTabPage.Size = new System.Drawing.Size(493, 374);
            this.delayTabPage.TabIndex = 2;
            this.delayTabPage.Text = "Opóźnienie [s]";
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.propertiesLabel.Location = new System.Drawing.Point(3, 3);
            this.propertiesLabel.Margin = new System.Windows.Forms.Padding(0);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Padding = new System.Windows.Forms.Padding(10);
            this.propertiesLabel.Size = new System.Drawing.Size(487, 368);
            this.propertiesLabel.TabIndex = 0;
            this.propertiesLabel.Text = "Właściwości";
            // 
            // capacityChart
            // 
            chartArea4.Name = "ChartArea1";
            this.capacityChart.ChartAreas.Add(chartArea4);
            this.capacityChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.capacityChart.Location = new System.Drawing.Point(3, 3);
            this.capacityChart.Name = "capacityChart";
            this.capacityChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Chocolate;
            series4.BorderWidth = 3;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Name = "Series1";
            this.capacityChart.Series.Add(series4);
            this.capacityChart.Size = new System.Drawing.Size(487, 368);
            this.capacityChart.TabIndex = 0;
            this.capacityChart.Text = "chart1";
            // 
            // delayChart
            // 
            chartArea3.Name = "ChartArea1";
            this.delayChart.ChartAreas.Add(chartArea3);
            this.delayChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delayChart.Location = new System.Drawing.Point(3, 3);
            this.delayChart.Name = "delayChart";
            this.delayChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Name = "Series1";
            this.delayChart.Series.Add(series3);
            this.delayChart.Size = new System.Drawing.Size(487, 368);
            this.delayChart.TabIndex = 1;
            this.delayChart.Text = "chart1";
            // 
            // VehicleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(501, 400);
            this.Controls.Add(this.tabControl);
            this.Name = "VehicleForm";
            this.Text = "VehicleForm";
            this.tabControl.ResumeLayout(false);
            this.summaryTabPage.ResumeLayout(false);
            this.capacityTabPage.ResumeLayout(false);
            this.delayTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.capacityChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage summaryTabPage;
        private System.Windows.Forms.TabPage capacityTabPage;
        private System.Windows.Forms.TabPage delayTabPage;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart capacityChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart delayChart;
    }
}