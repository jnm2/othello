namespace Othello
{
    partial class Form1
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
            this.othelloBoard1 = new Othello.OthelloBoard();
            this.SuspendLayout();
            // 
            // othelloBoard1
            // 
            this.othelloBoard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.othelloBoard1.Location = new System.Drawing.Point(0, 0);
            this.othelloBoard1.Name = "othelloBoard1";
            this.othelloBoard1.Size = new System.Drawing.Size(644, 513);
            this.othelloBoard1.TabIndex = 0;
            this.othelloBoard1.Text = "othelloBoard1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 513);
            this.Controls.Add(this.othelloBoard1);
            this.Name = "Form1";
            this.Text = "Othello";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OthelloBoard othelloBoard1;

    }
}

