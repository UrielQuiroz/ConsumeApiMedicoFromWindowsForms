using ConsumeApiMedicoFromWindowsForms.CapaDatos;
using ConsumeApiMedicoFromWindowsForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsumeApiMedicoFromWindowsForms
{
    public partial class FRMPopupDoctor : Form
    {
        public int IdDoctor { get; set; }

        string nombreArchivo;

        public FRMPopupDoctor()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void FRMPopupDoctor_Load(object sender, EventArgs e)
        {

            await Task.Run(() => { LlenarCombo(); });

            if (IdDoctor == 0)
            {
                this.Text = "Agregar Doctor";
                rbMasculino.Checked = true;
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

                if (model.Sexo == 1)
                {
                    rbMasculino.Checked = true;
                }
                else
                {
                    rbFemenino.Checked = true;
                }

                //data:image/formatoImagen;base64;
                string foto = model.Archivo;
                nombreArchivo = model.nombreArchivo;
                
                if (foto != null && foto != "")
                {
                    string extension = Path.GetExtension(nombreArchivo).Substring(1);
                    foto = foto.Replace("data:image/" + extension + ";base64,", "");
                    byte[] arrayFoto = Convert.FromBase64String(foto);
                    MemoryStream ms = new MemoryStream(arrayFoto);
                    pbFoto.Image = Image.FromStream(ms);
                }
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivo de imagen |*jpg;*png";
            if (ofd.ShowDialog().Equals(DialogResult.OK)) ;
            {
                nombreArchivo = Path.GetFileName(ofd.FileName);
                byte[] buffer = File.ReadAllBytes(ofd.FileName);
                MemoryStream ms = new MemoryStream(buffer);
                pbFoto.Image = Image.FromStream(ms);
            }
        }
    }
}
