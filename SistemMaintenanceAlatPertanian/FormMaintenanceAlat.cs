using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormMaintenance : Form
    {
        private DAL dataAccess = new DAL();

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
                DataTable dt = dataAccess.GetAllAlat();
                cbAlat.DataSource = dt;
                cbAlat.DisplayMember = "nama_alat";
                cbAlat.ValueMember = "id_alat";
                cbAlat.SelectedIndex = -1;
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
                DataTable dt = dataAccess.GetAllTeknisi();
                cbTeknisi.DataSource = dt;
                cbTeknisi.DisplayMember = "nama_teknisi";
                cbTeknisi.ValueMember = "id_teknisi";
                cbTeknisi.SelectedIndex = -1;
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
                dtMaintenance = dataAccess.GetAllMaintenance();
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
                int idAlat = Convert.ToInt32(cbAlat.SelectedValue);
                int idTeknisi = Convert.ToInt32(cbTeknisi.SelectedValue);

                bool sukses = dataAccess.InsertMaintenance(idAlat, idTeknisi, dtpTanggal.Value.Date, cbJenisPerbaikan.Text, txtKeterangan.Text);

                if (sukses)
                {
                    MessageBox.Show("Data maintenance berhasil ditambahkan");
                    TampilData();
                    ClearForm();
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
                int idMaintenance = Convert.ToInt32(currentRecord["id_maintenance"]);
                int idAlat = Convert.ToInt32(cbAlat.SelectedValue);
                int idTeknisi = Convert.ToInt32(cbTeknisi.SelectedValue);

                bool sukses = dataAccess.UpdateMaintenance(idMaintenance, idAlat, idTeknisi, dtpTanggal.Value.Date, cbJenisPerbaikan.Text, txtKeterangan.Text);

                if (sukses)
                {
                    MessageBox.Show("Data berhasil diperbarui");
                    TampilData();
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
                    int idMaintenance = Convert.ToInt32(currentRecord["id_maintenance"]);
                    bool sukses = dataAccess.DeleteMaintenance(idMaintenance);

                    if (sukses)
                    {
                        MessageBox.Show("Data berhasil dihapus");
                        TampilData();
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
                DataTable dtCari = dataAccess.SearchMaintenance(txtCari.Text);
                bindingSource.DataSource = dtCari;

                if (dgvMaintenance.Columns.Contains("id_alat"))
                    dgvMaintenance.Columns["id_alat"].Visible = false;

                if (dgvMaintenance.Columns.Contains("id_teknisi"))
                    dgvMaintenance.Columns["id_teknisi"].Visible = false;
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