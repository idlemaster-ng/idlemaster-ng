﻿using System;
using System.Windows.Forms;
using IdleMaster.Properties;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Win32;          //For RegistryKey

namespace IdleMaster
{
  public partial class frmSettings : Form
  {
    public frmSettings()
    {
      InitializeComponent();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (radIdleDefault.Checked)
        {
            Settings.Default.sort = "default";
        }
        if (radIdleLeastDrops.Checked)
        {
            Settings.Default.sort = "leastcards";
        }
        if (radIdleMostDrops.Checked)
        {
            Settings.Default.sort = "mostcards";
        }
        if (radIdleMostValue.Checked)
        {
            Settings.Default.sort = "mostvalue";
        }

        if (cboLanguage.Text != "")
        {
            if (cboLanguage.Text != Settings.Default.language)
            {
                MessageBox.Show(localization.strings.please_restart);
            }
            Settings.Default.language = cboLanguage.Text;
        }

        if (radOneThenMany.Checked)
        {
            Settings.Default.OnlyOneGameIdle = false;
            Settings.Default.OneThenMany = true;
            Settings.Default.AlwaysMany = false;
        }
        else if (radOneGameOnly.Checked)
        {
            Settings.Default.OnlyOneGameIdle = radOneGameOnly.Checked && !radManyThenOne.Checked;
            Settings.Default.OneThenMany = false;
            Settings.Default.AlwaysMany = false;
        }
        else if (radAlwaysMany.Checked)
        {
            Settings.Default.OnlyOneGameIdle = false;
            Settings.Default.OneThenMany = false;
            Settings.Default.AlwaysMany = true;
        }
        else
        {
            Settings.Default.OnlyOneGameIdle = false;
            Settings.Default.OneThenMany = false;
            Settings.Default.AlwaysMany = false;
        }
        Settings.Default.minToTray = chkMinToTray.Checked;
        Settings.Default.ignoreclient = chkIgnoreClientStatus.Checked;
        Settings.Default.showUsername = chkShowUsername.Checked;
        Settings.Default.openLinksInClient = openInClient.Checked;
        setStartOnBoot(chkStartOnBoot.Checked);
        Settings.Default.Save();
        Close();
    }

    private void frmSettings_Load(object sender, EventArgs e)
    {
        if (Settings.Default.language != "")
        {
            cboLanguage.SelectedItem = Settings.Default.language;            
        }
        else
        {
            switch (Thread.CurrentThread.CurrentUICulture.EnglishName)
            {
                case "Chinese (Simplified, China)":
                case "Chinese (Traditional, China)":
                case "Portuguese (Brazil)":
                    cboLanguage.SelectedItem = Thread.CurrentThread.CurrentUICulture.EnglishName;
                    break;
                default:
                    cboLanguage.SelectedItem = Regex.Replace(Thread.CurrentThread.CurrentUICulture.EnglishName, @"\(.+\)", "").Trim();
                    break;
            }
        }
        
        switch (Settings.Default.sort)
        {
            case "leastcards":
                radIdleLeastDrops.Checked = true;
                break;
            case "mostcards":
                radIdleMostDrops.Checked = true;
                break;
            case "mostvalue":
                radIdleMostValue.Checked = true;
                break;
            default:
                break;
        }

        // Load translation
        this.Text = localization.strings.idle_master_settings;
        grpGeneral.Text = localization.strings.general;
        grpIdlingQuantity.Text = localization.strings.idling_behavior;
        grpPriority.Text = localization.strings.idling_order;
        btnOK.Text = localization.strings.accept;
        btnCancel.Text = localization.strings.cancel;
        ttHints.SetToolTip(btnAdvanced, localization.strings.advanced_auth);
        chkMinToTray.Text = localization.strings.minimize_to_tray;
        ttHints.SetToolTip(chkMinToTray, localization.strings.minimize_to_tray);
        chkIgnoreClientStatus.Text = localization.strings.ignore_client_status;
        ttHints.SetToolTip(chkIgnoreClientStatus, localization.strings.ignore_client_status);
        chkShowUsername.Text = localization.strings.show_username;
        ttHints.SetToolTip(chkShowUsername, localization.strings.show_username);
        radAlwaysMany.Text = localization.strings.idle_alwaysmany;
        ttHints.SetToolTip(radAlwaysMany, localization.strings.idle_alwaysmany);
        radOneGameOnly.Text = localization.strings.idle_individual;
        ttHints.SetToolTip(radOneGameOnly, localization.strings.idle_individual);
        radManyThenOne.Text = localization.strings.idle_simultaneous;
        ttHints.SetToolTip(radManyThenOne, localization.strings.idle_simultaneous);
        radOneThenMany.Text = localization.strings.idle_onethenmany;
        ttHints.SetToolTip(radOneThenMany, localization.strings.idle_onethenmany);
        radIdleDefault.Text = localization.strings.order_default;
        ttHints.SetToolTip(radIdleDefault, localization.strings.order_default);
        radIdleMostValue.Text = localization.strings.order_value;
        ttHints.SetToolTip(radIdleMostValue, localization.strings.order_value);
        radIdleMostDrops.Text = localization.strings.order_most;
        ttHints.SetToolTip(radIdleMostDrops, localization.strings.order_most);
        radIdleLeastDrops.Text = localization.strings.order_least;
        ttHints.SetToolTip(radIdleLeastDrops, localization.strings.order_least);
        chkStartOnBoot.Text = localization.strings.start_on_boot;
        ttHints.SetToolTip(chkStartOnBoot, localization.strings.start_on_boot);
        openInClient.Text = localization.strings.open_links_in_client;
        ttHints.SetToolTip(openInClient, localization.strings.open_links_in_client);
        ttHints.SetToolTip(buttonHelpStartOnBoot, localization.strings.start_on_boot_hint);
        lblLanguage.Text = localization.strings.interface_language;

        if (Settings.Default.OneThenMany)
        {
            radOneThenMany.Checked = true;
        }
        else if (Settings.Default.AlwaysMany)
        {
            radAlwaysMany.Checked = true;
        }
        else
        {
            radOneGameOnly.Checked = Settings.Default.OnlyOneGameIdle;
            radManyThenOne.Checked = !Settings.Default.OnlyOneGameIdle;
        }        

        if (Settings.Default.minToTray)
        {
            chkMinToTray.Checked = true;
        }

        if (Settings.Default.ignoreclient)
        {
            chkIgnoreClientStatus.Checked = true;
        }

        if (Settings.Default.showUsername)
        {
            chkShowUsername.Checked = true;
        }
        
        if (getSteamStartOnBoot())
        {
            chkStartOnBoot.Enabled = true;
            buttonHelpStartOnBoot.Enabled = false;
            buttonHelpStartOnBoot.Visible = false;
        }
        else
        {
            //Check if user did not disable start steam on boot
            if (getStartOnBoot()) setStartOnBoot(false);
            chkStartOnBoot.Enabled = false;
            buttonHelpStartOnBoot.Enabled = true;
            buttonHelpStartOnBoot.Visible = true;
        }
        if (getStartOnBoot())
        {
            chkStartOnBoot.Checked = true;
        }
        openInClient.Checked = Settings.Default.openLinksInClient;
    }

    private void btnAdvanced_Click(object sender, EventArgs e)
    {
      var frm = new frmSettingsAdvanced();
      frm.ShowDialog();
    }

    private void setStartOnBoot(bool value)
    {
       RegistryKey registryRunAtStartup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
       if (chkStartOnBoot.Checked)
       {
          registryRunAtStartup.SetValue("IdleMaster", Application.ExecutablePath.ToString());
       }
       else if(registryRunAtStartup.GetValue("IdleMaster")!=null)
       {
          registryRunAtStartup.DeleteValue("IdleMaster", false);
       }
    }
        private bool getStartOnBoot()
        {
            RegistryKey registryRunAtStartup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (registryRunAtStartup.GetValue("IdleMaster") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool getSteamStartOnBoot()
        {
            RegistryKey registryRunAtStartup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (registryRunAtStartup.GetValue("Steam") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void buttonHelpStartOnBoot_MouseEnter(object sender, EventArgs e)
        {
            
        }
    }
}
