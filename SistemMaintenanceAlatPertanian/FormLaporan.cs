using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormLaporan : Form
    {
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        public FormLaporan()
        {
            InitializeComponent();
            
            this.Load += FormLaporan_Load;
        }

        private void FormLaporan_Load(object sender, EventArgs e)
        {
            TampilkanLaporan();
        }

        private void TampilkanLaporan()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LaporanMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Tidak ada data riwayat perbaikan yang ditemukan di database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        RptMaintenance laporan = new RptMaintenance();
                        laporan.SetDataSource(dt);

                        crystalReportViewer1.ReportSource = laporan;
                        crystalReportViewer1.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat laporan: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            
        }
    }
}