using DocumentFormat.OpenXml.Spreadsheet;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace FCC_Application.Models
{
    public static class CrackingProcess
    {
        public static double Cps { get; set; }
        public static Reactor reactor { get; set; }
        public static double cf { get; set; }
        public static double cp { get; set; }
        public static double ro { get; set; }
        public static double Eg { get; set; }
        public static double Ef { get; set; }
        public static double Hf { get; set; }
        public static double alpha { get; set; }
        public static double lambda { get; set; }
        public static double k0f { get; set; }
        public static double k0g { get; set; }
        public static double delta { get; set; }
        public static double m_factor { get; set; }
        public static double T0 { get; set; }
        public static double Fs { get; set; }
        public static double Ff { get; set; }
        public static double kc { get; set; }
        public static double Ecf { get; set; }
        public static double N { get; set; }
        public static double k03 { get; set; }
        public static double Crc { get; set; }
        public static List<DataPoint> temperature;
        public static List<DataPoint> concentration;
        public static List<DataPoint> g_concentration;
        public static List<double> yg;
        public static List<double> t;
        public static List<double> gasoile_c;
        public static XLWorkbook workbook;


        private static double phi0, u, K1, K2, Rco, phi, gasoile, a2;

        public static double Calculate(double step) {
            delta = step;
            gasoile = 1;
            a2 = k03/k0f;
            temperature = new List<DataPoint>();
            concentration = new List<DataPoint>();
            g_concentration = new List<DataPoint>();
            yg = new List<double>();
            t = new List<double>();
            gasoile_c = new List<double>();
            phi0 = 1 - CrackingProcess.m_factor * CrackingProcess.Crc;
            u = 4 * CrackingProcess.Ff / (Math.PI * Math.Pow(CrackingProcess.reactor.D, 2) * CrackingProcess.ro);
            Rco = CrackingProcess.Fs / CrackingProcess.Ff;
            temperature.Add(new DataPoint(0, CrackingProcess.T0));
            concentration.Add(new DataPoint(0,0));
            g_concentration.Add(new DataPoint(0, 1));
            t.Add(T0);
            yg.Add(0);
            gasoile_c.Add(gasoile);
            for (double z = CrackingProcess.delta; Math.Round(z, GetDecimalDigitsCount(CrackingProcess.delta)) <
                CrackingProcess.reactor.h; z += CrackingProcess.delta)
            {
                K1 = CrackingProcess.k0f * Math.Pow(Math.E, (-CrackingProcess.Ef / (8.31 * t[t.Count - 1])));
                K2 = CrackingProcess.k0g * Math.Pow(Math.E, (-CrackingProcess.Eg / (8.31 * t[t.Count - 1])));
                phi = phi0 * Math.Exp(-CrackingProcess.alpha * z / u);
                t.Add(RungeKutt(heat_balance, t[t.Count - 1]));
                temperature.Add(new DataPoint(z, t[t.Count - 1]));
                gasoile = RungeKutt(gas_balance, gasoile);
                gasoile_c.Add(gasoile);
                g_concentration.Add(new DataPoint(z, gasoile));
                yg.Add(RungeKutt(gasoline_balance, yg[yg.Count - 1]));
                concentration.Add(new DataPoint(z,yg[yg.Count-1]));
            }
            phi = phi0 * Math.Exp(-CrackingProcess.alpha * CrackingProcess.reactor.h / u);
            t.Add(RungeKutt(heat_balance, t[t.Count - 1]));
            temperature.Add(new DataPoint(reactor.h,t[t.Count - 1]));
            gasoile = RungeKutt(gas_balance, gasoile);
            gasoile_c.Add(gasoile);
            g_concentration.Add(new DataPoint(reactor.h, gasoile));
            yg.Add(RungeKutt(gasoline_balance, yg[yg.Count - 1]));
            concentration.Add(new DataPoint(reactor.h,yg[yg.Count - 1]));
            double Csc = CrackingProcess.Crc + CrackingProcess.kc * Math.Sqrt((CrackingProcess.reactor.h / 
                (u * Math.Pow(CrackingProcess.Crc, CrackingProcess.N)) * Math.Exp(-CrackingProcess.Ecf / (8.31 * t[t.Count - 1]))));
            return Csc;
        }

        private static int GetDecimalDigitsCount(double value)
        {
            string[] str = value.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." }).Split('.');
            return str.Length == 2 ? str[1].Length : 0;
        }

        private delegate double KineticFunc(double x);

        private static double gas_balance(double last)
        {
            return (-K1 * Math.Pow(last, 2) * Rco * phi) / u;
        }
        private static double gasoline_balance(double last)
        {
            return ((a2*K1* Math.Pow(gasoile, 2) - K2 * last) * Rco * phi) / u;
        }
        private static double heat_balance(double last)
        {
            return Hf * Ff * ((-K1 * Math.Pow(gasoile, 2) * Rco * phi) / u) / 
                (Fs * Cps + CrackingProcess.Ff * CrackingProcess.cf + CrackingProcess.lambda * CrackingProcess.Ff * CrackingProcess.cp);
        }

        private static double RungeKutt(KineticFunc kineticFunc, double last)
        {
            double k1, k2, k3, k4;
            k1 = kineticFunc(last);
            k2 = kineticFunc(last + (CrackingProcess.delta * k1) / 2);
            k3 = kineticFunc(last + (CrackingProcess.delta * k2) / 2);
            k4 = kineticFunc(last + CrackingProcess.delta * k3);
            last = last + (CrackingProcess.delta / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
            return last;
        }
    }
}
