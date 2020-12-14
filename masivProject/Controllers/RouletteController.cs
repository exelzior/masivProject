using masivProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace masivProject.Controllers
{
    public class RouletteController : ApiController
    {
        readonly RouletteBusinessLogic  rouletteLogic = new RouletteBusinessLogic();
        [HttpGet]
        [ActionName("ListCurrentRoulettes")]
        public async Task<List<Roulette>> ListCurrentRoulettes()
        {
            return await rouletteLogic.GetRoulettesFromCache();
        }
        [HttpGet]
        [ActionName("NewRouletteCreation")]
        public async Task<JsonHttpStatusResult>  NewRouletteCreation()
        {
            return await rouletteLogic.CreateNewRoulette();
        }
        [HttpPost]
        [ActionName("OpenRouletteOperation")]
        public async Task<JsonHttpStatusResult> OpenRouletteOperation([FromBody] Roulette rouletteSearch)
        {
            return await rouletteLogic.OpenRouletteOperation(rouletteSearch);
        }
        [HttpPost]
        [ActionName("NewBetRegistration")]
        public async Task<JsonHttpStatusResult> NewBetRegistration([FromBody] IncomeBet betRequest, [FromUri] int idUser)
        {
            return await rouletteLogic.NewBet(betRequest);
        }
        [HttpPost]
        [ActionName("CloseRouletteOperation")]
        public async Task<JsonHttpStatusResult> CloseRouletteOperation([FromBody] Roulette rouletteSearch)
        {
            return await rouletteLogic.CloseRouletteOperation(rouletteSearch);
        }
    }
}
