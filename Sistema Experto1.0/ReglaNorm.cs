using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Experto1._0
{
    class ReglaNorm
    {
        public ReglaNorm(int IdReglax, string ReglaX, string NegadosX)
        {
            this.IdRegla = IdReglax;
            this.Reglax = ReglaX;
            this.Negados = NegadosX;

        }

        public int IdRegla { get; set; }
        public string Reglax { get; set; }
        public string Negados { get; set; }


    }
}
