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

    }
}
