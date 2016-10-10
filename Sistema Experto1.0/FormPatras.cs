using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_Experto1._0
{
    public partial class FormPatras : Form
    {
        private List<Atomo> listonon;
        private List<Regla> listRules;
        Form1 formato = new Form1();

        List<int> Antecedentes = new List<int>();
        List<int> ConclusionesINT = new List<int>();
        List<int> ConclusionesFIN = new List<int>();
        List<int> NEWAntecedentes = new List<int>();
        List<int> NEWConclusionesINT = new List<int>();
        List<int> NEWConclusionesFIN = new List<int>();

        public FormPatras(List<Regla> listRules,List<Atomo> listonon)
        {
            InitializeComponent();
            this.listRules = listRules;
            this.listonon = listonon;
        }
        public void llenarListBox()
        {
            string reglaOBJ = "";
            string ConclOBJ = "";
            foreach (Regla Regla in listRules)
            {
                reglaOBJ = Regla.Reglax;
                ConclOBJ = Regla.Conclusion;
                string[] atomosRULE = reglaOBJ.Split('^');
                //Asigna Antecedentes
                foreach (string atomR in atomosRULE) { if (!Antecedentes.Contains(Convert.ToInt16(atomR))) Antecedentes.Add(Convert.ToInt16(atomR)); }
                //Asigna Consecuentes
                if (!ConclusionesFIN.Contains(Convert.ToInt16(ConclOBJ))) ConclusionesFIN.Add(Convert.ToInt16(ConclOBJ));
                //Interseccion de antecedentes y consecuentes para Asignar Intermedios
                foreach (int atomR in Antecedentes) { if (!ConclusionesINT.Contains(atomR)) { if (ConclusionesFIN.Contains(atomR)) { ConclusionesINT.Add(Convert.ToInt16(atomR)); } } }
                //Retira Intermedios de antecedentes y Consecuentes
                foreach (int atomR in ConclusionesINT) { Antecedentes.Remove(atomR); ConclusionesFIN.Remove(atomR); }
            }
            ConclusionesINT.Sort();
            ConclusionesFIN.Sort();
            Antecedentes.Sort();

            string item = "";
            foreach(int final in ConclusionesFIN)
            {
                item = formato.buscarAtomo(final);
                LSTBfinales.Items.Add(item);
            }
        }
        private void BTNseleccionar_Click(object sender, EventArgs e)
        {
            if (LSTBfinales.SelectedItem!=null)
                EncAtras(formato.reverseBuscarAtomo((string)LSTBfinales.SelectedItem));
        }
        private void EncAtras(int v)
        {
            NEWConclusionesFIN.Add(v);
            buscarListas(v);
            NEWAntecedentes.Sort();
            NEWConclusionesINT.Sort();
            NEWConclusionesFIN.Sort();
            formato.algoritmoEncAdelante(NEWAntecedentes, NEWConclusionesINT, NEWConclusionesFIN);
            this.Close();
        }
        private void buscarListas(int v)
        {
            foreach (Regla regla in listRules){
                if (Convert.ToInt16(regla.Conclusion) == v){
                    string[] atomosRegla = regla.Reglax.Split('^');
                    foreach (string atomo in atomosRegla){
                        foreach (int atomoANT in Antecedentes){
                            if (Convert.ToInt16(atomo) == atomoANT){
                                if(!NEWAntecedentes.Contains(Convert.ToInt16(atomo)))
                                NEWAntecedentes.Add(Convert.ToInt16(atomo));
                            }
                        }
                        foreach (int atomoint in ConclusionesINT){
                            if (Convert.ToInt16(atomo) == atomoint){
                                if (!NEWConclusionesINT.Contains(Convert.ToInt16(atomo))){
                                    NEWConclusionesINT.Add(Convert.ToInt16(atomo));
                                    buscarListas(Convert.ToInt16(atomo));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
