using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace masivProject.Models
{
    public class CloseRouletteList
    {
        public Guid RouletteId { get; set; }
        public List<Winner> Winners  { get; set; }
    }
}