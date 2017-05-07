using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rita
{
    public partial class NewFile : Form
    {
        public int width, height;

        public NewFile()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tbxWidth.Text, out width) || !int.TryParse(tbxHeight.Text, out height))
            {
                MessageBox.Show("Please insert a number as value", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Dispose();
            }
          
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
