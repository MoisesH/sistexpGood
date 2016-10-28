using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Experto1._0
{
    class ReglaNormNum
    {
        public ReglaNormNum(int IdReglax, List<int> ReglaX)
        {
            this.IdRegla = IdReglax;
            this.Reglax = ReglaX;
        }
        public int IdRegla { get; set; }
        public List<int> Reglax { get; set; }
    }
}
