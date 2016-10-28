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
    public partial class FormObjetivos : Form
    {
        private List<Atomo> listonon;
        private List<Regla> listRules;
        private Form1 formato;

        List<int> Antecedentes = new List<int>();
        List<int> ConclusionesINT = new List<int>();
        List<int> ConclusionesFIN = new List<int>();
        List<int> NEWAntecedentes = new List<int>();
        List<int> NEWConclusionesINT = new List<int>();
        List<int> NEWConclusionesFIN = new List<int>();

        public FormObjetivos(List<Atomo> listonon, Form1 formato)
        {
            InitializeComponent();
            this.listonon = listonon;
            this.formato = formato;
        }

        private void BTNSeleccionar_Click(object sender, EventArgs e)
        {
            if (LSTBAtomos.SelectedItem != null)
                formato.algoritmoObjetivos(formato.reverseBuscarAtomo((string)LSTBAtomos.SelectedItem));
            this.Close();
            

        }
        public void llenarListBox()
        {
            foreach (Atomo atomox in listonon)
            {
                LSTBAtomos.Items.Add(formato.buscarAtomo(Convert.ToInt16(atomox.IdAtomo)));
            }
        }


    }
}


