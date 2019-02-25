﻿using System;
using System.Reflection;
using System.Windows.Forms;

namespace GuildLounge.TabPages.SettingsPages
{
    public partial class General : UserControl
    {
        private static readonly ApiHandler _api = new ApiHandler();

        public General()
        {
            InitializeComponent();

            labelBuildNumber.Text = "Build: " + Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();

            LoadSettingsGeneral();
            if (checkBoxAutoUpdate.Checked)
                CheckForUpdate();
        }

        public void LoadSettingsGeneral()
        {
            textBoxGameDirectory.Text = Properties.Settings.Default.GameDir;
            textBoxStartParams.Text = Properties.Settings.Default.StartParams;

            switch (Properties.Settings.Default.LaunchBehavior)
            {
                case "STAY_OPEN":
                    radioButtonLaunchStayOpen.Checked = true;
                    break;
                case "MINIMIZE":
                    radioButtonLaunchMinimize.Checked = true;
                    break;
                case "CLOSE":
                    radioButtonLaunchClose.Checked = true;
                    break;
            }

            checkBoxAutoUpdate.Checked = Properties.Settings.Default.CheckForUpdates;
        }

        private void buttonBrowseGameDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == DialogResult.OK)
                textBoxGameDirectory.Text = fbd.SelectedPath;
        }

        private void textBoxGameDirectory_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.GameDir = textBoxGameDirectory.Text;
            if(Parent != null)
                ((Settings)Parent).SettingsChanged();
        }

        private void textBoxStartParams_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartParams = textBoxStartParams.Text;
            if (Parent != null)
                ((Settings)Parent).SettingsChanged();
        }

        private void radioButtonLaunchStayOpen_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LaunchBehavior = "STAY_OPEN";
            if (Parent != null)
                ((Settings)Parent).SettingsChanged();
        }

        private void radioButtonLaunchMinimize_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LaunchBehavior = "MINIMIZE";
            if (Parent != null)
                ((Settings)Parent).SettingsChanged();
        }

        private void radioButtonLaunchClose_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LaunchBehavior = "CLOSE";
            if (Parent != null)
                ((Settings)Parent).SettingsChanged();
        }

        private void checkBoxAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckForUpdates = checkBoxAutoUpdate.Checked;
            if (Parent != null)
                ((Settings)Parent).SettingsChanged();
        }

        private void buttonCheckForUpdates_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private async void CheckForUpdate()
        {
            try
            {
                BuildInfo APIResponse = await _api.GetResponse<BuildInfo>("http://api.guildlounge.com/", "build");

                if(APIResponse.Id > Assembly.GetExecutingAssembly().GetName().Version.Revision)
                {
                    var result = MessageBox.Show("There is a newer version available.\n" +
                        "If you click no you can update from the settings by clicking 'Check Now'.", "Guild Lounge Updater",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                        UpdateExecutable();
                }
            }
            catch (Exception exc)
            {
                labelUpdaterInfo.Text = "Up to date!";
                Utility.TimeoutToDisappear(labelUpdaterInfo);
            }
        }

        private void UpdateExecutable()
        {
            Console.WriteLine("UPDATING KAPPA");
        }
    }
}