namespace IgniteUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.downloadDescString = new System.Windows.Forms.Label();
            this.downloadInfoString = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 72);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(398, 15);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Обновление:";
            // 
            // downloadDescString
            // 
            this.downloadDescString.AutoSize = true;
            this.downloadDescString.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.downloadDescString.Location = new System.Drawing.Point(14, 34);
            this.downloadDescString.Name = "downloadDescString";
            this.downloadDescString.Size = new System.Drawing.Size(133, 17);
            this.downloadDescString.TabIndex = 2;
            this.downloadDescString.Text = "-- загрузка файлов";
            // 
            // downloadInfoString
            // 
            this.downloadInfoString.AutoSize = true;
            this.downloadInfoString.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.downloadInfoString.Location = new System.Drawing.Point(370, 93);
            this.downloadInfoString.Name = "downloadInfoString";
            this.downloadInfoString.Size = new System.Drawing.Size(44, 17);
            this.downloadInfoString.TabIndex = 3;
            this.downloadInfoString.Text = "100%";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(422, 119);
            this.Controls.Add(this.downloadInfoString);
            this.Controls.Add(this.downloadDescString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ignite Update Tool";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label downloadDescString;
        private System.Windows.Forms.Label downloadInfoString;
    }
}