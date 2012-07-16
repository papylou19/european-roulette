﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [Table("Game")]
    public class Game
    {
        public int Id { get; set; }
        public byte? Number { get; set; }
    }
}
