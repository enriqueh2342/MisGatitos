using System.Net.Http;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using MisGatitos.Models;
using Newtonsoft.Json;

namespace MisGatitos.Controllers
{
    public class GatitosController : Controller
    {
        private readonly HttpClient _client;

        public GatitosController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("x-api-key", "live_Px4R2oxR6FRVibSRNV9SOXSkcYbpJpOj5sayes4j48KnZtu5t5aEExMXuOYIhcE6");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = new Uri("https://api.thecatapi.com/");
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Gato> gatos = GetGatos();

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


    }
}