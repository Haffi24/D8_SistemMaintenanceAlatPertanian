using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormMaintenance : Form
    {
        private readonly string connectionString = @"Data Source=LAPTOP-D3717QUD\USERHAFFI; Initial Catalog=DBMaintenanceAlat; Integrated Security=True;";

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMaintenance = new DataTable();
        private string idMaintenanceTerpilih = "";

        public FormMaintenance()
        {
            InitializeComponent();
        }

        private void CatatLogError(string pesanError, string lokasiError)
        {
            try
            {
                string folderPath = Application.StartupPath + "\\Logs";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = folderPath + "\\ErrorLog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                string formatLog = $"[{DateTime.Now.ToString("HH:mm:ss")}] ERROR di {lokasiError} : {pesanError}";

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(formatLog);
                    sw.WriteLine(new string('-', 50));
                }
            }
            catch
            {
            }
        }

        private void ClearForm()
        {
            cbAlat.SelectedIndex = -1;
            cbTeknisi.SelectedIndex = -1;
            cbJenisPerbaikan.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Today;
            txtKeterangan.Clear();
            idMaintenanceTerpilih = "";
            cbAlat.Focus();
        }

        private void BindControls()
        {
            cbAlat.DataBindings.Clear();
            cbTeknisi.DataBindings.Clear();
            cbJenisPerbaikan.DataBindings.Clear();
            dtpTanggal.DataBindings.Clear();
            txtKeterangan.DataBindings.Clear();

            cbAlat.DataBindings.Add("SelectedValue", bindingSource, "id_alat", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTeknisi.DataBindings.Add("SelectedValue", bindingSource, "id_teknisi", true, DataSourceUpdateMode.OnPropertyChanged);
            cbJenisPerbaikan.DataBindings.Add("Text", bindingSource, "jenis_perbaikan", true, DataSourceUpdateMode.OnPropertyChanged);
            dtpTanggal.DataBindings.Add("Value", bindingSource, "tgl_service", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKeterangan.DataBindings.Add("Text", bindingSource, "keterangan", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void LoadAlat()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT id_alat, nama_alat FROM Alat";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbAlat.DataSource = dt;
                    cbAlat.DisplayMember = "nama_alat";
                    cbAlat.ValueMember = "id_alat";
                    cbAlat.SelectedIndex = -1;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "LoadAlat");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "LoadAlat");
            }
        }

        private void LoadTeknisi()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT id_teknisi, nama_teknisi FROM Teknisi";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbTeknisi.DataSource = dt;
                    cbTeknisi.DisplayMember = "nama_teknisi";
                    cbTeknisi.ValueMember = "id_teknisi";
                    cbTeknisi.SelectedIndex = -1;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "LoadTeknisi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "LoadTeknisi");
            }
        }

        private void TampilData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT m.id_maintenance, m.id_alat, a.nama_alat, m.id_teknisi, t.nama_teknisi, m.tgl_service, m.jenis_perbaikan, m.keterangan " +
                                   "FROM Maintenance m JOIN Alat a ON m.id_alat = a.id_alat JOIN Teknisi t ON m.id_teknisi = t.id_teknisi";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtMaintenance = new DataTable();
                    da.Fill(dtMaintenance);

                    bindingSource.DataSource = dtMaintenance;
                    dgvMaintenance.DataSource = bindingSource;

                    dgvMaintenance.Columns["id_maintenance"].HeaderText = "ID";
                    dgvMaintenance.Columns["nama_alat"].HeaderText = "Nama Alat";
                    dgvMaintenance.Columns["nama_teknisi"].HeaderText = "Teknisi";
                    dgvMaintenance.Columns["tgl_service"].HeaderText = "Tanggal";
                    dgvMaintenance.Columns["jenis_perbaikan"].HeaderText = "Jenis Perbaikan";
                    dgvMaintenance.Columns["keterangan"].HeaderText = "Keterangan";

                    dgvMaintenance.Columns["id_alat"].Visible = false;
                    dgvMaintenance.Columns["id_teknisi"].Visible = false;

                    BindControls();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "TampilData");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "TampilData");
            }
        }

        private void FormMaintenance_Load(object sender, EventArgs e)
        {
            bindingNavigator1.BindingSource = bindingSource;

            dgvMaintenance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaintenance.MultiSelect = false;
            dgvMaintenance.ReadOnly = true;
            dgvMaintenance.AllowUserToAddRows = false;
            dgvMaintenance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cbJenisPerbaikan.Items.Clear();
            cbJenisPerbaikan.Items.Add("Perbaikan Ringan");
            cbJenisPerbaikan.Items.Add("Perbaikan Berat");

            cbJenisPerbaikan.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAlat.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTeknisi.DropDownStyle = ComboBoxStyle.DropDownList;

           
            dtpTanggal.MinDate = DateTime.Today;
            dtpTanggal.MaxDate = DateTime.Today;
            dtpTanggal.Value = DateTime.Today;
           

            LoadAlat();
            LoadTeknisi();
            TampilData();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cbAlat.SelectedIndex == -1 || cbTeknisi.SelectedIndex == -1 || cbJenisPerbaikan.SelectedIndex == -1)
            {
                MessageBox.Show("Alat, Teknisi, dan Jenis Perbaikan wajib dipilih!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_alat", cbAlat.SelectedValue);
                        cmd.Parameters.AddWithValue("@id_teknisi", cbTeknisi.SelectedValue);
                        cmd.Parameters.AddWithValue("@tgl_service", dtpTanggal.Value.Date);
                        cmd.Parameters.AddWithValue("@jenis_perbaikan", cbJenisPerbaikan.Text);
                        cmd.Parameters.AddWithValue("@keterangan", txtKeterangan.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data maintenance berhasil ditambahkan");
                        TampilData();
                        ClearForm();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "btnSimpan_Click");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "btnSimpan_Click");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataRowView currentRecord = (DataRowView)bindingSource.Current;
            if (currentRecord == null) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_maintenance", currentRecord["id_maintenance"]);
                        cmd.Parameters.AddWithValue("@id_alat", cbAlat.SelectedValue);
                        cmd.Parameters.AddWithValue("@id_teknisi", cbTeknisi.SelectedValue);
                        cmd.Parameters.AddWithValue("@tgl_service", dtpTanggal.Value.Date);
                        cmd.Parameters.AddWithValue("@jenis_perbaikan", cbJenisPerbaikan.Text);
                        cmd.Parameters.AddWithValue("@keterangan", txtKeterangan.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil diperbarui");
                        TampilData();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "btnUpdate_Click");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "btnUpdate_Click");
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            DataRowView currentRecord = (DataRowView)bindingSource.Current;
            if (currentRecord == null) return;

            if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteMaintenance", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_maintenance", currentRecord["id_maintenance"]);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data berhasil dihapus");
                            TampilData();
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CatatLogError(sqlEx.Message, "btnHapus_Click");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CatatLogError(ex.Message, "btnHapus_Click");
                }
            }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SearchMaintenance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        bindingSource.DataSource = dt;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "txtCari_TextChanged");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem:\n" + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "txtCari_TextChanged");
            }
        }

        private void dgvMaintenance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRowView row = (DataRowView)bindingSource.Current;
                if (row != null)
                {
                    idMaintenanceTerpilih = row["id_maintenance"].ToString();
                }
            }
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            
                FormLaporan frmLaporan = new FormLaporan();
                frmLaporan.ShowDialog();
            
        }

        private void dtpTanggal_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}