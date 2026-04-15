using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemMaintenanceAlatPertanian
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnAlat_Click(object sender, EventArgs e)
        {
            FormAlat frmAlat = new FormAlat();
            frmAlat.ShowDialog();
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            // Nama variabel di sini adalah 'frmMaintenance'
            FormMaintenance frmMaintenance = new FormMaintenance();

            // Maka di sini juga HARUS memanggil 'frmMaintenance', bukan 'frm'
            frmMaintenance.ShowDialog();
        }

        private void btnTeknisi_Click(object sender, EventArgs e)
        {
            // Pastikan nama class form kamu adalah FormTeknisi
            FormTeknisi frmTeknisi = new FormTeknisi();
            frmTeknisi.ShowDialog();
        }
       

    }
}