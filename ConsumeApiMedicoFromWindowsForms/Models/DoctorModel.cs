using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumeApiMedicoFromWindowsForms.Models
{
    public class DoctorModel
    {
        //Listado
        public int IdDoctor { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreClinica { get; set; }
        public string NombreEspecialidad { get; set; }
        public string Email { get; set; }
        public DateTime FechaContrato { get; set; } 


        //Agregar o Editar
        public string nombre { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Foto { get; set; }
        public string nombreArchivo { get; set; }
        public int IdClinica { get; set; }
        public int IdEspecialidad { get; set; }
        public int IdSexo { get; set; }
        public decimal Sueldo { get; set; }
        public string TelefonoCelular { get; set; }
    }
}
