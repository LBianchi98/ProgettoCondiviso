using System.Windows.Forms;

namespace progetto
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();

            //Viene reso il background del Form trasparente
            TransparencyKey = BackColor;
        }
    }
}
