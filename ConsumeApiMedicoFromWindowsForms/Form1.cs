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
    }
}
