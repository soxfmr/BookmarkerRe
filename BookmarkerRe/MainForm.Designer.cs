namespace BookmarkerRe
{
    partial class MainForm
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
            this.MainUserInterface = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // MainUserInterface
            // 
            this.MainUserInterface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainUserInterface.Location = new System.Drawing.Point(0, 0);
            this.MainUserInterface.MinimumSize = new System.Drawing.Size(20, 20);
            this.MainUserInterface.Name = "MainUserInterface";
            this.MainUserInterface.Size = new System.Drawing.Size(651, 405);
            this.MainUserInterface.TabIndex = 0;
            this.MainUserInterface.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.MainUserInterface_DocumentCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 405);
            this.Controls.Add(this.MainUserInterface);
            this.Name = "MainForm";
            this.Text = "Bookmarker - Chrome 书签自动整理";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser MainUserInterface;
    }
}