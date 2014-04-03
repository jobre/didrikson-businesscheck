namespace TTForms
{
    partial class BCWeb
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCWeb));
            this.wbBC = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wbBC
            // 
            this.wbBC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbBC.Location = new System.Drawing.Point(0, 0);
            this.wbBC.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbBC.Name = "wbBC";
            this.wbBC.Size = new System.Drawing.Size(998, 624);
            this.wbBC.TabIndex = 0;
            this.wbBC.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbBC_DocumentCompleted);
            // 
            // BCWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 624);
            this.Controls.Add(this.wbBC);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BCWeb";
            this.Text = "BusinessCheck Rapport";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BCWeb_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbBC;
    }
}