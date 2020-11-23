using ConsumeApiMedicoFromWindowsForms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsumeApiMedicoFromWindowsForms.CapaDatos
{
    public class DoctorDAL
    {

        public async Task<List<DoctorModel>> listarDoctor()
        {
            string rpta = "";
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.221:8081/Api/Doctor";
            HttpResponseMessage response = await client.GetAsync(url);
            List<DoctorModel> listModel = new List<DoctorModel>();

            if (response != null)
            {
                rpta = await response.Content.ReadAsStringAsync();
                listModel = JsonConvert.DeserializeObject<List<DoctorModel>>(rpta);
            }

            return listModel;
         }


        public async Task<List<EspecialidadModel>> listarEspecialidad()
        {
            string rpta = "";
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.221:8081/Api/Especialidad";
            HttpResponseMessage response = await client.GetAsync(url);
            List<EspecialidadModel> listModel = new List<EspecialidadModel>();

            if (response != null)
            {
                rpta = await response.Content.ReadAsStringAsync();
                listModel = JsonConvert.DeserializeObject<List<EspecialidadModel>>(rpta);
            }

            return listModel;
        }

        public async Task<List<ClinicaModel>> listarClinica()
        {
            string rpta = "";
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.221:8081/Api/Clinica";
            HttpResponseMessage response = await client.GetAsync(url);
            List<ClinicaModel> listModel = new List<ClinicaModel>();

            if (response != null)
            {
                rpta = await response.Content.ReadAsStringAsync();
                listModel = JsonConvert.DeserializeObject<List<ClinicaModel>>(rpta);
            }

            return listModel;
        }


        public async Task<DoctorModel> recuperarDoctor(int idDoc)
        {
            string rpta = "";
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.221:8081/Api/Doctor/?IdDoctor=" + idDoc;
            HttpResponseMessage response = await client.GetAsync(url);

            DoctorModel model = new DoctorModel();
            if (response != null)
            {
                rpta = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<DoctorModel>(rpta);
            }

            return model;
        }


        public async Task<int> eliminarDoctor(int idDoctor)
        {
            int rpta = 0;
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.221:8081/Api/Doctor/?IdDoctor=" + idDoctor;
            DoctorModel model = new DoctorModel
            {
                IdDoctor = idDoctor
            };
            var jsonRequest = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            if (response != null)
            {
                string rptaCadena = await response.Content.ReadAsStringAsync();
                rpta = int.Parse(rptaCadena);
            }

            return rpta;
        }

    }
}
