using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static masivProject.Models.BetStatusEnum;

namespace masivProject.Models
{
    public class Bet
    {
        public int IdUsuario { get; set; }
        public int Number { get; set; }
        public string Color { get; set; }
        public WinningStatus WinningStatus { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WinningAmount { get; set; }
    }
}