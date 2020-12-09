namespace Light4SightNG
{
    partial class ShowResultsForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowResultsForm));
            this.ergebnisBMP = new System.Windows.Forms.PictureBox();
            this.fertig = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.resultText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ergebnisBMP)).BeginInit();
            this.SuspendLayout();
            // 
            // ergebnisBMP
            // 
            this.ergebnisBMP.Location = new System.Drawing.Point(11, 12);
            this.ergebnisBMP.Name = "ergebnisBMP";
            this.ergebnisBMP.Size = new System.Drawing.Size(204, 114);
            this.ergebnisBMP.TabIndex = 0;
            this.ergebnisBMP.TabStop = false;
            // 
            // fertig
            // 
            this.fertig.Location = new System.Drawing.Point(11, 592);
            this.fertig.Name = "fertig";
            this.fertig.Size = new System.Drawing.Size(480, 27);
            this.fertig.TabIndex = 1;
            this.fertig.Text = "Zurück";
            this.fertig.UseVisualStyleBackColor = true;
            this.fertig.Click += new System.EventHandler(this.fertig_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 522);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(480, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "NeuesVerzeichnis";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 506);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Verzeichnis";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 547);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(480, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Verschiebe Daten in Verzeichnis";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // resultText
            // 
            this.resultText.Location = new System.Drawing.Point(11, 132);
            this.resultText.Multiline = true;
            this.resultText.Name = "resultText";
            this.resultText.Size = new System.Drawing.Size(480, 349);
            this.resultText.TabIndex = 5;
            // 
            // ZeigeErgebnis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 640);
            this.Controls.Add(this.resultText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.fertig);
            this.Controls.Add(this.ergebnisBMP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ZeigeErgebnis";
            this.Text = "Ergebnis";
            ((System.ComponentModel.ISupportInitialize)(this.ergebnisBMP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.PictureBox ergebnisBMP;
        System.Windows.Forms.Button fertig;
        System.Windows.Forms.TextBox textBox1;
        System.Windows.Forms.Label label1;
        System.Windows.Forms.Button button1;
        System.Windows.Forms.TextBox resultText;
    }
}