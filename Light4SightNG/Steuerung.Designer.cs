namespace Light4SightNG
{
    partial class Steuerung
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Steuerung));
            this.Rod = new System.Windows.Forms.CheckBox();
            this.LCone = new System.Windows.Forms.CheckBox();
            this.MCone = new System.Windows.Forms.CheckBox();
            this.SCone = new System.Windows.Forms.CheckBox();
            this.RCFF = new System.Windows.Forms.TextBox();
            this.LCFF = new System.Windows.Forms.TextBox();
            this.MCFF = new System.Windows.Forms.TextBox();
            this.SCFF = new System.Windows.Forms.TextBox();
            this.mLCFF = new System.Windows.Forms.Button();
            this.mRCFF = new System.Windows.Forms.Button();
            this.mMCFF = new System.Windows.Forms.Button();
            this.mSCFF = new System.Windows.Forms.Button();
            this.gbProband = new System.Windows.Forms.GroupBox();
            this.cbAugenseite = new System.Windows.Forms.ComboBox();
            this.lblAugenseite = new System.Windows.Forms.Label();
            this.tbProbandenNummer = new System.Windows.Forms.TextBox();
            this.lblProbandenNummer = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mACFF = new System.Windows.Forms.Button();
            this.ACFF = new System.Windows.Forms.TextBox();
            this.result = new System.Windows.Forms.Button();
            this.ergebnisseVorhanden = new System.IO.FileSystemWatcher();
            this.UseDefaultCFF = new System.Windows.Forms.CheckBox();
            this.dateinamen = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.envFreq = new System.Windows.Forms.ComboBox();
            this.pEnv = new System.Windows.Forms.CheckBox();
            this.selectAlgorithm = new System.Windows.Forms.ListBox();
            this.LabelAlgorithms = new System.Windows.Forms.Label();
            this.NumberOfTrials = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PredictedThreshold = new System.Windows.Forms.NumericUpDown();
            this.gbProband.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ergebnisseVorhanden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfTrials)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PredictedThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // Rod
            // 
            this.Rod.AutoSize = true;
            this.Rod.Location = new System.Drawing.Point(30, 132);
            this.Rod.Name = "Rod";
            this.Rod.Size = new System.Drawing.Size(72, 17);
            this.Rod.TabIndex = 0;
            this.Rod.Text = "Stäbchen";
            this.Rod.UseVisualStyleBackColor = true;
            this.Rod.CheckedChanged += new System.EventHandler(this.Rod_CheckedChanged);
            // 
            // LCone
            // 
            this.LCone.AutoSize = true;
            this.LCone.Location = new System.Drawing.Point(30, 158);
            this.LCone.Name = "LCone";
            this.LCone.Size = new System.Drawing.Size(69, 17);
            this.LCone.TabIndex = 1;
            this.LCone.Text = "L-Zapfen";
            this.LCone.UseVisualStyleBackColor = true;
            this.LCone.CheckedChanged += new System.EventHandler(this.LCone_CheckedChanged);
            // 
            // MCone
            // 
            this.MCone.AutoSize = true;
            this.MCone.Location = new System.Drawing.Point(30, 184);
            this.MCone.Name = "MCone";
            this.MCone.Size = new System.Drawing.Size(72, 17);
            this.MCone.TabIndex = 2;
            this.MCone.Text = "M-Zapfen";
            this.MCone.UseVisualStyleBackColor = true;
            this.MCone.CheckedChanged += new System.EventHandler(this.MCone_CheckedChanged);
            // 
            // SCone
            // 
            this.SCone.AutoSize = true;
            this.SCone.Location = new System.Drawing.Point(30, 210);
            this.SCone.Name = "SCone";
            this.SCone.Size = new System.Drawing.Size(70, 17);
            this.SCone.TabIndex = 3;
            this.SCone.Text = "S-Zapfen";
            this.SCone.UseVisualStyleBackColor = true;
            this.SCone.CheckedChanged += new System.EventHandler(this.SCone_CheckedChanged);
            // 
            // RCFF
            // 
            this.RCFF.Location = new System.Drawing.Point(108, 132);
            this.RCFF.Name = "RCFF";
            this.RCFF.Size = new System.Drawing.Size(100, 20);
            this.RCFF.TabIndex = 4;
            this.RCFF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LCFF
            // 
            this.LCFF.Location = new System.Drawing.Point(108, 158);
            this.LCFF.Name = "LCFF";
            this.LCFF.Size = new System.Drawing.Size(100, 20);
            this.LCFF.TabIndex = 5;
            this.LCFF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MCFF
            // 
            this.MCFF.Location = new System.Drawing.Point(108, 184);
            this.MCFF.Name = "MCFF";
            this.MCFF.Size = new System.Drawing.Size(100, 20);
            this.MCFF.TabIndex = 6;
            this.MCFF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SCFF
            // 
            this.SCFF.Location = new System.Drawing.Point(108, 210);
            this.SCFF.Name = "SCFF";
            this.SCFF.Size = new System.Drawing.Size(100, 20);
            this.SCFF.TabIndex = 7;
            this.SCFF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mLCFF
            // 
            this.mLCFF.Location = new System.Drawing.Point(214, 155);
            this.mLCFF.Name = "mLCFF";
            this.mLCFF.Size = new System.Drawing.Size(75, 23);
            this.mLCFF.TabIndex = 9;
            this.mLCFF.Text = "messen";
            this.mLCFF.UseVisualStyleBackColor = true;
            this.mLCFF.Click += new System.EventHandler(this.mLCFF_Click);
            // 
            // mRCFF
            // 
            this.mRCFF.Location = new System.Drawing.Point(214, 129);
            this.mRCFF.Name = "mRCFF";
            this.mRCFF.Size = new System.Drawing.Size(75, 23);
            this.mRCFF.TabIndex = 10;
            this.mRCFF.Text = "messen";
            this.mRCFF.UseVisualStyleBackColor = true;
            this.mRCFF.Click += new System.EventHandler(this.mRCFF_Click);
            // 
            // mMCFF
            // 
            this.mMCFF.Location = new System.Drawing.Point(214, 181);
            this.mMCFF.Name = "mMCFF";
            this.mMCFF.Size = new System.Drawing.Size(75, 23);
            this.mMCFF.TabIndex = 11;
            this.mMCFF.Text = "messen";
            this.mMCFF.UseVisualStyleBackColor = true;
            this.mMCFF.Click += new System.EventHandler(this.mMCFF_Click);
            // 
            // mSCFF
            // 
            this.mSCFF.Location = new System.Drawing.Point(214, 207);
            this.mSCFF.Name = "mSCFF";
            this.mSCFF.Size = new System.Drawing.Size(75, 23);
            this.mSCFF.TabIndex = 12;
            this.mSCFF.Text = "messen";
            this.mSCFF.UseVisualStyleBackColor = true;
            this.mSCFF.Click += new System.EventHandler(this.mSCFF_Click);
            // 
            // gbProband
            // 
            this.gbProband.Controls.Add(this.cbAugenseite);
            this.gbProband.Controls.Add(this.lblAugenseite);
            this.gbProband.Controls.Add(this.tbProbandenNummer);
            this.gbProband.Controls.Add(this.lblProbandenNummer);
            this.gbProband.Location = new System.Drawing.Point(45, 12);
            this.gbProband.Name = "gbProband";
            this.gbProband.Size = new System.Drawing.Size(227, 96);
            this.gbProband.TabIndex = 13;
            this.gbProband.TabStop = false;
            this.gbProband.Text = "Proband";
            // 
            // cbAugenseite
            // 
            this.cbAugenseite.FormattingEnabled = true;
            this.cbAugenseite.Items.AddRange(new object[] {
            "OD",
            "OS"});
            this.cbAugenseite.Location = new System.Drawing.Point(108, 54);
            this.cbAugenseite.Name = "cbAugenseite";
            this.cbAugenseite.Size = new System.Drawing.Size(53, 21);
            this.cbAugenseite.TabIndex = 3;
            // 
            // lblAugenseite
            // 
            this.lblAugenseite.AutoSize = true;
            this.lblAugenseite.Location = new System.Drawing.Point(6, 57);
            this.lblAugenseite.Name = "lblAugenseite";
            this.lblAugenseite.Size = new System.Drawing.Size(60, 13);
            this.lblAugenseite.TabIndex = 2;
            this.lblAugenseite.Text = "Augenseite";
            // 
            // tbProbandenNummer
            // 
            this.tbProbandenNummer.Location = new System.Drawing.Point(108, 23);
            this.tbProbandenNummer.Name = "tbProbandenNummer";
            this.tbProbandenNummer.Size = new System.Drawing.Size(77, 20);
            this.tbProbandenNummer.TabIndex = 1;
            // 
            // lblProbandenNummer
            // 
            this.lblProbandenNummer.AutoSize = true;
            this.lblProbandenNummer.Location = new System.Drawing.Point(6, 26);
            this.lblProbandenNummer.Name = "lblProbandenNummer";
            this.lblProbandenNummer.Size = new System.Drawing.Size(96, 13);
            this.lblProbandenNummer.TabIndex = 0;
            this.lblProbandenNummer.Text = "Probandennummer";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(30, 312);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(259, 38);
            this.start.TabIndex = 14;
            this.start.Text = "Starte Untersuchung";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 115);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "CFF";
            // 
            // mACFF
            // 
            this.mACFF.Location = new System.Drawing.Point(214, 247);
            this.mACFF.Name = "mACFF";
            this.mACFF.Size = new System.Drawing.Size(75, 23);
            this.mACFF.TabIndex = 16;
            this.mACFF.Text = "Alle messen";
            this.mACFF.UseVisualStyleBackColor = true;
            this.mACFF.Click += new System.EventHandler(this.button1_Click);
            // 
            // ACFF
            // 
            this.ACFF.Location = new System.Drawing.Point(108, 247);
            this.ACFF.Name = "ACFF";
            this.ACFF.Size = new System.Drawing.Size(100, 20);
            this.ACFF.TabIndex = 17;
            this.ACFF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ACFF.TextChanged += new System.EventHandler(this.ACFF_TextChanged);
            // 
            // result
            // 
            this.result.Location = new System.Drawing.Point(30, 356);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(259, 38);
            this.result.TabIndex = 18;
            this.result.Text = "Zeige Ergebnisse von letzter Untersuchung";
            this.result.UseVisualStyleBackColor = true;
            this.result.Click += new System.EventHandler(this.result_Click);
            // 
            // ergebnisseVorhanden
            // 
            this.ergebnisseVorhanden.EnableRaisingEvents = true;
            this.ergebnisseVorhanden.Path = ".\\Untersuchungen";
            this.ergebnisseVorhanden.SynchronizingObject = this;
            this.ergebnisseVorhanden.Changed += new System.IO.FileSystemEventHandler(this.ergebnisseVorhanden_Changed);
            this.ergebnisseVorhanden.Created += new System.IO.FileSystemEventHandler(this.ergebnisseVorhanden_Created);
            this.ergebnisseVorhanden.Deleted += new System.IO.FileSystemEventHandler(this.ergebnisseVorhanden_Deleted);
            // 
            // UseDefaultCFF
            // 
            this.UseDefaultCFF.AutoSize = true;
            this.UseDefaultCFF.Location = new System.Drawing.Point(108, 273);
            this.UseDefaultCFF.Name = "UseDefaultCFF";
            this.UseDefaultCFF.Size = new System.Drawing.Size(82, 17);
            this.UseDefaultCFF.TabIndex = 20;
            this.UseDefaultCFF.Text = "Default CFF";
            this.UseDefaultCFF.UseVisualStyleBackColor = true;
            this.UseDefaultCFF.CheckedChanged += new System.EventHandler(this.UseDefaultCFF_CheckedChanged);
            // 
            // dateinamen
            // 
            this.dateinamen.Location = new System.Drawing.Point(331, 247);
            this.dateinamen.Name = "dateinamen";
            this.dateinamen.Size = new System.Drawing.Size(131, 23);
            this.dateinamen.TabIndex = 25;
            this.dateinamen.Text = "Sperren/Entsperren";
            this.dateinamen.UseVisualStyleBackColor = true;
            this.dateinamen.Click += new System.EventHandler(this.dateinamen_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(331, 131);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 21);
            this.comboBox1.TabIndex = 26;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(331, 157);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(131, 21);
            this.comboBox2.TabIndex = 27;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(331, 184);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(131, 21);
            this.comboBox3.TabIndex = 28;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.Enabled = false;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(331, 209);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(131, 21);
            this.comboBox4.TabIndex = 29;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Envelope - Frequenz";
            // 
            // envFreq
            // 
            this.envFreq.FormattingEnabled = true;
            this.envFreq.Items.AddRange(new object[] {
            "0",
            "1/8",
            "1/4",
            "1/2",
            "1"});
            this.envFreq.Location = new System.Drawing.Point(520, 69);
            this.envFreq.Name = "envFreq";
            this.envFreq.Size = new System.Drawing.Size(121, 21);
            this.envFreq.TabIndex = 32;
            this.envFreq.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // pEnv
            // 
            this.pEnv.AutoSize = true;
            this.pEnv.Location = new System.Drawing.Point(520, 98);
            this.pEnv.Name = "pEnv";
            this.pEnv.Size = new System.Drawing.Size(104, 17);
            this.pEnv.TabIndex = 33;
            this.pEnv.Text = "Pause Envelope";
            this.pEnv.UseVisualStyleBackColor = true;
            this.pEnv.CheckedChanged += new System.EventHandler(this.pEnv_CheckedChanged);
            // 
            // selectAlgorithm
            // 
            this.selectAlgorithm.FormattingEnabled = true;
            this.selectAlgorithm.Items.AddRange(new object[] {
            "Randomly-Interleaved-Staircases",
            "Constant Stimuli",
            "Threshold finder (BP)"});
            this.selectAlgorithm.Location = new System.Drawing.Point(520, 171);
            this.selectAlgorithm.Name = "selectAlgorithm";
            this.selectAlgorithm.Size = new System.Drawing.Size(163, 43);
            this.selectAlgorithm.TabIndex = 34;
            this.selectAlgorithm.SelectedIndexChanged += new System.EventHandler(this.selectAlgorithm_SelectedIndexChanged);
            // 
            // LabelAlgorithms
            // 
            this.LabelAlgorithms.AutoSize = true;
            this.LabelAlgorithms.Location = new System.Drawing.Point(517, 155);
            this.LabelAlgorithms.Name = "LabelAlgorithms";
            this.LabelAlgorithms.Size = new System.Drawing.Size(122, 13);
            this.LabelAlgorithms.TabIndex = 35;
            this.LabelAlgorithms.Text = "Teststrategie auswählen";
            // 
            // NumberOfTrials
            // 
            this.NumberOfTrials.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NumberOfTrials.Location = new System.Drawing.Point(520, 260);
            this.NumberOfTrials.Name = "NumberOfTrials";
            this.NumberOfTrials.Size = new System.Drawing.Size(120, 20);
            this.NumberOfTrials.TabIndex = 36;
            this.NumberOfTrials.ValueChanged += new System.EventHandler(this.NumberOfTrials_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(497, 244);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Anzahl Trials (Constant Stimuli and BP)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(497, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(191, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Predicted threshold for Constant Stimuli\r\n";
            // 
            // PredictedThreshold
            // 
            this.PredictedThreshold.DecimalPlaces = 2;
            this.PredictedThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PredictedThreshold.Location = new System.Drawing.Point(520, 330);
            this.PredictedThreshold.Name = "PredictedThreshold";
            this.PredictedThreshold.Size = new System.Drawing.Size(120, 20);
            this.PredictedThreshold.TabIndex = 40;
            this.PredictedThreshold.ValueChanged += new System.EventHandler(this.PredictedThreshold_ValueChanged);
            // 
            // Steuerung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 415);
            this.Controls.Add(this.PredictedThreshold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NumberOfTrials);
            this.Controls.Add(this.LabelAlgorithms);
            this.Controls.Add(this.selectAlgorithm);
            this.Controls.Add(this.pEnv);
            this.Controls.Add(this.envFreq);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateinamen);
            this.Controls.Add(this.UseDefaultCFF);
            this.Controls.Add(this.result);
            this.Controls.Add(this.ACFF);
            this.Controls.Add(this.mACFF);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.start);
            this.Controls.Add(this.gbProband);
            this.Controls.Add(this.mSCFF);
            this.Controls.Add(this.mMCFF);
            this.Controls.Add(this.mRCFF);
            this.Controls.Add(this.mLCFF);
            this.Controls.Add(this.SCFF);
            this.Controls.Add(this.MCFF);
            this.Controls.Add(this.LCFF);
            this.Controls.Add(this.RCFF);
            this.Controls.Add(this.SCone);
            this.Controls.Add(this.MCone);
            this.Controls.Add(this.LCone);
            this.Controls.Add(this.Rod);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Steuerung";
            this.Text = "Messe Freq.-Empf.-Kurven";
            this.Load += new System.EventHandler(this.Steuerung_Load);
            this.gbProband.ResumeLayout(false);
            this.gbProband.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ergebnisseVorhanden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfTrials)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PredictedThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Rod;
        private System.Windows.Forms.CheckBox LCone;
        private System.Windows.Forms.CheckBox MCone;
        private System.Windows.Forms.CheckBox SCone;
        private System.Windows.Forms.TextBox RCFF;
        private System.Windows.Forms.TextBox LCFF;
        private System.Windows.Forms.TextBox MCFF;
        private System.Windows.Forms.TextBox SCFF;
        private System.Windows.Forms.Button mLCFF;
        private System.Windows.Forms.Button mRCFF;
        private System.Windows.Forms.Button mMCFF;
        private System.Windows.Forms.Button mSCFF;
        private System.Windows.Forms.GroupBox gbProband;
        private System.Windows.Forms.ComboBox cbAugenseite;
        private System.Windows.Forms.Label lblAugenseite;
        private System.Windows.Forms.TextBox tbProbandenNummer;
        private System.Windows.Forms.Label lblProbandenNummer;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mACFF;
        private System.Windows.Forms.TextBox ACFF;
        private System.Windows.Forms.Button result;
        private System.IO.FileSystemWatcher ergebnisseVorhanden;
        private System.Windows.Forms.CheckBox UseDefaultCFF;
        private System.Windows.Forms.Button dateinamen;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox envFreq;
        private System.Windows.Forms.CheckBox pEnv;
        private System.Windows.Forms.Label LabelAlgorithms;
        private System.Windows.Forms.ListBox selectAlgorithm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumberOfTrials;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown PredictedThreshold;
    }
}