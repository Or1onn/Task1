using System;
using System.Collections.Generic;

namespace PcComponents
{
    public partial class Accessory
    {
        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public int Year { get; set; }
    }
}
