using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamGamesTool
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            SteamPathBox.Text = Settings1.Default.SteamPath;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string path = SteamPathBox.Text;
            Settings1.Default.SteamPath = path;
            Settings1.Default.Save();
            
        }
    }
}
