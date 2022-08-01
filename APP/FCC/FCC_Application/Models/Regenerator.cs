using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCC_Application.Models
{
    public class Regenerator
    {
        public double h { get; set; }
        public double D { get; set; }
        public int id { get; set; }
        public Regenerator(double h, double D, int id)
        {
            this.h = h;
            this.D = D;
            this.id = id;
        }
    }
}
