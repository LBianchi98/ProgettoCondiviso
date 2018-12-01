using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace progetto
{
    public partial class Form1 : Form
    {
        List<int> numeri = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }


        // La funzione add() controlla se i numeri nella lista sono minori di dieci 
        // e li aggiunge nella listbox prendendoli dalla textbox
        // altrimenti disabilita il bottone btnAdd.
        public void add()
        {
            try
            {
                if (numeri.Count() < 10)
                    numeri.Add(Convert.ToInt32(txtB.Text));
                else
                    btnAdd.Enabled = false;
            }
            catch (FormatException e) {
                MessageBox.Show("inserire un numero valido");
            }
        }

        public void SalvaSuFile(List<int> numeri)
        {
            FileStream fileStream = new FileStream(
            "numeri.txt", FileMode.OpenOrCreate,
            FileAccess.ReadWrite, FileShare.None);

            byte[] bdata = null;
            String str = String.Empty;

            foreach (int i in numeri)
            {
                str = str + i.ToString() + ',';
            }

            bdata = Encoding.ASCII.GetBytes(str);

            fileStream.Write(bdata, 0, bdata.Length);

            fileStream.Close();
        }

        public List<int> CaricaDaFile()
        {
            FileStream fileStream = new FileStream(
            "numeri.txt", FileMode.OpenOrCreate,
            FileAccess.ReadWrite, FileShare.None);

            List<int> n = new List<int>();
            string fileContent;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                fileContent = reader.ReadToEnd();
            }
            
            List<string> tokens = fileContent.Split(',').ToList();

            tokens.RemoveAll(str => string.IsNullOrEmpty(str));

            foreach (string s in tokens)
            {
                n.Add(Convert.ToInt32(s));
                
            }

            fileStream.Close();

            return n;
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            if (lstbx.Items.Count == 0)
            {
                MessageBox.Show("Impossibile salvare poichè la lista è vuota.");
            }
            else
            {
                SalvaSuFile(numeri.ToList<int>());
            }
        }

        private void btnCarica_Click(object sender, EventArgs e)
        {   
            numeri = CaricaDaFile();
            numeri.Sort();
            lstbx.DataSource = numeri;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtB.Text != "")
            {
                add();
                numeri.Sort();
                lstbx.DataSource = null;
                lstbx.DataSource = numeri;
                txtB.Text = "";
            }
            else
            {
                MessageBox.Show("Impossibile aggiungere poichè non è stato inserito alcun elemento.");
            }
        }

        private void lstbx_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // La funzione btnClear_Click() svuota la listbox eliminando tutti i numeri e la lista numeri pure viene svuotata.
        private void btnClear_Click(object sender, EventArgs e)
        {
            lstbx.DataSource = null;
            numeri.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Viene mostrato in background il form principale come richiesto dall'esercizio
            Show();
            bool done = false;
            //Un thread pool è un gestore di thread 
            //utilizzato per ottimizzare e semplificare l'utilizzo dei thread.
            //QueueUserWorkItem accoda un metodo da eseguire. Il metodo
            //viene eseguito quando un thread del pool di thread diventa disponibile.
            ThreadPool.QueueUserWorkItem((x) =>
            {
                using (var splashform = new SplashForm())
                {
                    //Viene mostrato il form responsabile dello Splash Screen
                    splashform.Show();
                    while (!done)
                    {
                        Application.DoEvents();
                    }
                    splashform.Close();
                }
            });
            Thread.Sleep(3000);
            done = true;
            //Attiva il form principale dopo che il form dello splash screen è stato chiuso
            Activate();
        }
    }
}