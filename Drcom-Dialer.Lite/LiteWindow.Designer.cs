namespace Drcom_Dialer.Lite
{
    partial class LiteWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiteWindow));
            this.userLab = new System.Windows.Forms.Label();
            this.PaswLab = new System.Windows.Forms.Label();
            this.userText = new System.Windows.Forms.TextBox();
            this.paswText = new System.Windows.Forms.TextBox();
            this.dialBtn = new System.Windows.Forms.Button();
            this.settingBtn = new System.Windows.Forms.Button();
            this.aboutBtn = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.remPaswCheckBox = new System.Windows.Forms.CheckBox();
            this.autoLoginCheckBox = new System.Windows.Forms.CheckBox();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // userLab
            // 
            this.userLab.AutoSize = true;
            this.userLab.Location = new System.Drawing.Point(12, 15);
            this.userLab.Name = "userLab";
            this.userLab.Size = new System.Drawing.Size(31, 13);
            this.userLab.TabIndex = 0;
            this.userLab.Text = "账号";
            // 
            // PaswLab
            // 
            this.PaswLab.AutoSize = true;
            this.PaswLab.Location = new System.Drawing.Point(12, 41);
            this.PaswLab.Name = "PaswLab";
            this.PaswLab.Size = new System.Drawing.Size(31, 13);
            this.PaswLab.TabIndex = 1;
            this.PaswLab.Text = "密码";
            // 
            // userText
            // 
            this.userText.Location = new System.Drawing.Point(49, 12);
            this.userText.Name = "userText";
            this.userText.Size = new System.Drawing.Size(223, 20);
            this.userText.TabIndex = 2;
            // 
            // paswText
            // 
            this.paswText.Location = new System.Drawing.Point(49, 38);
            this.paswText.Name = "paswText";
            this.paswText.Size = new System.Drawing.Size(223, 20);
            this.paswText.TabIndex = 3;
            // 
            // dialBtn
            // 
            this.dialBtn.Location = new System.Drawing.Point(12, 87);
            this.dialBtn.Name = "dialBtn";
            this.dialBtn.Size = new System.Drawing.Size(75, 23);
            this.dialBtn.TabIndex = 4;
            this.dialBtn.Text = "拨号";
            this.dialBtn.UseVisualStyleBackColor = true;
            this.dialBtn.Click += new System.EventHandler(this.dialBtn_Click);
            // 
            // settingBtn
            // 
            this.settingBtn.Location = new System.Drawing.Point(106, 87);
            this.settingBtn.Name = "settingBtn";
            this.settingBtn.Size = new System.Drawing.Size(75, 23);
            this.settingBtn.TabIndex = 5;
            this.settingBtn.Text = "设置";
            this.settingBtn.UseVisualStyleBackColor = true;
            this.settingBtn.Click += new System.EventHandler(this.settingBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.Location = new System.Drawing.Point(197, 87);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(75, 23);
            this.aboutBtn.TabIndex = 6;
            this.aboutBtn.Text = "关于";
            this.aboutBtn.UseVisualStyleBackColor = true;
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 118);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(284, 22);
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(137, 17);
            this.statusLabel.Text = "点击关于查看免责声明";
            // 
            // remPaswCheckBox
            // 
            this.remPaswCheckBox.AutoSize = true;
            this.remPaswCheckBox.Location = new System.Drawing.Point(15, 64);
            this.remPaswCheckBox.Name = "remPaswCheckBox";
            this.remPaswCheckBox.Size = new System.Drawing.Size(74, 17);
            this.remPaswCheckBox.TabIndex = 8;
            this.remPaswCheckBox.Text = "记住密码";
            this.remPaswCheckBox.UseVisualStyleBackColor = true;
            this.remPaswCheckBox.CheckedChanged += new System.EventHandler(this.remPaswCheckBox_CheckedChanged);
            // 
            // autoLoginCheckBox
            // 
            this.autoLoginCheckBox.AutoSize = true;
            this.autoLoginCheckBox.Location = new System.Drawing.Point(192, 64);
            this.autoLoginCheckBox.Name = "autoLoginCheckBox";
            this.autoLoginCheckBox.Size = new System.Drawing.Size(74, 17);
            this.autoLoginCheckBox.TabIndex = 9;
            this.autoLoginCheckBox.Text = "自动登陆";
            this.autoLoginCheckBox.UseVisualStyleBackColor = true;
            this.autoLoginCheckBox.CheckedChanged += new System.EventHandler(this.autoLoginCheckBox_CheckedChanged);
            // 
            // LiteWindow
            // 
            this.AcceptButton = this.dialBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 140);
            this.Controls.Add(this.autoLoginCheckBox);
            this.Controls.Add(this.remPaswCheckBox);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.aboutBtn);
            this.Controls.Add(this.settingBtn);
            this.Controls.Add(this.dialBtn);
            this.Controls.Add(this.paswText);
            this.Controls.Add(this.userText);
            this.Controls.Add(this.PaswLab);
            this.Controls.Add(this.userLab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LiteWindow";
            this.Text = "广工Dr.Com";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userLab;
        private System.Windows.Forms.Label PaswLab;
        private System.Windows.Forms.TextBox userText;
        private System.Windows.Forms.TextBox paswText;
        private System.Windows.Forms.Button dialBtn;
        private System.Windows.Forms.Button settingBtn;
        private System.Windows.Forms.Button aboutBtn;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.CheckBox remPaswCheckBox;
        private System.Windows.Forms.CheckBox autoLoginCheckBox;
    }
}

