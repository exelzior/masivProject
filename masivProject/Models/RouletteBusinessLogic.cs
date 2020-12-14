using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace masivProject.Models
{
    public class RouletteBusinessLogic
    {
        ObjectCache cache = MemoryCache.Default;
        public async Task<List<Roulette>> GetRoulettesFromCache() {
            List<Roulette> cachedRoulettes = (List<Roulette>)cache.Get("ListRoulettes");
            if (cachedRoulettes == null)
                cachedRoulettes = new List<Roulette>();
            return cachedRoulettes;
        }
        public async Task<List<Bet>> GetBetsFromRoulette(Roulette roulette)
        {
            List<Roulette> cachedRoulettes = await GetRoulettesFromCache();
            return cachedRoulettes.FirstOrDefault(a => a.Id == roulette.Id).RouletteBets;
        } 
        public async void RefreshRoulettesCache(List<Roulette> roulettes)
        {
            cache.Remove("ListRoulettes");
            cache.Set("ListRoulettes", roulettes, DateTimeOffset.UtcNow.AddDays(1));
        }
        public async Task<JsonHttpStatusResult> CreateNewRoulette() {
            try
            {
                List<Roulette> cachedRoulettes = await GetRoulettesFromCache();
                Guid newRouletteId = Guid.NewGuid();
                cachedRoulettes.Add(new Roulette
                {
                    Id = newRouletteId,
                    Status = RouletteEnum.RouletteStatus.Created
                });
                RefreshRoulettesCache(cachedRoulettes);
                return new JsonHttpStatusResult(newRouletteId, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new JsonHttpStatusResult(ex.Message, HttpStatusCode.NotAcceptable);
            }
        }
        public async Task<JsonHttpStatusResult> OpenRouletteOperation(Roulette roulette)
        {
            try
            {
                List<Roulette> cachedRoulettes = await GetRoulettesFromCache();
                Parallel.ForEach(cachedRoulettes, itemRoulette => {
                    if (itemRoulette.Id == roulette.Id)
                        itemRoulette.Status = RouletteEnum.RouletteStatus.Open;
                });
                RefreshRoulettesCache(cachedRoulettes);
                return new JsonHttpStatusResult("Ruleta Abierta", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new JsonHttpStatusResult(ex.Message, HttpStatusCode.NotAcceptable);
            }
        }
        public async Task<JsonHttpStatusResult> CloseRouletteOperation(Roulette endingRoulette)
        {
            try
            {
                List<Roulette> cachedRoulettes = await GetRoulettesFromCache();
                CloseRouletteList closeRouletteList = new CloseRouletteList();
                closeRouletteList.RouletteId = endingRoulette.Id;
                var random = new Random();
                var winningCombinations = WinningCombination();
                int winningNumber = random.Next(WinningCombination().Count);
                string winningColor = winningCombinations[winningNumber];
                Parallel.ForEach(cachedRoulettes.Where(a => a.Id == endingRoulette.Id), closeRoulette => {
                    closeRoulette.Status = RouletteEnum.RouletteStatus.Closed;
                    Parallel.ForEach(closeRoulette.RouletteBets, item => { item.WinningStatus = BetStatusEnum.WinningStatus.Lose; });
                    Parallel.ForEach(closeRoulette.RouletteBets.Where(a => a.Number == winningNumber).ToList(), item =>
                    {
                        item.WinningAmount = item.BetAmount * 5;
                        item.WinningStatus = BetStatusEnum.WinningStatus.Win;
                        closeRouletteList.Winners.Add(new Winner
                        {
                            IdUser = item.IdUsuario,
                            BetAmmount = item.BetAmount,
                            WinAmmount = item.WinningAmount
                        });
                    });
                    Parallel.ForEach(closeRoulette.RouletteBets.Where(a => a.Color == winningColor).ToList(), item =>
                    {
                        item.WinningAmount = item.BetAmount * (decimal)1.8;
                        item.WinningStatus = BetStatusEnum.WinningStatus.Win;
                        closeRouletteList.Winners.Add(new Winner
                        {
                            IdUser = item.IdUsuario,
                            BetAmmount = item.BetAmount,
                            WinAmmount = item.WinningAmount
                        });
                    });
                });
                RefreshRoulettesCache(cachedRoulettes);
                return new JsonHttpStatusResult(closeRouletteList, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new JsonHttpStatusResult(ex.Message, HttpStatusCode.NotAcceptable); 
            }
        }
        public async Task<JsonHttpStatusResult> NewBet(IncomeBet incomeBet)
        {
            try
            {
                if(incomeBet.NewBet.BetAmount > 10000)
                    return new JsonHttpStatusResult(ConfigurationManager.AppSettings["AmmountExceededMessage"], HttpStatusCode.NotAcceptable);

                List<Roulette> cachedRoulettes = await GetRoulettesFromCache();
                Parallel.ForEach(cachedRoulettes, roulette => {
                    if (roulette.Id == incomeBet.RouletteId)
                        roulette.RouletteBets.Add(incomeBet.NewBet);
                });
                RefreshRoulettesCache(cachedRoulettes);
                return new JsonHttpStatusResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new JsonHttpStatusResult(ex.Message, HttpStatusCode.NotAcceptable);
            }
        }
        private Dictionary<int, string> WinningCombination() {
            Dictionary<int, string> combination = new Dictionary<int, string>();
            for (int i = 0; i < 37; i++)
            {
                if (i % 2 == 0)
                    combination.Add(i,"ROJO");
                else
                    combination.Add(i,"NEGRO");
            }
            return combination;
        }
    }
}