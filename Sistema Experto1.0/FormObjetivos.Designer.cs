namespace Sistema_Experto1._0
{
    partial class FormObjetivos
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
            this.LSTBAtomos = new System.Windows.Forms.ListBox();
            this.BTNSeleccionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LSTBAtomos
            // 
            this.LSTBAtomos.FormattingEnabled = true;
            this.LSTBAtomos.Location = new System.Drawing.Point(12, 12);
            this.LSTBAtomos.Name = "LSTBAtomos";
            this.LSTBAtomos.Size = new System.Drawing.Size(171, 173);
            this.LSTBAtomos.TabIndex = 0;
            // 
            // BTNSeleccionar
            // 
            this.BTNSeleccionar.Location = new System.Drawing.Point(12, 191);
            this.BTNSeleccionar.Name = "BTNSeleccionar";
            this.BTNSeleccionar.Size = new System.Drawing.Size(171, 23);
            this.BTNSeleccionar.TabIndex = 1;
            this.BTNSeleccionar.Text = "Seleccionar";
            this.BTNSeleccionar.UseVisualStyleBackColor = true;
            this.BTNSeleccionar.Click += new System.EventHandler(this.BTNSeleccionar_Click);
            // 
            // FormObjetivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 224);
            this.Controls.Add(this.BTNSeleccionar);
            this.Controls.Add(this.LSTBAtomos);
            this.Name = "FormObjetivos";
            this.Text = "FormObjetivos";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LSTBAtomos;
        private System.Windows.Forms.Button BTNSeleccionar;
    }
}