namespace Guppy_Simple
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
            this.components = new System.ComponentModel.Container();
            this.tmrTick = new System.Windows.Forms.Timer(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblGupInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTogglePause = new System.Windows.Forms.Button();
            this.lblBrainInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numUDGenTime = new System.Windows.Forms.NumericUpDown();
            this.chkRelatives = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDGenTime)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrTick
            // 
            this.tmrTick.Interval = 1;
            this.tmrTick.Tick += new System.EventHandler(this.tmrTick_Tick);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Location = new System.Drawing.Point(-4, 24);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(82, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "INFORMATION";
            // 
            // lblGupInfo
            // 
            this.lblGupInfo.AutoSize = true;
            this.lblGupInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblGupInfo.ForeColor = System.Drawing.Color.White;
            this.lblGupInfo.Location = new System.Drawing.Point(2, 71);
            this.lblGupInfo.Name = "lblGupInfo";
            this.lblGupInfo.Size = new System.Drawing.Size(125, 52);
            this.lblGupInfo.TabIndex = 1;
            this.lblGupInfo.Text = "Guppy Number:\r\nNumber of pellets eaten: \r\nX position:\r\nY position: ";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnTogglePause);
            this.panel1.Controls.Add(this.lblBrainInfo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numUDGenTime);
            this.panel1.Controls.Add(this.chkRelatives);
            this.panel1.Controls.Add(this.lblGupInfo);
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 850);
            this.panel1.TabIndex = 2;
            // 
            // btnTogglePause
            // 
            this.btnTogglePause.Location = new System.Drawing.Point(30, 3);
            this.btnTogglePause.Name = "btnTogglePause";
            this.btnTogglePause.Size = new System.Drawing.Size(75, 20);
            this.btnTogglePause.TabIndex = 7;
            this.btnTogglePause.Text = "Start";
            this.btnTogglePause.UseVisualStyleBackColor = true;
            this.btnTogglePause.Click += new System.EventHandler(this.btnTogglePause_Click);
            // 
            // lblBrainInfo
            // 
            this.lblBrainInfo.BackColor = System.Drawing.Color.Black;
            this.lblBrainInfo.ForeColor = System.Drawing.Color.White;
            this.lblBrainInfo.Location = new System.Drawing.Point(-1, 192);
            this.lblBrainInfo.Multiline = true;
            this.lblBrainInfo.Name = "lblBrainInfo";
            this.lblBrainInfo.ReadOnly = true;
            this.lblBrainInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblBrainInfo.Size = new System.Drawing.Size(140, 529);
            this.lblBrainInfo.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Time Between Generations:";
            // 
            // numUDGenTime
            // 
            this.numUDGenTime.BackColor = System.Drawing.Color.Black;
            this.numUDGenTime.ForeColor = System.Drawing.Color.White;
            this.numUDGenTime.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numUDGenTime.Location = new System.Drawing.Point(6, 165);
            this.numUDGenTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numUDGenTime.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numUDGenTime.Name = "numUDGenTime";
            this.numUDGenTime.Size = new System.Drawing.Size(120, 20);
            this.numUDGenTime.TabIndex = 4;
            this.numUDGenTime.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // chkRelatives
            // 
            this.chkRelatives.AutoSize = true;
            this.chkRelatives.ForeColor = System.Drawing.Color.White;
            this.chkRelatives.Location = new System.Drawing.Point(5, 126);
            this.chkRelatives.Name = "chkRelatives";
            this.chkRelatives.Size = new System.Drawing.Size(100, 17);
            this.chkRelatives.TabIndex = 3;
            this.chkRelatives.Text = "Show Relatives";
            this.chkRelatives.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(613, 610);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Guppies!";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDGenTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrTick;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblGupInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkRelatives;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numUDGenTime;
        private System.Windows.Forms.TextBox lblBrainInfo;
        private System.Windows.Forms.Button btnTogglePause;
    }
}

