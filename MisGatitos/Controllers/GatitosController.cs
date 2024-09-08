using System.Net.Http;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using MisGatitos.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace MisGatitos.Controllers
{
    public class GatitosController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _uriApi;

        public GatitosController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("x-api-key", "live_Px4R2oxR6FRVibSRNV9SOXSkcYbpJpOj5sayes4j48KnZtu5t5aEExMXuOYIhcE6");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _uriApi = ConfigurationManager.AppSettings["CatApi"];
            _client.BaseAddress = new Uri(_uriApi);
        }

        [HttpGet]
        public ActionResult Index(string CatBreeds, int? limit)
        {
            IEnumerable<Gato> gatos = GetGatos();
            IEnumerable<Resultado> resultados = new List<Resultado>();

            if (limit != null)
            {
                resultados = GetResultados(CatBreeds, limit);
            }

            ViewBag.CatBreeds = new SelectList(gatos, "id", "name");

            return View();
        }

        public IEnumerable<Gato> GetGatos()
        {

            IEnumerable<Gato> gatos = new List<Gato>();
            var response = _client.GetAsync("v1/breeds").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                gatos = JsonConvert.DeserializeObject<IEnumerable<Gato>>(responseContent);
            }

            return gatos;
        }

        public IEnumerable<Resultado> GetResultados(string catBreeds, int? limit)
        {
            IEnumerable<Resultado> resultados = new List<Resultado>();
            var response = _client.GetAsync($"v1/images/search?breed_ids={catBreeds}&limit={limit}").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                resultados = JsonConvert.DeserializeObject<IEnumerable<Resultado>>(responseContent);
            }

            return resultados;
        }


    }
}