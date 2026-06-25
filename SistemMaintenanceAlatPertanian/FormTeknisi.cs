using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemMaintenanceAlatPertanian
{
    public partial class FormTeknisi : Form
    {
        private DAL dataAccess = new DAL();

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtTeknisi = new DataTable();
        private string idTeknisiTerpilih = "";

        public FormTeknisi()
        {
            InitializeComponent();
            btnPilihFoto.Click += btnPilihFoto_Click;
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

        private byte[] ImageToByteArray(Image img)
        {
            if (img == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image img = Image.FromStream(ms);
                return new Bitmap(img);
            }
        }

        private void ClearForm()
        {
            txtNamaTeknisi.Clear();
            pbFotoTeknisi.Image = null;
            idTeknisiTerpilih = "";
            txtNamaTeknisi.Focus();
        }

        private void TampilData()
        {
            try
            {
                dtTeknisi = dataAccess.GetAllTeknisi();
                bindingSource.DataSource = dtTeknisi;
                dgvTeknisi.DataSource = bindingSource;
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

        private void FormTeknisi_Load(object sender, EventArgs e)
        {
            bindingNavigator1.BindingSource = bindingSource;
            dgvTeknisi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTeknisi.MultiSelect = false;
            dgvTeknisi.ReadOnly = true;
            dgvTeknisi.AllowUserToAddRows = false;
            dgvTeknisi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            TampilData();
        }

        private void btnPilihFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Pilih Foto Teknisi";
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbFotoTeknisi.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaTeknisi.Text))
            {
                MessageBox.Show("Nama Teknisi wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                byte[] fotoBytes = ImageToByteArray(pbFotoTeknisi.Image);
                bool sukses = dataAccess.InsertTeknisi(txtNamaTeknisi.Text, fotoBytes);

                if (sukses)
                {
                    MessageBox.Show("Data teknisi berhasil ditambahkan");
                    ClearForm();
                    TampilData();
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
            DataRowView row = (DataRowView)bindingSource.Current;
            if (row == null)
            {
                MessageBox.Show("Pilih data dari tabel terlebih dahulu!");
                return;
            }

            try
            {
                int idTeknisi = Convert.ToInt32(row["id_teknisi"]);
                byte[] fotoBytes = ImageToByteArray(pbFotoTeknisi.Image);

                bool sukses = dataAccess.UpdateTeknisi(idTeknisi, txtNamaTeknisi.Text, fotoBytes);

                if (sukses)
                {
                    MessageBox.Show("Data berhasil diperbarui");
                    ClearForm();
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
            DataRowView row = (DataRowView)bindingSource.Current;
            if (row == null)
            {
                MessageBox.Show("Pilih data dari tabel terlebih dahulu!");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int idTeknisi = Convert.ToInt32(row["id_teknisi"]);
                    bool sukses = dataAccess.DeleteTeknisi(idTeknisi);

                    if (sukses)
                    {
                        MessageBox.Show("Data berhasil dihapus");
                        ClearForm();
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
                DataTable dtCari = dataAccess.SearchTeknisi(txtCari.Text);
                bindingSource.DataSource = dtCari;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi penolakan dari Database:\n" + sqlEx.Message, "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CatatLogError(sqlEx.Message, "txtCari_TextChanged");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mencari data: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CatatLogError(ex.Message, "txtCari_TextChanged");
            }
        }

        private void dgvTeknisi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRowView row = (DataRowView)bindingSource.Current;
                if (row != null)
                {
                    idTeknisiTerpilih = row["id_teknisi"].ToString();

                    txtNamaTeknisi.Text = row["nama_teknisi"].ToString();

                    if (row["foto_teknisi"] != DBNull.Value)
                    {
                        byte[] fotoBytes = (byte[])row["foto_teknisi"];
                        pbFotoTeknisi.Image = ByteArrayToImage(fotoBytes);
                    }
                    else
                    {
                        pbFotoTeknisi.Image = null;
                    }
                }
            }
        }

        private void txtNamaTeknisi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {

                e.Handled = true;


                System.Media.SystemSounds.Beep.Play();
            }
        }
    }
}