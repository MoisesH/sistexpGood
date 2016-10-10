namespace Sistema_Experto1._0
{
    partial class FormPatras
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
            this.LSTBfinales = new System.Windows.Forms.ListBox();
            this.BTNseleccionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LSTBfinales
            // 
            this.LSTBfinales.FormattingEnabled = true;
            this.LSTBfinales.Location = new System.Drawing.Point(12, 12);
            this.LSTBfinales.Name = "LSTBfinales";
            this.LSTBfinales.Size = new System.Drawing.Size(171, 173);
            this.LSTBfinales.TabIndex = 0;
            // 
            // BTNseleccionar
            // 
            this.BTNseleccionar.Location = new System.Drawing.Point(12, 191);
            this.BTNseleccionar.Name = "BTNseleccionar";
            this.BTNseleccionar.Size = new System.Drawing.Size(171, 23);
            this.BTNseleccionar.TabIndex = 1;
            this.BTNseleccionar.Text = "Seleccionar";
            this.BTNseleccionar.UseVisualStyleBackColor = true;
            this.BTNseleccionar.Click += new System.EventHandler(this.BTNseleccionar_Click);
            // 
            // FormPatras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 224);
            this.Controls.Add(this.BTNseleccionar);
            this.Controls.Add(this.LSTBfinales);
            this.Name = "FormPatras";
            this.Text = "FormPatras";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LSTBfinales;
        private System.Windows.Forms.Button BTNseleccionar;
    }
}