using ConsumeApiMedicoFromWindowsForms.CapaDatos;
using ConsumeApiMedicoFromWindowsForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsumeApiMedicoFromWindowsForms
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListarDoctores();
        }

        public async void ListarDoctores()
        {
            DoctorDAL oDoctorDAL = new DoctorDAL();
            List<DoctorModel> model = await oDoctorDAL.listarDoctor();
            dgvDoctor.DataSource = model;
            for (int i = 6; i < dgvDoctor.Columns.Count; i++)
            {
                dgvDoctor.Columns[i].Visible = false;
            }
        }

        private void toolStripNuevo_Click(object sender, EventArgs e)
        {
            FRMPopupDoctor frm = new FRMPopupDoctor();
            frm.IdDoctor = 0;
            frm.ShowDialog();
            if (frm.DialogResult.Equals(DialogResult.OK))
            {
                ListarDoctores();
            }
        }

        private void toolStripEditar_Click(object sender, EventArgs e)
        {
            FRMPopupDoctor frm = new FRMPopupDoctor();
            int idDoc = (int) dgvDoctor.CurrentRow.Cells[0].Value;
            frm.IdDoctor = idDoc;
            frm.ShowDialog();
            if (frm.DialogResult.Equals(DialogResult.OK))
            {
                ListarDoctores();
            }
        }

        private async void toolStripEliminar_Click(object sender, EventArgs e)
        {
            int idDoc = (int) dgvDoctor.CurrentRow.Cells[0].Value;
            DoctorDAL DAL = new DoctorDAL();
            int rpta = await DAL.eliminarDoctor(idDoc);

            if (rpta == 1)
            {
                MessageBox.Show("Se elimino correctamente");
                ListarDoctores();
            }
            else
            {
                MessageBox.Show("Ocurrio un error!");
            }
        }
    }
}
