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
    public partial class SizeForm : Form
    {

        public int size;

        public SizeForm()
        {
            InitializeComponent();
        }

        private void SizeForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(int.TryParse(tbxSize.Text, out size)){
                if (size <= 30)
                {
                    this.Dispose();
                }
                else {
                    MessageBox.Show("Value was over 30px, setting size to 30px", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    size = 30;
                    this.Dispose();
                }
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}
