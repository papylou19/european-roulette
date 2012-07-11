using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [Table("Identities")]
    public class Identities
    {
        [Key]
        public string Name { get; set; }
        public long Value { get; set; }
    }
}
