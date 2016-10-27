using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finisar.SQLite;

namespace Sistema_Experto1._0
{
    public partial class Form1 : Form
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        List<Atomo> listonon = new List<Atomo>();
        List<Regla> listRules = new List<Regla>();
        List<ReglaClon> listRulesClon = new List<ReglaClon>();
        List<ReglaClonNum> listRulesClonNum = new List<ReglaClonNum>();
        List<ReglaNorm> listRulesNorm = new List<ReglaNorm>();
        List<int[]> listaPorPreguntar = new List<int[]>();

        public Form1()
        {
            InitializeComponent();
            IniciarDB();
            IniciarDGVatomos();
            IniciarDGVrules();
            
        }
        public void IniciarDB()
        {
            sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=False;Compress=True;");
        }
        public void IniciarDGVatomos()
        {
            DGVatomos.Rows.Clear();
            listonon.Clear();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_conn.Open();
            sqlite_cmd.CommandText = "SELECT * FROM atomos";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read()){
                DGVatomos.Rows.Add(new Object[] {sqlite_datareader.GetValue(0), sqlite_datareader.GetValue(1)});
                listonon.Add(new Atomo(Convert.ToInt16(sqlite_datareader.GetValue(0).ToString()), sqlite_datareader.GetValue(1).ToString()));
            }
            sqlite_conn.Close();
        }
        public void IniciarDGVrules()
        {
            DGVrules.Rows.Clear();
            listRules.Clear();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_conn.Open();
            sqlite_cmd.CommandText = "SELECT * FROM reglas";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                listRules.Add(new Regla(Convert.ToInt16(sqlite_datareader.GetValue(0).ToString()), sqlite_datareader.GetValue(1).ToString(), sqlite_datareader.GetValue(2).ToString(), sqlite_datareader.GetValue(3).ToString(), sqlite_datareader.GetValue(4).ToString()));
                string ReglaDGV="";
                string ConclusionDGV = "";
                string Premisa = sqlite_datareader.GetValue(1).ToString();
                string PNegados = sqlite_datareader.GetValue(2).ToString();
                string Resultado = sqlite_datareader.GetValue(3).ToString();
                string RNegado = sqlite_datareader.GetValue(4).ToString();
                string[] APremisa = Premisa.Split('^');
                string[] APNegados = PNegados.Split(',');
                for (int i = 0; i < APremisa.Length; i++)
                {
                   if(i==0){
                       if(APNegados[i]=="-1")
                       {ReglaDGV = "¬" + APremisa[i];}
                       else
                       {ReglaDGV = APremisa[i];}
                   }
                   else{
                       if (APNegados[i] == "-1")
                       {ReglaDGV = ReglaDGV + "^" + "¬" + APremisa[i];}
                       else
                       {ReglaDGV = ReglaDGV + "^" + APremisa[i];}
                   }
                }
                if(RNegado=="-1")
                {ConclusionDGV = "¬" + Resultado;}
                else 
                {ConclusionDGV = Resultado;}
                DGVrules.Rows.Add(new Object[] { sqlite_datareader.GetValue(0), ReglaDGV, ConclusionDGV, Premisa });
            }
            sqlite_conn.Close();
            IniciarDGVreal();
        }
        public void IniciarDGVreal()
        {
            DGVreal.Rows.Clear();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_conn.Open();
            sqlite_cmd.CommandText = "SELECT * FROM reglas";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            string ReglaDGV = "";
            string ConclusionDGV = "";
            string atomoDGV = "";

            while (sqlite_datareader.Read())
            {
                string Premisa = sqlite_datareader.GetValue(1).ToString();
                string PNegados = sqlite_datareader.GetValue(2).ToString();
                string Resultado = sqlite_datareader.GetValue(3).ToString();
                string RNegado = sqlite_datareader.GetValue(4).ToString();
                //
                string[] APremisa = Premisa.Split('^');
                string[] APrueba = APremisa[0].Split('v');
                APremisa.Union(APrueba);
                //
                string[] APNegados = PNegados.Split(',');
                for (int i = 0; i < APremisa.Length; i++)
                {
                    foreach (Atomo atomo in listonon)
                    { if (Convert.ToInt16(APremisa[i]) == atomo.IdAtomo)
                        { atomoDGV = atomo.Atomox; } 
                    }
                    if (i == 0){
                        if (APNegados[i] == "-1")
                        { ReglaDGV = "Si no " + atomoDGV; }
                        else
                        { ReglaDGV = "Si " + atomoDGV; }
                    }
                    else{
                        if (i != APremisa.Length - 1){
                            if (APNegados[i] == "-1")
                            { ReglaDGV = ReglaDGV + ", " + "no " + atomoDGV; }
                            else
                            { ReglaDGV = ReglaDGV + ", " + atomoDGV; }
                        }
                        else{
                            if (APNegados[i] == "-1")
                            { ReglaDGV = ReglaDGV + " y " + "no " + atomoDGV; }
                            else
                            { ReglaDGV = ReglaDGV + " y " + atomoDGV; }
                        }
                    }
                }
                foreach (Atomo atomo in listonon)
                {
                    if (Convert.ToInt16(Resultado) == atomo.IdAtomo)
                    { atomoDGV = atomo.Atomox; }
                }
                if (RNegado == "-1")
                { ConclusionDGV = "Entonces no " + atomoDGV; }
                else
                { ConclusionDGV = "Entonces " + atomoDGV; }
                DGVreal.Rows.Add(new Object[] { sqlite_datareader.GetValue(0), ReglaDGV, ConclusionDGV });
            }
            sqlite_conn.Close();
        }
        private void TXTaddatomo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar==(int)Keys.Enter)
            {
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO atomos VALUES (NULL,'"+ TXTaddatomo.Text +"');";
                sqlite_cmd.ExecuteNonQuery();
                sqlite_conn.Close();
                IniciarDGVatomos();
                TXTaddatomo.Text = "";
            }
        }
        private void DGVatomos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGVatomos.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex != -1 && e.ColumnIndex == 5)
            {
                bool negado = false;
                bool and = false;
                bool or = false;
                string valor = LBLrule.Text;
                foreach (DataGridViewRow row in DGVatomos.Rows){   
                        negado = Convert.ToBoolean(row.Cells[Negado.Name].Value);
                        and = Convert.ToBoolean(row.Cells[AND.Name].Value);
                        or = Convert.ToBoolean(row.Cells[OR.Name].Value);
                        if(row.Index == e.RowIndex && !(and && or)){
                            valor = Convert.ToString(row.Cells[ID.Name].Value);
                            LBLrule.Text = LBLrule.Text + (negado ? "¬" : "") + valor + (and ? "^" : "") + (or ? "v" : "");
                        }
                        clearRow(row);
                    }
            }
        }
        private void clearRow(DataGridViewRow row)
        {
            row.Cells[Negado.Name].Value = false;
            row.Cells[AND.Name].Value = false;
            row.Cells[OR.Name].Value = false;
        }
        private void button1_Click(object sender, EventArgs e) { LBLrule.Text = LBLrule.Text + " >"; }
        private void BTNañadirRule_Click(object sender, EventArgs e)
        {
            string negados = "";
            string Antecedentes = "";
            string rule = LBLrule.Text;                             //Guarda en rule la Regla
            string[] rulex= rule.Split('>');                        //Separa la Regla en arreglo de rulex[0]=antecedentes y rulex[1]=Conclusion
            string antecedentes1 = rulex[0];                        //Guarda los antecedentes
            string conclusion = rulex[1];                           //Guarda la Conclusion
            string negado="";                                       
            string[] antecedentes2 = antecedentes1.Split('^');      //Separa los antecedentes en una lista de antecedentes

            foreach (string atomo in antecedentes2)                 //Cicla los antecedentes
            {
                if (atomo.Substring(0,1)=="¬"){                     //Retira el Negado y asigna -1 a negados(variable que asignara Negacion a atomo)
                    if(negados=="")
                    { negados = "-1"; }
                    else
                    { negados= negados + ",-1"; }
                }
                else{                                               //Si no tiene negado asigna un 1
                    if (negados == "")
                    { negados = "1"; }
                    else
                    { negados = negados + ",1"; }
                }
                if(Antecedentes=="")                                
                { Antecedentes = atomo.Trim(new Char[] { '¬', ' ' });  }                        //Retira caracteres invalidos a los antecedentes de atomo y agrega a antecedentes
                else
                { Antecedentes = Antecedentes + "^" + atomo.Trim(new Char[] { '¬', ' ' }); }    //Retira caracteres invalidos a los antecedentes de atomo y agrega a antecedntes
            }
            if (conclusion.Substring(0, 1) == "¬")                  //Retira el Negado y asigna -1 a negado(variable que asignara Negacion a atomo)
            { negado = "-1"; }
            else
            { negado = "1"; }
            conclusion.Trim(new Char[] { '¬', ' ' });               //Retira caracteres invalidos

            //añadir:
            //ID NULL
            //Antecedentes  String separado por "^" de atomos   DONE
            //negados       String separada por "," de -1 o 1   DONE
            //conclusion    String de atomo                     DONE
            //Negado        Sting de -1 o 1                     DONE 
            sqlite_conn.Open();                                     //Abre conexion con DB
            sqlite_cmd = sqlite_conn.CreateCommand();               //Crea Comando
            sqlite_cmd.CommandText = "INSERT INTO reglas VALUES (NULL,'" + Antecedentes + "','" + negados + "','" + conclusion.Trim(new Char[] { '¬', ' ' }) + "','" + negado + "');";
            sqlite_cmd.ExecuteNonQuery();                           //Ejecuta Comando
            sqlite_conn.Close();                                    //Cierra DB
            IniciarDGVrules();                                      //Carga Datagridview
            LBLrule.Text = "";                                      //Borra label
        }
        private void BTNresetRULE_Click(object sender, EventArgs e) { LBLrule.Text = ""; }
        public void BTNEncAdelante_Click(object sender, EventArgs e)
        {

            List<int> Antecedentes = new List<int>();
            List<int> ConclusionesINT = new List<int>();
            List<int> ConclusionesFIN = new List<int>();
            string reglaOBJ = "";
            string ConclOBJ = "";
            foreach (Regla Regla in listRules)
            {
                reglaOBJ = Regla.Reglax;
                ConclOBJ = Regla.Conclusion;
                string[] atomosRULE = reglaOBJ.Split('^');
                //Asigna Antecedentes
                foreach (string atomR in atomosRULE){if(!Antecedentes.Contains(Convert.ToInt16(atomR))) Antecedentes.Add(Convert.ToInt16(atomR));}
                //Asigna Consecuentes
                if (!ConclusionesFIN.Contains(Convert.ToInt16(ConclOBJ))) ConclusionesFIN.Add(Convert.ToInt16(ConclOBJ));
                //Interseccion de antecedentes y consecuentes para Asignar Intermedios
                foreach (int atomR in Antecedentes) { if (!ConclusionesINT.Contains(atomR)) { if (ConclusionesFIN.Contains(atomR)) {ConclusionesINT.Add(Convert.ToInt16(atomR)); } } }
                //Retira Intermedios de antecedentes y Consecuentes
                foreach(int atomR in ConclusionesINT) {  Antecedentes.Remove(atomR); ConclusionesFIN.Remove(atomR); }  
            }
            //ordena Conjuntos
            ConclusionesINT.Sort();
            ConclusionesFIN.Sort();
            Antecedentes.Sort();

            
            algoritmoEncAdelante(Antecedentes, ConclusionesINT, ConclusionesFIN);
            
        }
        public void algoritmoEncAdelante(List<int> Antecedentes, List<int> ConclusionesINT, List<int> ConclusionesFIN)
        {
            #region Prepara Listas y Variables
            //ClonaListas
            CloneListRules();
            CloneListRulesNum();
            string disparado = "";
            //Prepara Lista para preguntar
            List<int> ListaPreguntar = new List<int>();
            foreach (int x in Antecedentes) { ListaPreguntar.Add(x); }

            //*************************Inicializa Banderas
            int atomoVALUE = 0;
            int terminado = 0;
            int pason = 0;
            //***************************Saca Cuenta de la lista
            int exx = ListaPreguntar.Count();
            #endregion
            //Ciclara hasta que no dispare una Conclusion Final
            while (terminado != 1)
            {
                //Ciclara el tamaño de la listaPreguntar
                for (int ixx = 0; ixx < exx; ixx++)
                {
                    /// Asigna valor del atomo segun Usuario conteste
                    if (MessageBox.Show(    "¿" + buscarAtomo(Convert.ToInt16(ListaPreguntar[ixx])) +"?",
                                            "Importante Pregunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            { atomoVALUE = 1; }
                                            else { atomoVALUE = -1; }

                    //Manda Correr Algoritmo y si dispara algo manda el numero del atomo disparado
                    disparado = modificarReglas(ListaPreguntar[ixx], atomoVALUE);
                    //Si no disparo nada continua con el siguiente item en ListaPreguntar
                    while (disparado != "")
                    {
                        //Si dispara algo revisa si es Conclusion Final
                        foreach (int atomoFinal in ConclusionesFIN)
                        {
                            //Cicla en las conclusiones finales
                            if (atomoFinal == Convert.ToInt32(disparado))
                            {
                                //si es Final manda Algoritmo de explicacion
                                explicarTerminoFinal(atomoFinal, false);
                                //Rompe Ciclo

                                //Sale del Ciclo de Preguntas
                                if (MessageBox.Show("Llegaste a una Conclusion Final \n¿Desea Continuar?",
                                           "Continuar?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    terminado = 0;
                                }
                                else
                                {
                                    disparado = "";
                                    terminado = 1;
                                }
                                break;
                            }
                        }
                        //Si termino Rompe Ciclo de preguntas
                        if (terminado == 1) break;
                        //SI NO termino se asume que es una Conclusion Intermedia y Comienza removiendola de la lista 

                        ConclusionesINT.Remove(Convert.ToInt32(disparado));
                        //MessageBox.Show("llegaste a una Media:\n" + buscarAtomo(Convert.ToInt16(disparado)));
                        //Manda correr algoritmo y si dispara algo manda el numero del atomo disparado
                        disparado = modificarReglas(Convert.ToInt32(disparado), 1);

                    }
                    //Si ya termina
                    if (terminado == 1) break;

                }
                //Pregunta si ya activo la bandera de haber pasado una vez el ciclo, si ya se activo rompe el ciclo
                if (pason >= 1) break;
                //Limpia la Lista
                ListaPreguntar.Clear();
                //Agrega a la lista por preguntar a todas las conclusiones intermedias
                ListaPreguntar.AddRange(ConclusionesINT);
                //Aumenta el tamaño del For
                exx = ListaPreguntar.Count();
                //Aumenta la bandera indicando que ya paso 1 vez
                pason++;
            }
        }

        private void explicarTerminoFinal(int atom, bool BandInt)
        {
            int numRegla = 100;
            string buscado = "";
            List<int> reglaAuxiliar = new List<int>();
            numRegla = buscarRegla(atom, false);
            buscado = buscarAtomo(atom);
            if (!BandInt)
            {
                if (Convert.ToInt16(listRulesClon[numRegla - 1].Negado) == -1)
                { MessageBox.Show("No " + buscado + "\n\nPorque:"); }
                else if (Convert.ToInt16(listRulesClon[numRegla - 1].Negado) == 1)
                { MessageBox.Show(buscado + "\n\nPorque:"); }
            }
            string[] regla = (listRules[numRegla - 1].Reglax.Split('^'));
            foreach (string atomoregla in regla)
            {
                int numRegla1 = buscarRegla(Convert.ToInt16(atomoregla), true);
                if (numRegla1 != 100)
                {
                    reglaAuxiliar.Add(Convert.ToInt32(atomoregla));
                }
                else
                {
                    int suma = 0;
                    int sumax = 0;

                    sumax = listRulesClonNum[numRegla - 1].Reglax.Count();
                    foreach (int ix in listRulesClonNum[numRegla - 1].Reglax)
                    {
                        suma = ix + suma;
                    }
                    if (sumax != suma)
                    {
                        MessageBox.Show("Tu definiste la Conclusion Intermedia");
                        reglaAuxiliar.Clear();
                    }
                    else
                    {
                        buscado = buscarAtomo(Convert.ToInt32(atomoregla));
                        if (buscarSignoAtomo(Convert.ToInt16(atomoregla), (numRegla - 1)) == -1)
                        {
                            MessageBox.Show("Definiste que:\n\nNo " + buscado);
                        }
                        else if (buscarSignoAtomo(Convert.ToInt16(atomoregla), (numRegla - 1)) == 1)
                        {
                            MessageBox.Show("Definiste que:\n\n" + buscado);
                        }
                    }

                    

                }
            }

            foreach(int atomoregla in reglaAuxiliar)
            {
                buscado = buscarAtomo(Convert.ToInt32(atomoregla));
                if (buscarSignoAtomo(Convert.ToInt16(atomoregla), (numRegla - 1)) == -1)
                {
                    MessageBox.Show("Y se dedujo que:\n\nNo "+ buscado +"\n\nMediante una Regla Intermedia\n que se dedujo porque:");
                }
                else if (buscarSignoAtomo(Convert.ToInt16(atomoregla), (numRegla - 1)) == 1)
                {
                    MessageBox.Show("Y se dedujo que:\n\n " + buscado + "\n\nMediante una Regla Intermedia\n que se dedujo porque:");
                }
                explicarTerminoFinal(Convert.ToInt16(atomoregla), true);
            }
        }

        private int buscarSignoAtomo(int atomoABuscar, int NumRegla)
        {
            int signo = 0;
            string[] reglaseparada;
            string[] signosSeparados;
            signosSeparados = listRulesClon[NumRegla].Negados.Split(',');
            reglaseparada = listRulesClon[NumRegla].Reglax.Split('^');

            for (int i = 0; i<= listRulesClonNum[NumRegla].Reglax.Count();i++)
            {
                
                if (atomoABuscar== Convert.ToInt16(reglaseparada[i]))
                {
                    signo = Convert.ToInt16(listRulesClonNum[NumRegla].Reglax[i]) * Convert.ToInt16(signosSeparados[i]);
                    return signo;
                }

            }
            return 100;
        }
        private int buscarRegla(int atom, bool neg)
        {
            for (int x = 0; x < listRules.Count(); x++)
            {

                if (atom == Convert.ToInt32(listRules[x].Conclusion))
                {
                    if (neg)
                    {
                        if (listRulesClonNum[x].Conclusion != 0)
                        {
                            return listRules[x].IdRegla;
                        }
                    }
                    else
                    {
                        if (listRulesClonNum[x].Conclusion != 0 )
                        {
                            return listRules[x].IdRegla;
                        }
                    }
                }
            }
            return 100;
        }
        public int buscarRegla(int atom)
        {
            for (int x = 0; x < listRules.Count() - 1; x++)
            {
                if (atom == Convert.ToInt32(listRules[x].Conclusion))
                {
                    return listRules[x].IdRegla;
                }
            }
            return 100;
        }
        public string buscarAtomo(int atom)
        {
            //Cicla la lista de atomos
            foreach (Atomo atomoreal in listonon)
            {
                //Compara atomo buscado con el que esta ciclando
                if (atom == atomoreal.IdAtomo)
                {
                    //Regresa el nombre del atomo
                    return atomoreal.Atomox;
                }
            }
            //Si no Esta en la lista regresa NULL
            return null;
        }
        public int reverseBuscarAtomo(string atom)
        {
            //Cicla la lista de atomos
            foreach (Atomo atomoreal in listonon)
            {
                //Compara atomo buscado con el que esta ciclando
                if (atom == atomoreal.Atomox)
                {
                    //Regresa el nombre del atomo
                    return atomoreal.IdAtomo;
                }
            }
            //Si no Esta en la lista regresa NULL
            return 100;
        }
        private void CloneListRulesNum()
        {
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_conn.Open();
            sqlite_cmd.CommandText = "SELECT * FROM reglas";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                listRulesClon.Add(new ReglaClon(Convert.ToInt16(sqlite_datareader.GetValue(0).ToString()), sqlite_datareader.GetValue(1).ToString(), sqlite_datareader.GetValue(2).ToString(), sqlite_datareader.GetValue(3).ToString(), sqlite_datareader.GetValue(4).ToString()));

            }
            sqlite_conn.Close();
        }
        private void CloneListRules()
        {
            string rule = "";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_conn.Open();
            sqlite_cmd.CommandText = "SELECT * FROM reglas";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                rule=sqlite_datareader.GetValue(1).ToString();
                string[] rulex = rule.Split('^');
                List<int> banderas = new List<int>();
                foreach (string a in rulex) { banderas.Add(0); }
                listRulesClonNum.Add(
                    new ReglaClonNum(Convert.ToInt16(sqlite_datareader.GetValue(0).ToString()),banderas,0));
            }
            sqlite_conn.Close();
        }
        public string modificarReglas(int antecedente, int atomoValor)
        {
            //Se manda el atomo a valorar y se manda valor dado por el usuario
            /// variables de la regla
            string[] antecedentesRULE;
            string[] antecedentesNEG;
            string Disparado = "";
            int atomoValor1 = 1;
            ///cicla las reglas para analizar
            for (int ix = listRulesClon.Count - 1; ix >= 0; ix--)
            {
                //Separa regla y Signos
                antecedentesRULE = listRulesClon[ix].Reglax.Split('^');
                antecedentesNEG = listRulesClon[ix].Negados.Split(',');
                
                //cicla el tamaño de la regla
                for (int i = 0; i < antecedentesRULE.Length; i++) 
                {
                    //Buscar el Atomo
                    if (antecedente == Convert.ToInt16(antecedentesRULE[i])) 
                    {
                        //Revisa si esa posicion en la regla no ha sido valorada o negada por otra regla
                        if (listRulesClonNum[ix].Reglax[i] == 0)
                        {
                            
                            //Si el signo de la regla es igual a -1
                            if (antecedentesNEG[i] == "-1")
                            {
                                //Toma el valor asigando por el usuario, lo Niega y lo mete a AtomoValor
                                atomoValor1 = atomoValor * (-1);
                                //Introduce el valor en las reglas(Matriz numerica)
                                listRulesClonNum[ix].Reglax[i] = atomoValor1;
                                //Lanza algoritmo para validar si revento alguna regla
                                Disparado = validarRule();
                                atomoValor1 = 1;
                                break;
                            }
                            else if(antecedentesNEG[i]=="1")
                            {
                                //Introduce el Valor en las reglas(Matriz numerica)
                                listRulesClonNum[ix].Reglax[i] = atomoValor;
                                //Lanza algoritmo para validar si revento alguna regla
                                Disparado = validarRule();
                                atomoValor1 = 1;
                                break;
                            }
                        }
                    }
                    
                }
                if (Disparado != "")
                    break;


            }
            return Disparado;
        }
        private string validarRule()
        {
            //Inicializa Banderas
            int bandera = 0;
            int banderaRemover = 0;
            int contadorregla = 0;
            int ex = 0;
            //Cicla las Reglas numericas(Matriz) 
            foreach (ReglaClonNum regla in listRulesClonNum)
            {
                //Regresa bandera a 0
                bandera = 0;
                //Si la Regla no ha sido disparada continua
                if (regla.Conclusion == 0)
                {
                    //asigna a ex la cuenta de atomos que hay en la regla
                    ex = regla.Reglax.Count();
                    //cicla la regla numerica 
                    foreach (int ax in regla.Reglax)
                    {
                        //revisa si el atomo esta en 1, osea que ya se contesto que SI
                        if (ax == 1)
                        {
                            //Aumenta contador
                            bandera++;
                        }
                        if (ax == -1)
                        {
                            banderaRemover--;
                        }
                    }
                    if (banderaRemover < 0)
                    {
                        removeRule(listRulesClon[contadorregla]);
                        regla.Conclusion = -2;
                        banderaRemover = 0;
                    }
                    //Si la bandera es igual a la cuenta de atomos entonces YA se ha Disparado la regla
                    if (bandera == ex)
                    {
                        // asigna 1 a la conclusion numerica
                        if (Convert.ToInt16(listRulesClon[contadorregla].Negado) == 1)
                        {
                            regla.Conclusion = 1;
                        }
                        else
                        {
                            regla.Conclusion = -1;
                        }
                        // Regresa el numero de atomo que disparo la regla
                        return listRulesClon[contadorregla].Conclusion;
                    }
                }
                //aumenta contador para no perder cuenta de que numero de regla se disparo
                contadorregla++;
            }
            //Si no dispara nada regresa "" para continuar ciclando
            return "";
        }
        private void removeRule(ReglaClon ReglaLST)
        {
            //Inicializa Contadores
            int numRule = 0;
            int numAtom = 0;
            //Cicla las Reglas Clon
            foreach(ReglaClon reglaaaa in listRulesClon)
            {
                //Busca si la regla Ciclada es igual a la Regla que mando la funcion
                if(ReglaLST==reglaaaa)
                {
                    //Cicla los atomos separados por ^ de la Regla Ciclada
                    foreach (string reglax in reglaaaa.Reglax.Split('^'))
                    {
                        //La el atomo tiene un valor en 0 en la matriz numerica:
                        if (listRulesClonNum[numRule].Reglax[numAtom] == 0)
                        {
                            //Colocara un -2 asi ya no se podra preguntar por el atomo en el algoritmo modificar
                            listRulesClonNum[numRule].Reglax[numAtom] = -2;
                        }
                        //Aumenta contador para no perder pista del numero de Atomo
                        numAtom++;
                    }
                }
                //Aumenta contador para no perder pista del numero de Regla
                numRule++;
            }
        }
        private void BTNencAtras_Click(object sender, EventArgs e)
        {
                FormPatras Patras = new FormPatras(listRules,listonon);
                Patras.llenarListBox();
                Patras.Show();
        }

        public void BTNnormalizar_Click(object sender, EventArgs e)
        {
            
            StringBuilder sistemaNor = new StringBuilder();
            for (int i = 0; i < listRules.Count(); i++)
            {
                sistemaNor.Append("(" + normalizarRegla(listRules[i]) + ")");
                sistemaNor.Append(i <= listRules.Count() - 2 ? "^":"");
            }
            //MessageBox.Show(sistemaNor.ToString());

            FormObjetivos Objetivos = new FormObjetivos(listonon,this);
            Objetivos.llenarListBox();
            Objetivos.Show();
            
        }

        public string normalizarRegla(Regla reglax)
        {
            #region Inicializa Valores e Imprime
            //Inicializa Valor de Conclusion y su Signo
            int conclusionINT = Convert.ToInt16(reglax.Conclusion);
            int negadoINT = Convert.ToInt16(reglax.Negado);

            //Inicializa Valores de Regla y el Signo de cada atomo
            string[] reglaSTG = reglax.Reglax.Split('^');
            List<int> reglaINT= new List<int>();
            foreach (string x in reglaSTG) { reglaINT.Add(Convert.ToInt16(x)); }

            string[] negadosSTG = reglax.Negados.Split(',');
            List<int> negadosINT = new List<int>();
            foreach (string x in negadosSTG) { negadosINT.Add(Convert.ToInt16(x)); }

            int conclusionINTNew = Convert.ToInt16(reglax.Conclusion);
            int negadoINTNew = Convert.ToInt16(reglax.Negado);

            //Inicializa Valores de Regla y el Signo de cada atomo
            List<int> reglaINTNew = new List<int>();
            List<int> negadosINTNew = new List<int>();

            //Junta en un String(sb) signos y Atomos
            StringBuilder toShow = new StringBuilder();
            StringBuilder reglaOri = new StringBuilder();
            StringBuilder reglaNor = new StringBuilder();
            StringBuilder reglaNorNEGToList = new StringBuilder();
            StringBuilder reglaNorToList = new StringBuilder();
            
            for (int i = 0; i < reglaINT.Count(); i++)
            {

                reglaOri.Append(negadosINT[i] == 1 ? "" : "¬");


                reglaOri.Append(Convert.ToString(reglaINT[i]));
                reglaOri.Append(i <= reglaINT.Count() - 2 ? "^" : "->");
            }


            

            //Junta en el mismo String(sb) el sigo y la conclusion
            reglaOri.Append(negadoINT == 1 ? "" : "¬");
            reglaOri.Append(conclusionINT + "\n");

            for (int i = 0; i < reglaINT.Count(); i++)
            {
                reglaNorNEGToList.Append(Convert.ToString(negadosINT[i]*-1) + ",");
                reglaNorToList.Append(Convert.ToString(reglaINT[i])+ "v");

                reglaINTNew.Add(negadosINT[i] == 1 ? -1 : 1);
                reglaNor.Append(reglaINTNew[i] == 1 ? "" : "¬");
                reglaNor.Append(Convert.ToString(reglaINT[i]));
                reglaNor.Append("v");
            }
            reglaNor.Append(negadoINT == 1 ? "" : "¬");
            reglaNor.Append(conclusionINT);

            reglaNorNEGToList.Append(Convert.ToString(negadoINT));
            reglaNorToList.Append(Convert.ToString(conclusionINT));

            //Imprime Regla y Conclusion ya con signos

            toShow.Append("Regla: \n");
            toShow.Append(reglaOri.ToString());
            toShow.Append("Regla Normalizada: \n");
            toShow.Append(reglaNor.ToString());
            //MessageBox.Show(toShow.ToString());
            #endregion
            listRulesNorm.Add(new ReglaNorm(reglax.IdRegla,reglaNorToList.ToString(), reglaNorNEGToList.ToString()));

            return reglaNor.ToString();
        }

        public void algoritmoObjetivos(int atomo)
        {
            listaPorPreguntar.Clear();
            List<int> atomosInRule = new List<int>();
            List<int> signosInRule = new List<int>();
            List<int> listaReglasTocadas = new List<int>();

            listaPorPreguntar.Add(new int[] { atomo, 1 });
            listaPorPreguntar.Add(new int[] { atomo, -1 });
            int banderaz = 0;
            int contadorDeReglas = 0;
            #region[Llenar ListaPorPreguntar]
            //Ciclar lista por preguntar
            for (int ex = 0; /*ex < listaPorPreguntar.Count()||*/contadorDeReglas<listRules.Count(); ex++)
            {
                //Clicar lista de Reglas
                foreach(ReglaNorm reglanormalizada in listRulesNorm)
                {
                    //Parsea Atomos y signos
                    atomosInRule = reglanormalizada.Reglax.Split('v').Select(Int32.Parse).ToList();
                    signosInRule = reglanormalizada.Negados.Split(',').Select(Int32.Parse).ToList();
                    //Cicla cada atomo y signo en el tamaño de la regla
                    for (int i = 0; i < atomosInRule.Count(); i++)
                    {
                        //Declara Arreglo del atomo ciclado de la regla
                        int[] Acomparar = new int[] { atomosInRule[i], signosInRule[i] };
                        //Compara si el Arreglo del atomo es el que se esta revisando en el Ciclo de la lista preguntada
                        if (Acomparar.SequenceEqual(listaPorPreguntar[ex]))
                        {
                            int bandera = 0;
                            //Si lo encuentra en alguna Regla...
                            //Cicla en cada atomo y signo en el tamaño de la regla ya definida por la busqueda
                            //Agregara los atomos y signos que no esten en la lista por preguntar
                            for(int ix = 0; ix < atomosInRule.Count(); ix++)
                            {
                                int[] AAgregar;
                                //Bandera para determinar que estan o no en la lista
                                
                                //Arreglo de atomo-signo que se agregara a la lista con el signo invertido por Declaracion e algoritmo, NO SE INVERTIRA SIGNO si se trata de si mismo
                                //////////// 
                                AAgregar = ((atomosInRule[ix] != Acomparar[0]) ? new int[] { atomosInRule[ix], (signosInRule[ix] * -1) } : new int[] { atomosInRule[ix], (signosInRule[ix] * 1) });
                                
                                //Cicla en la lista por preguntar para validar que no esten
                                for (int e = 0; e < listaPorPreguntar.Count(); e++)
                                {
                                    //Compara si el arreglo atomo-signo para agregar es el mismo que el que se esta ciclando en la listaporPreguntar
                                    if (!listaPorPreguntar[e].SequenceEqual(AAgregar))
                                    {
                                        //Si no es el mismo se levantara una Bandera para declarar que el arreglo atomo-signo no esta en la Lista
                                        bandera = 1;
                                    }
                                    else
                                    {
                                        //Si es el mismo declara la bandera en 0 para definir que no es
                                        bandera = 0;
                                        //Rompe el Ciclo
                                        break;
                                    }
                                }
                                if (bandera == 1)
                                {
                                    listaPorPreguntar.Add(AAgregar);
                                    banderaz++;
                                    bandera = 0;
                                }
                            }
                        }
                    }
                    contadorDeReglas=(banderaz!=0? contadorDeReglas+1:contadorDeReglas);
                    banderaz = 0;
                }
            }
            #endregion
            listaPorPreguntar.Reverse();
            int atomoVALUE = 0;
            foreach(int[] atomoPregunta in listaPorPreguntar)
            {
            StringBuilder Pregunta = new StringBuilder();
            Pregunta.Append((atomoPregunta[1] == -1) ? "No " : "Si ");
            Pregunta.Append(buscarAtomo(atomoPregunta[0]));
            if (MessageBox.Show("¿" + Pregunta.ToString() + "?",
                                            "Importante Pregunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
            { atomoVALUE = 1; }
            else { atomoVALUE = -1; }
            }

        }


    }
}
