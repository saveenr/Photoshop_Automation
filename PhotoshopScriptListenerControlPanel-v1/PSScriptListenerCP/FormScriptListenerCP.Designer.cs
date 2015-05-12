namespace PSScriptListenerCP
{
    partial class FormScriptListenerCP
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
            this.buttonCheckStatus = new System.Windows.Forms.Button();
            this.buttonInstall = new System.Windows.Forms.Button();
            this.buttonUninstall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxPSInstalled = new System.Windows.Forms.CheckBox();
            this.checkBoxListenerInstalled = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxListenerAvailable = new System.Windows.Forms.CheckBox();
            this.textBoxPlugInFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPSListenrPlugInName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPSUtilFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPSAUutoPlugInFolder = new System.Windows.Forms.TextBox();
            this.textBoxPSInstallLocation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCheckStatus
            // 
            this.buttonCheckStatus.Location = new System.Drawing.Point(323, 65);
            this.buttonCheckStatus.Name = "buttonCheckStatus";
            this.buttonCheckStatus.Size = new System.Drawing.Size(99, 23);
            this.buttonCheckStatus.TabIndex = 3;
            this.buttonCheckStatus.Text = "Refresh Status";
            this.buttonCheckStatus.UseVisualStyleBackColor = true;
            this.buttonCheckStatus.Click += new System.EventHandler(this.buttonCheckStatus_Click);
            // 
            // buttonInstall
            // 
            this.buttonInstall.Location = new System.Drawing.Point(68, 30);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(75, 23);
            this.buttonInstall.TabIndex = 0;
            this.buttonInstall.Text = "Install Listener as Plug-In";
            this.buttonInstall.UseVisualStyleBackColor = true;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // buttonUninstall
            // 
            this.buttonUninstall.Location = new System.Drawing.Point(278, 30);
            this.buttonUninstall.Name = "buttonUninstall";
            this.buttonUninstall.Size = new System.Drawing.Size(75, 23);
            this.buttonUninstall.TabIndex = 1;
            this.buttonUninstall.Text = "Uninstall";
            this.buttonUninstall.UseVisualStyleBackColor = true;
            this.buttonUninstall.Click += new System.EventHandler(this.buttonUninstall_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Firebrick;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(-3, -3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(460, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Only Works for Photoshop CS2";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(3, 305);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(432, 88);
            this.textBox1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Message Log";
            // 
            // checkBoxPSInstalled
            // 
            this.checkBoxPSInstalled.AutoSize = true;
            this.checkBoxPSInstalled.Enabled = false;
            this.checkBoxPSInstalled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxPSInstalled.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPSInstalled.Name = "checkBoxPSInstalled";
            this.checkBoxPSInstalled.Size = new System.Drawing.Size(116, 17);
            this.checkBoxPSInstalled.TabIndex = 0;
            this.checkBoxPSInstalled.Text = "Photoshop Installed";
            this.checkBoxPSInstalled.UseVisualStyleBackColor = true;
            // 
            // checkBoxListenerInstalled
            // 
            this.checkBoxListenerInstalled.AutoSize = true;
            this.checkBoxListenerInstalled.Enabled = false;
            this.checkBoxListenerInstalled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxListenerInstalled.Location = new System.Drawing.Point(6, 65);
            this.checkBoxListenerInstalled.Name = "checkBoxListenerInstalled";
            this.checkBoxListenerInstalled.Size = new System.Drawing.Size(151, 17);
            this.checkBoxListenerInstalled.TabIndex = 2;
            this.checkBoxListenerInstalled.Text = "Listener installed as Plug-In";
            this.checkBoxListenerInstalled.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxListenerAvailable);
            this.groupBox1.Controls.Add(this.checkBoxPSInstalled);
            this.groupBox1.Controls.Add(this.checkBoxListenerInstalled);
            this.groupBox1.Controls.Add(this.buttonCheckStatus);
            this.groupBox1.Location = new System.Drawing.Point(6, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Installation Status";
            // 
            // checkBoxListenerAvailable
            // 
            this.checkBoxListenerAvailable.AutoSize = true;
            this.checkBoxListenerAvailable.Enabled = false;
            this.checkBoxListenerAvailable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxListenerAvailable.Location = new System.Drawing.Point(6, 42);
            this.checkBoxListenerAvailable.Name = "checkBoxListenerAvailable";
            this.checkBoxListenerAvailable.Size = new System.Drawing.Size(181, 17);
            this.checkBoxListenerAvailable.TabIndex = 1;
            this.checkBoxListenerAvailable.Text = "Listener available in Utilities folder";
            this.checkBoxListenerAvailable.UseVisualStyleBackColor = true;
            // 
            // textBoxPlugInFolder
            // 
            this.textBoxPlugInFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPlugInFolder.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPlugInFolder.Location = new System.Drawing.Point(8, 71);
            this.textBoxPlugInFolder.Name = "textBoxPlugInFolder";
            this.textBoxPlugInFolder.ReadOnly = true;
            this.textBoxPlugInFolder.Size = new System.Drawing.Size(426, 20);
            this.textBoxPlugInFolder.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Photoshop Base Plug-in Folder:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonUninstall);
            this.groupBox2.Controls.Add(this.buttonInstall);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 77);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Installation";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(382, 472);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(8, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(449, 442);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(441, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.textBoxPSListenrPlugInName);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.textBoxPSUtilFolder);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.textBoxPSAUutoPlugInFolder);
            this.tabPage2.Controls.Add(this.textBoxPSInstallLocation);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.textBoxPlugInFolder);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(441, 416);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Name of Listenr Plug-In";
            // 
            // textBoxPSListenrPlugInName
            // 
            this.textBoxPSListenrPlugInName.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxPSListenrPlugInName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPSListenrPlugInName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPSListenrPlugInName.Location = new System.Drawing.Point(9, 236);
            this.textBoxPSListenrPlugInName.Name = "textBoxPSListenrPlugInName";
            this.textBoxPSListenrPlugInName.ReadOnly = true;
            this.textBoxPSListenrPlugInName.Size = new System.Drawing.Size(426, 20);
            this.textBoxPSListenrPlugInName.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Photoshop Utilities Folder";
            // 
            // textBoxPSUtilFolder
            // 
            this.textBoxPSUtilFolder.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxPSUtilFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPSUtilFolder.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPSUtilFolder.Location = new System.Drawing.Point(9, 175);
            this.textBoxPSUtilFolder.Name = "textBoxPSUtilFolder";
            this.textBoxPSUtilFolder.ReadOnly = true;
            this.textBoxPSUtilFolder.Size = new System.Drawing.Size(426, 20);
            this.textBoxPSUtilFolder.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(352, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Photoshop Automation Plug-in Folder (listener copied here when installed)";
            // 
            // textBoxPSAUutoPlugInFolder
            // 
            this.textBoxPSAUutoPlugInFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPSAUutoPlugInFolder.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPSAUutoPlugInFolder.Location = new System.Drawing.Point(8, 123);
            this.textBoxPSAUutoPlugInFolder.Name = "textBoxPSAUutoPlugInFolder";
            this.textBoxPSAUutoPlugInFolder.ReadOnly = true;
            this.textBoxPSAUutoPlugInFolder.Size = new System.Drawing.Size(426, 20);
            this.textBoxPSAUutoPlugInFolder.TabIndex = 5;
            // 
            // textBoxPSInstallLocation
            // 
            this.textBoxPSInstallLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPSInstallLocation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPSInstallLocation.Location = new System.Drawing.Point(9, 19);
            this.textBoxPSInstallLocation.Name = "textBoxPSInstallLocation";
            this.textBoxPSInstallLocation.ReadOnly = true;
            this.textBoxPSInstallLocation.Size = new System.Drawing.Size(426, 20);
            this.textBoxPSInstallLocation.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Photoshop Install Location";
            // 
            // FormScriptListenerCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 498);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FormScriptListenerCP";
            this.Text = "Photoshop Scripting Listener Control Panel";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCheckStatus;
        private System.Windows.Forms.Button buttonInstall;
        private System.Windows.Forms.Button buttonUninstall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxPSInstalled;
        private System.Windows.Forms.CheckBox checkBoxListenerInstalled;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxPlugInFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxListenerAvailable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxPSInstallLocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPSAUutoPlugInFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPSUtilFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPSListenrPlugInName;
    }
}

