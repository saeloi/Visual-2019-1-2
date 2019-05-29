using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace A015_UnitConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txtCm.Text!="")
            {
                txtIn.Text = (double.Parse(txtCm.Text) * 0.3937).ToString();
                txtY.Text = (double.Parse(txtCm.Text) * 0.0109).ToString();
                txtF.Text = (double.Parse(txtCm.Text) * 0.0328).ToString();
            }
            if (txtIn.Text != "")
            {
                txtCm.Text = (double.Parse(txtIn.Text) * 2.54).ToString();
                txtY.Text = (double.Parse(txtIn.Text) * 0.0278).ToString();
                txtF.Text = (double.Parse(txtIn.Text) * 0.0833).ToString();
            }
            if (txtF.Text != "")
            {
                txtIn.Text = (double.Parse(txtF.Text) * 12.0).ToString();
                txtY.Text = (double.Parse(txtF.Text) * 0.333).ToString();
                txtCm.Text = (double.Parse(txtF.Text) * 30.48).ToString();
            }
            if (txtY.Text != "")
            {
                txtIn.Text = (double.Parse(txtY.Text) * 36.0).ToString();
                txtCm.Text = (double.Parse(txtY.Text) * 91.438).ToString();
                txtF.Text = (double.Parse(txtY.Text) * 3.0).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtCm.Text = "";
            txtF.Text = "";
            txtIn.Text = "";
            txtY.Text = "";
        }
    }
}
