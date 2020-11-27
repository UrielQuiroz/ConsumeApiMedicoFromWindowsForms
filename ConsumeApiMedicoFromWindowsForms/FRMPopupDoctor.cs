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

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            bool exito = true;
            if (txtNombre.Text == ""){ errorDatos.SetError(txtNombre, "Ingresar nombre"); exito = false; } else{ errorDatos.SetError(txtNombre, ""); }
            if (txtApPaterno.Text == ""){ errorDatos.SetError(txtApPaterno, "Ingresar Apellido Paterno"); exito = false; } else{ errorDatos.SetError(txtApPaterno, ""); }
            if (txtApMaterno.Text == ""){ errorDatos.SetError(txtApMaterno, "Ingresar Apellido Materno"); exito = false; } else{ errorDatos.SetError(txtApMaterno, ""); }
            if ((int)cboClinica.SelectedValue == 0){ errorDatos.SetError(cboClinica, "Seleccione una clinica"); exito = false; } else{ errorDatos.SetError(cboClinica, ""); }
            if ((int)cboEspecialidad.SelectedValue == 0){ errorDatos.SetError(cboEspecialidad, "Seleccione una especialidad"); exito = false; } else{ errorDatos.SetError(cboEspecialidad, ""); }
            if (txtSueldo.Text == "") { errorDatos.SetError(txtSueldo, "Ingresar Sueldo"); exito = false; } else { errorDatos.SetError(txtSueldo, ""); }

            if (exito == false)
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            DoctorDAL DAL = new DoctorDAL();
            DoctorModel model = new DoctorModel();
            model.IdDoctor = int.Parse(txtId.Text);
            model.nombre = txtNombre.Text;
            model.ApPaterno = txtApPaterno.Text;
            model.ApMaterno = txtApMaterno.Text;
            model.IdClinica = (int) cboClinica.SelectedValue;
            model.IdEspecialidad = (int) cboEspecialidad.SelectedValue;
            model.Email = txtEmail.Text;
            model.celular = txtCelular.Text;
            model.BHABILITADO = 1;

            if (rbMasculino.Checked == true)
            {
                model.Sexo = 1;
            }
            else if (rbFemenino.Checked == true)
            {
                model.Sexo = 2;
            }

            model.Sueldo = decimal.Parse(txtSueldo.Text);

            if (dtpFechaContrato.Value == null)
            {
                model.FechaContrato = DateTime.Now;
            }
            else
            {
                model.FechaContrato = dtpFechaContrato.Value;
            }


            byte[] buffer;
            Image img = pbFoto.Image;
            if (img != null)
            {
                MemoryStream ms = new MemoryStream();
                img.Save(ms, img.RawFormat);
                buffer = ms.ToArray();
                string fotoBase64 = Convert.ToBase64String(buffer);
                string extension = Path.GetExtension(nombreArchivo).Substring(1);
                model.Archivo = "data:image" + extension + ";base64," + fotoBase64;
                model.nombreArchivo = nombreArchivo;
            }

            int rpta = await DAL.AddEditDoctor(model);
            if (rpta == 1)
            {
                MessageBox.Show("Se guardo correctamente");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Ocurrio un error");
                this.DialogResult = DialogResult.None;
            }
        }
    }
}
