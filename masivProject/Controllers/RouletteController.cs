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
        RouletteBusinessLogic rouletteLogic = new RouletteBusinessLogic();
        [HttpGet]
        public async Task<List<Roulette>> ListRoulettes()
        {
            return await rouletteLogic.GetRoulettes();
        }
        [HttpGet]
        public async Task<Guid>  NewRouletteCreation()
        {
            Roulette roulette = new Roulette
            {
                Id = Guid.NewGuid(), Status = RouletteEnum.RouletteStatus.Created
            };
            rouletteLogic.CreateRoulette(roulette);
            return roulette.Id;
        }
        [HttpPost]
        public async Task<bool> NewBetRegistration([FromBody] IncomeBet betRequest, [FromUri] int idUser)
        {
            List<Roulette> roulettes = await rouletteLogic.GetRoulettes();
            Roulette roulette = roulettes.FirstOrDefault(a => a.Id == betRequest.RouletteId);
            if(roulette.RouletteBets == null)
                roulette.RouletteBets = new List<Bet>();
            roulette.RouletteBets.Add(betRequest.NewBet);
            rouletteLogic.NewBet(roulette);
            return false;
        }

 
    }
}
