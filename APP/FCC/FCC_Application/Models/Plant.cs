using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCC_Application.Models
{
    public class Plant
    {
        public Reactor reactor { get; set; }
        public Regenerator regenerator { get; set; }
        public double powerUsage { get; set; }
        public  Dictionary<int, int> equipment{ get; set; }
        public Plant()
        {
            powerUsage = 0;
            equipment = new Dictionary<int, int>();
        }

        public int ReturnDatabaseIdByMountIndex(int index) {
            int db_id;
            equipment.TryGetValue(index, out db_id);
            return db_id;
        }
        public double IncreaseUsage(double usage_value) {
            powerUsage += usage_value;
            return powerUsage;
        }
        public double DecreaseUsage(double usage_value) {
            powerUsage -= usage_value;
            return powerUsage;
        }
    }
}
