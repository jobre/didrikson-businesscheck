using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCS;

namespace FixOrder
{
    public partial class Form1 : Form
    {
        private GarpGenericDB mOGA, mOGR, mOGK, mI2H, mI2R, mI2T;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            mOGA = new GarpGenericDB("OGA");
            mOGR = new GarpGenericDB("OGR");
            mOGK = new GarpGenericDB("OGK");
            mI2H = new GarpGenericDB("I2H");
            mI2R = new GarpGenericDB("I2R");
            mI2T = new GarpGenericDB("I2T");

            if (string.IsNullOrEmpty(txtOnr.Text))
            {
                if(MessageBox.Show(this, "Vill du lägga på kundtext på samtliga olevererade order?", "Uppdatera order", MessageBoxButtons.YesNo ) == System.Windows.Forms.DialogResult.Yes)
                {
                    allOrder();
                    MessageBox.Show("Klart");
                }
            }
            else
            {
                justOneOrder(txtOnr.Text);
                MessageBox.Show("Klart");
            }

        }

        private void allOrder()
        {
            mOGA.first();
            while (!mOGA.EOF)
            {
                if (mOGA.getValue("LEF").Equals("0"))
                {
                    string text = getCustomerText(mOGA.getValue("KNR"));

                    if (!string.IsNullOrEmpty(text))
                    {
                        string row = "";
                        string onr = mOGA.getValue("ONR");

                        mOGR.find(mOGA.getValue("ONR").PadRight(6));
                        mOGR.next();

                        if (mOGR.getValue("ONR").Equals(mOGA.getValue("ONR")))
                        {
                            row = mOGR.getValue("RDC");
                            addRowText(text, onr);
                        }
                    }
                }

                mOGA.next();
            }
        }

        private void justOneOrder(string onr)
        {
            if (mOGA.find(onr))
            {
                string text = getCustomerText(mOGA.getValue("KNR"));

                if (!string.IsNullOrEmpty(text))
                {
                    string row = "";

                    mOGR.find(mOGA.getValue("ONR").PadRight(6));
                    mOGR.next();

                    if (mOGR.getValue("ONR").Equals(mOGA.getValue("ONR")))
                    {
                        row = mOGR.getValue("RDC");
                        addRowText(text, onr);
                    }
                }
            }
        }

        private void addRowText(string text, string onr)
        {
            List<OrderRowText> oldText = new List<OrderRowText>();
            List<OrderRowText> addText = new List<OrderRowText>();
            bool isNewText = true;

            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length > 60)
                    text = text.Substring(0, 60);

                OrderRowText o = new OrderRowText();
                o.Text = text;
                o.PLF = "1";
                o.OBF = "0";
                o.FSF = "0";
                o.FAF = "A";
                o.SQC = "255";
                addText.Add(o);
            }
            else
                return;

            try
            {
                mOGR.find(onr);
                mOGR.next();

                while (mOGR.getValue("ONR").Trim().Equals(onr.Trim()) || mOGR.EOF)
                {
                    mOGK.find(onr.PadRight(6) + mOGR.getValue("RDC"));
                    mOGK.next();

                    oldText.Clear();
                    while (mOGK.getValue("ONR").Trim().Equals(onr.Trim()) && mOGK.getValue("RDC").Trim().Equals(mOGR.getValue("RDC").Trim()) && !mOGK.EOF)
                    {
                        OrderRowText o = new OrderRowText();

                        o.Text = mOGK.getValue("TX1");
                        o.ONR = mOGK.getValue("ONR");
                        o.RDC = mOGK.getValue("RDC");
                        o.SQC = mOGK.getValue("SQC");
                        o.FAF = mOGK.getValue("FAF");
                        o.FSF = mOGK.getValue("FSF");
                        o.OBF = mOGK.getValue("OBF");
                        o.PLF = mOGK.getValue("PLF");

                        if (o.FAF.Equals("A"))
                        {
                            if (o.Text.Replace(" ", "").Trim().Equals(text.Replace(" ", "").Trim()))
                                isNewText = false;
                        }

                        oldText.Add(o);
                        mOGK.next();
                    }

                    if (isNewText)
                    {
                        foreach (OrderRowText o in oldText)
                        {
                            if (mOGK.find(mOGR.getValue("ONR").PadRight(6) + mOGK.getValue("RDC").PadLeft(3) + o.SQC.PadLeft(3)))
                            {
                                mOGK.delete();
                                if (!o.FAF.Equals("A"))
                                    addText.Add(o);
                            }
                        }

                        int sqc = 1;
                        foreach (OrderRowText o in addText)
                        {
                            mOGK.insert();
                            mOGK.setValue("ONR", GCF.noNULL(mOGR.getValue("ONR")));
                            mOGK.setValue("RDC", mOGR.getValue("RDC").PadLeft(3));
                            mOGK.setValue("OSE", "K");
                            mOGK.setValue("SQC", "255");
                            mOGK.setValue("TX1", o.Text);
                            mOGK.setValue("OBF", o.OBF);
                            mOGK.setValue("PLF", o.PLF);
                            mOGK.setValue("FSF", o.FSF);
                            mOGK.setValue("FAF", o.FAF);
                            mOGK.post();
                        }
                    }

                    mOGR.next();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("addRowText: " + e.Message);
            }
        }

        private string getCustomerText(string customer)
        {
            if (mI2H.find("M28" + customer.Trim()))
            {
                string lastContactKey = mI2H.getValue("HN1") + mI2H.getValue("HN2") + mI2H.getValue("HN3");

                mI2R.find(lastContactKey + "  0" + "  1");
                mI2R.next();

                if ((lastContactKey.Equals(mI2R.getValue("HN1") + mI2R.getValue("HN2") + mI2R.getValue("HN3"))))
                {
                    string textKey = mI2R.getValue("HN1") + mI2R.getValue("HN2") + mI2R.getValue("HN3");

                    mI2T.find(textKey);
                    mI2T.next();

                    if ((textKey.Equals(mI2T.getValue("HN1") + mI2T.getValue("HN2") + mI2T.getValue("HN3"))))
                    {
                        return mI2T.getValue("BUF");
                    }
                }
            }

            return "";
        }

        private bool checkOrgNr(string orgnr)
        {
            if (orgnr != null)
                return !orgnr.Equals("");
            else
                return false;

        }
    }

    public class OrderRowText
    {
        public string ONR { get; set; }
        public string RDC { get; set; }
        public string SQC { get; set; }
        public string Text { get; set; }
        public string FAF { get; set; }
        public string OBF { get; set; }
        public string PLF { get; set; }
        public string FSF { get; set; }
    }

}
