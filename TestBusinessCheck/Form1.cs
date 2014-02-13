using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestBusinessCheck.BC;

namespace TestBusinessCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BC.DataImport2SoapClient ctx = new DataImport2SoapClient("DataImport2Soap");
            BC.DataImport2Result result;
            
            result = ctx.DataImport2Company("tt", "integration", "ttbc0701", "sv", "RatingLimit", "556630-3433");

            foreach (BC.Block b in result.Blocks)
            {
                foreach (BC.Field f in b.Fields)
                {
                    listBox1.Items.Add(f.Code + " : " + f.Value);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
