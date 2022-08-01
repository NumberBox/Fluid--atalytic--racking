using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCC_Application.Models
{
    public class Reactor
    {
        public double h { get; set; }
        public double D { get; set; }
        public double perfomance { get; set; }
        public int id { get; set; }
        public Reactor(double h,double D,int id,double perfomance) {
            this.h = h;
            this.D = D;
            this.id = id;
            this.perfomance = perfomance;
        }
        
    }
}
