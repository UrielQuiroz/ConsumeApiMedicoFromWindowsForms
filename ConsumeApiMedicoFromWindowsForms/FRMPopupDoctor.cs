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
    public partial class FRMPopupDoctor : Form
    {
        public int IdDoctor { get; set; }

        public FRMPopupDoctor()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void FRMPopupDoctor_Load(object sender, EventArgs e)
        {
            rbMasculino.Checked = true;

            await Task.Run(() => { LlenarCombo(); });

            if (IdDoctor == 0)
            {
                this.Text = "Agregar Doctor";
            }
            else
            {
                this.Text = "Editar Doctor";
                DoctorDAL DAL = new DoctorDAL();
                DoctorModel model = await DAL.recuperarDoctor(IdDoctor);

                txtId.Text = model.IdDoctor.ToString();
                txtNombre.Text = model.nombre;
                txtApPaterno.Text = model.ApPaterno;
                txtApMaterno.Text = model.ApMaterno;
                cboClinica.SelectedValue = model.IdClinica;
                cboEspecialidad.SelectedValue = model.IdEspecialidad;
                txtEmail.Text = model.Email;
                txtCelular.Text = model.celular == null?"": model.celular.ToString();
                txtSueldo.Text = model.Sueldo.ToString();
            }
        }

        private async void LlenarCombo()
        {
            DoctorDAL DAL = new DoctorDAL();
            List<ClinicaModel> listClinica = await DAL.listarClinica();
            List<EspecialidadModel> listEspecialidad = await DAL.listarEspecialidad();
            listClinica.Insert(0, new ClinicaModel { IdClinica = 0, Nombre = "--SELECCIONE--" });
            listEspecialidad.Insert(0, new EspecialidadModel { IdEspecialidad = 0, Nombre = "--SELECCIONE--" });

            this.Invoke(new MethodInvoker(() =>
            {
                //Clinica
                cboClinica.DataSource = listClinica;
                cboClinica.DisplayMember = "Nombre";
                cboClinica.ValueMember = "IdClinica";

                //Especialidad
                cboEspecialidad.DataSource = listEspecialidad;
                cboEspecialidad.DisplayMember = "Nombre";
                cboEspecialidad.ValueMember = "IdEspecialidad";
            }));
        }
    }
}
