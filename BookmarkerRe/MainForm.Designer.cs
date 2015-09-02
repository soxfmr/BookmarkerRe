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
            this.webUI = new BookmarkerRe.View.WebUI();
            this.SuspendLayout();
            // 
            // webUI
            // 
            this.webUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webUI.Location = new System.Drawing.Point(-1, 0);
            this.webUI.Name = "webUI";
            this.webUI.Size = new System.Drawing.Size(652, 406);
            this.webUI.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 405);
            this.Controls.Add(this.webUI);
            this.Name = "MainForm";
            this.Text = "Bookmarker - Chrome 书签自动整理";
            this.ResumeLayout(false);

        }

        #endregion

        private View.WebUI webUI;
    }
}