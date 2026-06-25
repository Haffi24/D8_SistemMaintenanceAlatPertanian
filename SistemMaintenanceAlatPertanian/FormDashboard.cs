using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormDashboard : Form
    {
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        public FormDashboard()
        {
            InitializeComponent();
            this.Load += FormDashboard_Load;
            btnLoadGrafik.Click += btnLoadGrafik_Click;
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            cmbTipeGrafik.DropDownStyle = ComboBoxStyle.DropDownList;

            var items = new List<KeyValuePair<string, SeriesChartType>>
            {
                new KeyValuePair<string, SeriesChartType>("Kolom", SeriesChartType.Column),
                new KeyValuePair<string, SeriesChartType>("Pie", SeriesChartType.Pie)
            };

            cmbTipeGrafik.DataSource = items;
            cmbTipeGrafik.DisplayMember = "Key";
            cmbTipeGrafik.ValueMember = "Value";
            cmbTipeGrafik.SelectedIndex = 0;

            LoadDataGrafik();
        }

        private void btnLoadGrafik_Click(object sender, EventArgs e)
        {
            LoadDataGrafik();
        }

        private void LoadDataGrafik()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GrafikKondisiAlat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        chartKondisi.Series.Clear();
                        chartKondisi.Titles.Clear();
                        chartKondisi.Legends.Clear();

                        SeriesChartType tipe = (SeriesChartType)cmbTipeGrafik.SelectedValue;
                        Series s = new Series("Kondisi Alat");
                        s.ChartType = tipe;
                        s.IsValueShownAsLabel = true;

                        foreach (DataRow row in dt.Rows)
                        {
                            string kondisi = row["Kondisi"].ToString();
                            int jumlah = Convert.ToInt32(row["Jumlah"]);
                            s.Points.AddXY(kondisi, jumlah);
                        }

                        chartKondisi.Series.Add(s);

                        Title title = new Title("Grafik Jumlah Alat Berdasarkan Kondisi Fisik", Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue);
                        chartKondisi.Titles.Add(title);

                        Legend legend = new Legend("MainLegend");
                        legend.Docking = Docking.Right;
                        chartKondisi.Legends.Add(legend);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat grafik: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}