using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static masivProject.Models.RouletteEnum;

namespace masivProject.Models
{
    public class Roulette
    {
        public Guid Id { get; set; }
        public List<Bet> RouletteBets { get; set; }
        public RouletteStatus Status { get; set; }
      

    }
}