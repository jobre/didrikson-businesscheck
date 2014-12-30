using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DidriksonsForms
{
    public partial class BCWeb : Form
    {
        public BCWeb()
        {
            InitializeComponent();
        }

        public BCWeb(string address)
        {
            InitializeComponent();

            wbBC.Navigated += new WebBrowserNavigatedEventHandler(wbBC_Navigated);
            wbBC.Navigate(address, false);
        }

        void wbBC_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            wbBC.Refresh();
        }

        private void BCWeb_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void wbBC_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
