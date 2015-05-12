using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PSScriptListenerCP
{
    public partial class FormScriptListenerCP : Form
    {
        string photoshop_cs2_guid = "{236BB7C4-4419-42FD-0409-1E257A25E34D}";
        string photoshop_cs2_listener_fname = "ScriptListener.8li";

        public FormScriptListenerCP()
        {
            InitializeComponent();
            this.ExamineInstallationStatus();
        }

        private void buttonCheckStatus_Click(object sender, EventArgs e)
        {
            this.ExamineInstallationStatus();
        }

        private void ExamineInstallationStatus()
        {
            AppARPInfo info = this.ARP_Find_Uninstall_GUID(photoshop_cs2_guid);

            if (info==null)
            {
                this.checkBoxPSInstalled.Checked = false;
                return;
            }
            this.checkBoxPSInstalled.Checked = true;

            string installed_plugin_location = this.get_plugin_installed_location(info);
            string src_plugin_location = this.get_plugin_src_location(info);

            bool plug_in_installed = System.IO.File.Exists(installed_plugin_location);
            bool plug_in_available = System.IO.File.Exists(src_plugin_location);

            this.checkBoxListenerInstalled.Checked = plug_in_installed;
            this.checkBoxListenerAvailable.Checked = plug_in_available;
            this.textBoxPlugInFolder.Text = this.get_plugin_folder(info);

            this.textBoxPSInstallLocation.Text = info.InstallLocation;
            this.textBoxPSAUutoPlugInFolder.Text = this.get_plugin_auto_folder(info);
            this.textBoxPSUtilFolder.Text = this.get_utilities_location(info);
            this.textBoxPSListenrPlugInName.Text = this.photoshop_cs2_listener_fname;

            //Enable disable buttons accordingly
            this.buttonInstall.Enabled = (!plug_in_installed ) && (plug_in_available) ;
            this.buttonUninstall.Enabled = (plug_in_installed) && (plug_in_available) ;

        }

        private void ClearStatus()
        {
            this.textBox1.Clear();
        }

        private string get_plugin_src_location(AppARPInfo info)
        {
            string src_plugin_location = System.IO.Path.Combine(info.InstallLocation, this.get_utilities_location(info) );
            src_plugin_location = System.IO.Path.Combine(src_plugin_location, photoshop_cs2_listener_fname);
            return src_plugin_location;
        }

        private string get_utilities_location(AppARPInfo info)
        {
            string s= System.IO.Path.Combine(info.InstallLocation, @"Scripting Guide\Utilities" );
            return s;
        }

        private string get_plugin_auto_folder(AppARPInfo info)
        {
            string s = System.IO.Path.Combine( this.get_plugin_folder(info), @"Adobe Photoshop Only\Automate" );
            return s;
        }

        private string get_plugin_installed_location(AppARPInfo info)
        {
            string installed_plugin_location = System.IO.Path.Combine(info.InstallLocation, this.get_plugin_auto_folder(info) );
            installed_plugin_location = System.IO.Path.Combine(installed_plugin_location , photoshop_cs2_listener_fname);
            return installed_plugin_location;
        }

        private string get_plugin_folder(AppARPInfo info)
        {
            string s= System.IO.Path.Combine(info.InstallLocation, @"Plug-Ins");
            return s;
        }

        private void WriteStatus(string fmt, params object[] a)
        {
            string s = string.Format(fmt, a);
            this.textBox1.AppendText(s + "\r\n");
        }


        public AppARPInfo ARP_Find_Uninstall_GUID( string app_guid )
        {
            //normalize the guid
            app_guid = app_guid.ToLower();
            string regpath_uninstall_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            AppARPInfo app_info = null;
            bool app_found = false;

            Microsoft.Win32.RegistryKey hk_uninstall = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regpath_uninstall_key);

            foreach (string subkey in hk_uninstall.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey hk_app = hk_uninstall.OpenSubKey(subkey);
                string uninstall_string = this.get_string( hk_app, "UninstallString" );
                
                if (uninstall_string == null) 
                { 
                    // there was no uninstall string, so keep searching
                    continue; 
                }

                if (uninstall_string.ToLower().Contains(app_guid))
                {
                    // there was an uninstall string and it contained the guid we are looking for

                    app_found = true;

                    app_info = new AppARPInfo();
                    app_info.UninstallString = uninstall_string;
                    app_info.DisplayName = this.get_string(hk_app, "DisplayName");
                    app_info.InstallLocation = this.get_string(hk_app, "InstallLocation");
                    app_info.Publisher = this.get_string(hk_app, "Publisher");
                    app_info.DisplayVersion = this.get_string(hk_app, "DisplayVersion");
                }

                
                hk_app.Close();

                if (app_found)
                {
                    // if we did find the app, then quit searching through 
                    break;
                }

            }


            hk_uninstall.Close();

            return app_info;

        }

        private string get_string(Microsoft.Win32.RegistryKey hk_app,string name)
        {
            object val = hk_app.GetValue(name, null);
            
            if (val == null) 
            { 
                return null;  
            }
            else if (val.GetType() == typeof(string)) { 
                return (string)val; 
            }
            else
            {
                throw new Exception();
            }
        }

        private void install()
        {
            this.ClearStatus();
            this.WriteStatus("Installing Plug-In...");

            AppARPInfo info = this.ARP_Find_Uninstall_GUID(photoshop_cs2_guid);

            if (info == null)
            {
                this.WriteStatus("Photoshop CS2 is not installed");
                return;
            }

            string src_plugin_location = this.get_plugin_src_location(info);
            string installed_plugin_location = this.get_plugin_installed_location(info);

            if (System.IO.File.Exists(installed_plugin_location))
            {
                this.WriteStatus("The plug in is already installed");
                return;
            }

            if (!System.IO.File.Exists(src_plugin_location))
            {
                this.WriteStatus("The plug in is not on disk");
                this.WriteStatus("Listener cannot be installed.");
                return;
            }

            this.WriteStatus("Copying Listener into Plug-In folder");
            this.WriteStatus(" From: {0}", src_plugin_location);
            this.WriteStatus(" To: {0}", installed_plugin_location);
            try
            {
                System.IO.File.Copy(src_plugin_location, installed_plugin_location);
            }
            catch (System.IO.IOException exc)
            {
                this.WriteStatus("IO Failure during Copy.");
                this.WriteStatus("Failed to install.");
                return;
            }

            this.WriteStatus("Listener installed.");


        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            this.install();
            this.ExamineInstallationStatus();
        }

        private void uninstall()
        {
            this.ClearStatus();
            this.WriteStatus("Uninstalling Plug-In...");
            AppARPInfo info = this.ARP_Find_Uninstall_GUID(photoshop_cs2_guid);

            if (info == null)
            {
                this.WriteStatus("Photoshop CS2 is not installed");
                return;
            }

            string src_plugin_location = this.get_plugin_src_location(info);
            string installed_plugin_location = this.get_plugin_installed_location(info);

            if (!System.IO.File.Exists(installed_plugin_location))
            {
                this.WriteStatus("The plug in is not installed.");
                return;
            }

            if (!System.IO.File.Exists(src_plugin_location))
            {
                this.WriteStatus("The original listener file is not in the utilities folder");
                this.WriteStatus("Aborting removal. Listener not removed.");
                return;
            }

            this.WriteStatus("Removing Listener from Plug-In folder");
            this.WriteStatus(" File to delete: {0}", installed_plugin_location );
            try
            {
                System.IO.File.Delete(installed_plugin_location);
            }
            catch (System.IO.IOException exc)
            {
                this.WriteStatus("IO Failure during Delete.");
                this.WriteStatus("Failed to remove plug-in file.");
                this.WriteStatus("Common Cause: Photoshop is running.");
                return;
            }

            this.WriteStatus("Listener removed.");


        }

        private void buttonUninstall_Click(object sender, EventArgs e)
        {

            this.uninstall();
            this.ExamineInstallationStatus();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }

    public class AppARPInfo
    {
        public string UninstallString;
        public string DisplayName;
        public string InstallLocation;
        public string Publisher;
        public string DisplayVersion;
    }
}