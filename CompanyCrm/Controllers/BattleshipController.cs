using CompanyCrm.Models;
using CustomerModels.Battleship;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CompanyCrm.Controllers
{
    public class BattleshipController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BattleshipController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("Setup", new BattleshipViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Setup(BattleshipViewModel model)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

            for (int i = 0; i < model.UserFleet.Count; i++)
            {
                model.UserFleet[i].Coordinates = model.CoordinateLists[i];
            }

            List<Ship> combinedFleet = model.UserFleet;
            combinedFleet.AddRange(BattleshipLogic.PlaceShips());
            Game game = new Game { Ships = combinedFleet };

            string gameString = JsonConvert.SerializeObject(game);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "battleship");
            request.Content = new StringContent(gameString);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Game savedGame = JsonConvert.DeserializeObject<Game>(content);

            return View("Play", savedGame);
        }
    }
}
