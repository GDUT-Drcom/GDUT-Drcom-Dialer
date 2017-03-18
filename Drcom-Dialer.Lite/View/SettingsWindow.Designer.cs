namespace Drcom_Dialer
{
    partial class SettingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingWindow));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.zoneComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.updateCheckBox = new System.Windows.Forms.CheckBox();
            this.reminderCheckBox = new System.Windows.Forms.CheckBox();
            this.vpnCheckBox = new System.Windows.Forms.CheckBox();
            this.redialCheckBox = new System.Windows.Forms.CheckBox();
            this.startupCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.projCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.zoneComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "校区选择";
            // 
            // zoneComboBox
            // 
            this.zoneComboBox.FormattingEnabled = true;
            this.zoneComboBox.Items.AddRange(new object[] {
            "大学城",
            "龙洞",
            "东风路",
            "番禺"});
            this.zoneComboBox.Location = new System.Drawing.Point(6, 19);
            this.zoneComboBox.Name = "zoneComboBox";
            this.zoneComboBox.Size = new System.Drawing.Size(121, 21);
            this.zoneComboBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.updateCheckBox);
            this.groupBox2.Controls.Add(this.reminderCheckBox);
            this.groupBox2.Controls.Add(this.vpnCheckBox);
            this.groupBox2.Controls.Add(this.redialCheckBox);
            this.groupBox2.Controls.Add(this.startupCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(133, 147);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "功能项";
            // 
            // updateCheckBox
            // 
            this.updateCheckBox.AutoSize = true;
            this.updateCheckBox.Location = new System.Drawing.Point(6, 111);
            this.updateCheckBox.Name = "updateCheckBox";
            this.updateCheckBox.Size = new System.Drawing.Size(74, 17);
            this.updateCheckBox.TabIndex = 4;
            this.updateCheckBox.Text = "自动更新";
            this.updateCheckBox.UseVisualStyleBackColor = true;
            // 
            // reminderCheckBox
            // 
            this.reminderCheckBox.AutoSize = true;
            this.reminderCheckBox.Location = new System.Drawing.Point(6, 88);
            this.reminderCheckBox.Name = "reminderCheckBox";
            this.reminderCheckBox.Size = new System.Drawing.Size(86, 17);
            this.reminderCheckBox.TabIndex = 3;
            this.reminderCheckBox.Text = "欠费前提醒";
            this.reminderCheckBox.UseVisualStyleBackColor = true;
            // 
            // vpnCheckBox
            // 
            this.vpnCheckBox.AutoSize = true;
            this.vpnCheckBox.Location = new System.Drawing.Point(6, 65);
            this.vpnCheckBox.Name = "vpnCheckBox";
            this.vpnCheckBox.Size = new System.Drawing.Size(72, 17);
            this.vpnCheckBox.TabIndex = 2;
            this.vpnCheckBox.Text = "修复VPN";
            this.vpnCheckBox.UseVisualStyleBackColor = true;
            // 
            // redialCheckBox
            // 
            this.redialCheckBox.AutoSize = true;
            this.redialCheckBox.Location = new System.Drawing.Point(6, 42);
            this.redialCheckBox.Name = "redialCheckBox";
            this.redialCheckBox.Size = new System.Drawing.Size(74, 17);
            this.redialCheckBox.TabIndex = 1;
            this.redialCheckBox.Text = "断线重连";
            this.redialCheckBox.UseVisualStyleBackColor = true;
            // 
            // startupCheckBox
            // 
            this.startupCheckBox.AutoSize = true;
            this.startupCheckBox.Location = new System.Drawing.Point(6, 19);
            this.startupCheckBox.Name = "startupCheckBox";
            this.startupCheckBox.Size = new System.Drawing.Size(74, 17);
            this.startupCheckBox.TabIndex = 0;
            this.startupCheckBox.Text = "开机启动";
            this.startupCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.projCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(134, 55);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "其他";
            // 
            // projCheckBox
            // 
            this.projCheckBox.AutoSize = true;
            this.projCheckBox.Location = new System.Drawing.Point(6, 19);
            this.projCheckBox.Name = "projCheckBox";
            this.projCheckBox.Size = new System.Drawing.Size(122, 17);
            this.projCheckBox.TabIndex = 0;
            this.projCheckBox.Text = "用户体验改善计划";
            this.projCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(156, 297);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingWindow";
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox zoneComboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox updateCheckBox;
        private System.Windows.Forms.CheckBox reminderCheckBox;
        private System.Windows.Forms.CheckBox vpnCheckBox;
        private System.Windows.Forms.CheckBox redialCheckBox;
        private System.Windows.Forms.CheckBox startupCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox projCheckBox;
    }
}