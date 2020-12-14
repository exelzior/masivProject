using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace masivProject.Models
{
    public class RouletteBusinessLogic
    {
        ObjectCache cache = MemoryCache.Default;
        public async Task<List<Roulette>> GetRoulettes() {
            List<Roulette> cachedRoulettes = (List<Roulette>)cache.Get("ListRoulettes");
            if (cachedRoulettes == null)
                cachedRoulettes = new List<Roulette>();
            return cachedRoulettes;
        }

        public async void CreateRoulette(Roulette roulette) {
            List<Roulette> cachedRoulettes = (List<Roulette>)cache.Get("ListRoulettes");
            if (cachedRoulettes == null)
                cachedRoulettes = new List<Roulette>();
            cachedRoulettes.Add(roulette);
            cache.Remove("ListRoulettes");
            cache.Set("ListRoulettes", cachedRoulettes, DateTimeOffset.UtcNow.AddDays(1));
        }
        public async void NewBet(Roulette roulette)
        {
            List<Roulette> cachedRoulettes = (List<Roulette>)cache.Get("ListRoulettes");
            if (cachedRoulettes == null)
                cachedRoulettes = new List<Roulette>();
            if (cachedRoulettes.Any(a => a.Id == roulette.Id))
                cachedRoulettes.Remove(cachedRoulettes.FirstOrDefault(a => a.Id == roulette.Id));
            cachedRoulettes.Add(roulette);
            cache.Remove("ListRoulettes");
            cache.Set("ListRoulettes", cachedRoulettes, DateTimeOffset.UtcNow.AddDays(1));
        }
    }
}