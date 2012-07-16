using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameState
    {
        [Key]
        public int Id { get; set; }
        public short State { get; set; }
        public DateTime StartTime { get; set; }
    }
}
