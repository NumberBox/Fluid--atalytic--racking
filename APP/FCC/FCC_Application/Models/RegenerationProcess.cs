using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCC_Application.Models
{
    public static class RegenerationProcess
    {
        public static Regenerator regenerator{ get; set; }
        public static double k { get; set; }
        public static double W { get; set; }
        public static double Wa { get; set; }
        public static double Mc { get; set; }
        public static double n { get; set; }
        public static double sigma { get; set; }
        public static double Fs { get; set; }
        public static double Fa { get; set; }
        public static double Ma { get; set; }
        public static double yin { get; set; }
        public static double Cps { get; set; }
        public static double tin { get; set; }
        public static double dH { get; set; }
        public static double Cpa { get; set; }
        public static double ta { get; set;}
        public static double Calculate(double Crin) {
            double a1 = Fa / (Wa * Ma);
            double b1 = (((1 + sigma) * n + 2 + 4 * sigma) * k * W) / (4*(1 + sigma) * Wa * Mc);
            double A = b1 * Fs;
            double B = Fs * a1 + (k * W) * yin * a1 - Fs * Crin * b1;
            double C = Fs * Crin * a1;
            double D = Math.Pow(B, 2) - 4 * A * C;
            return (-B + Math.Sqrt(D)) / (2 * A);
        }

        public static double GetOutTemperature(double cokeOut) {
            double a1 = Fa / (Wa * Ma);
            double b1 = (((1 + sigma) * n + 2 + 4 * sigma) * k * W) / (4 * (1 + sigma) * Wa * Mc);
            double y1 = a1 * yin / (a1 + b1 * cokeOut);
            return W*Cps*((tin*Fs/W)+(ta*Fa*Cpa/(W*Cps))-(dH*k*cokeOut*y1/(Mc*Cps)))/(Fs*Cps+Fa*Cpa);
        }
    }
}
