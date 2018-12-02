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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (.txt)|*.txt";
            ofd.Title = "Open a file...";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                richTextBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog svf = new SaveFileDialog();
            svf.Filter = "Text files (.txt)|*.txt";
            svf.Title = "Save file...";
            if (svf.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(svf.FileName);
                sw.Write(richTextBox1.Text);
                sw.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }
    }
}