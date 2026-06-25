namespace SistemMaintenanceAlatPertanian
{
    partial class FormDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnLoadGrafik = new System.Windows.Forms.Button();
            this.chartKondisi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cmbTipeGrafik = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartKondisi)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadGrafik
            // 
            this.btnLoadGrafik.BackColor = System.Drawing.Color.Silver;
            this.btnLoadGrafik.Location = new System.Drawing.Point(864, 107);
            this.btnLoadGrafik.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadGrafik.Name = "btnLoadGrafik";
            this.btnLoadGrafik.Size = new System.Drawing.Size(160, 48);
            this.btnLoadGrafik.TabIndex = 0;
            this.btnLoadGrafik.Text = "LOAD";
            this.btnLoadGrafik.UseVisualStyleBackColor = false;
            // 
            // chartKondisi
            // 
            chartArea1.Name = "ChartArea1";
            this.chartKondisi.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartKondisi.Legends.Add(legend1);
            this.chartKondisi.Location = new System.Drawing.Point(39, 61);
            this.chartKondisi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartKondisi.Name = "chartKondisi";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartKondisi.Series.Add(series1);
            this.chartKondisi.Size = new System.Drawing.Size(789, 526);
            this.chartKondisi.TabIndex = 1;
            this.chartKondisi.Text = "chart1";
            // 
            // cmbTipeGrafik
            // 
            this.cmbTipeGrafik.FormattingEnabled = true;
            this.cmbTipeGrafik.Location = new System.Drawing.Point(864, 61);
            this.cmbTipeGrafik.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTipeGrafik.Name = "cmbTipeGrafik";
            this.cmbTipeGrafik.Size = new System.Drawing.Size(160, 24);
            this.cmbTipeGrafik.TabIndex = 2;
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(1054, 624);
            this.Controls.Add(this.cmbTipeGrafik);
            this.Controls.Add(this.chartKondisi);
            this.Controls.Add(this.btnLoadGrafik);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormDashboard";
            this.Text = "FormDashboard";
            ((System.ComponentModel.ISupportInitialize)(this.chartKondisi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadGrafik;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKondisi;
        private System.Windows.Forms.ComboBox cmbTipeGrafik;
    }
}