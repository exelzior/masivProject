using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace masivProject.Models
{
    public class IncomeBet
    {
        public Guid RouletteId { get; set; }
        public Bet NewBet { get; set; }
    }
}