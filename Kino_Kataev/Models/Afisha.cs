using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino_Kataev.Models
{
    public class Afisha
    {
        public int id { get; set; }
        public int id_kinoteatr { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }
        public int price { get; set; }

        public Afisha(int id, int id_kinoteatr, string name, DateTime time, int price)
        {
            this.id = id;
            this.id_kinoteatr = id_kinoteatr;
            this.name = name;
            this.time = time;
            this.price = price;
        }
    }
}
