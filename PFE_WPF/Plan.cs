using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE_WPF
{
    class Plan
    {
        public String camera { get; set; }
        public String cardSd { get; set; }
        public String decor { get; set; }
        public String description { get; set; }
        public String dialogue { get; set; }
        public String distance { get; set; }
        public String effetIN { get; set; }
        public String effetJN { get; set; }
        public String etat { get; set; }
        public String hauteur { get; set; }
        public String objectif { get; set; }
        public String plan { get; set; }
        public String seq { get; set; }
        public String sonOption { get; set; }
        public String urlImageLink { get; set; }
        public List<Prise> listPrise;
    }
}
