using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPersonalize
{
    public partial class MainUI : Form
    {
        public MainUI()
        {
            InitializeComponent();
            ShadowForm.SetShadowForm(this);
            this.Icon = Properties.Resources.icon;

            ApplyTheme();

            BrightnessTrackBar.Value = get_CurrentBrightnessLevel();
            ajustResolutionComboBox();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }
        private void ApplyTheme()
        {
            var themeColor = WindowsColor.GetAccentColor();
            colorSample.FillColor = themeColor;
            ApplyButton.FillColor = ApplyButton.FillColor2 = themeColor;
            ApplyButton.HoverState.FillColor = ApplyButton.HoverState.FillColor2 = BrightnessTrackBar.ThumbColor = ControlPaint.Light(themeColor);
            Banner.BackColor = WindowsColor.GetAccentColor();
            Banner.FillColor = Banner.FillColor2 = ControlPaint.Light(WindowsColor.GetAccentColor());
            Program.ApplyThemeColor_CheckButtons(this);
        }

        public void UpdateBrightnessValue()
        {
            BrightnessTrackBar.Value = get_CurrentBrightnessLevel();
            BrightnessPercent.Text = String.Format("Brightness: {0}%", BrightnessTrackBar.Value.ToString());
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            try
            {
                Taskbar taskbarActions = new Taskbar();
                StartMenu startMenuActions = new StartMenu();
                Personalize personalizeActions = new Personalize();

                if (PersonalizeDesktopIconSize_Small.Checked)
                {
                    personalizeActions.desktopIconSize = Personalize.DesktopIconSize.small;
                }
                else if (PersonalizeDesktopIconSize_Medium.Checked)
                {
                    personalizeActions.desktopIconSize = Personalize.DesktopIconSize.medium;
                }
                else
                {
                    personalizeActions.desktopIconSize = Personalize.DesktopIconSize.large;
                }

                double scrRatioHW = Screen.PrimaryScreen.Bounds.Height / Screen.PrimaryScreen.Bounds.Width;
                double tempHeight = 0;
                switch (Display_ResolutionComboBox.SelectedItem)
                {
                    case 0:
                        personalizeActions.width = 1366;
                        tempHeight = personalizeActions.width * scrRatioHW;
                        personalizeActions.height = (int)tempHeight;
                        break;
                    case 1:
                        personalizeActions.width = 1920;
                        tempHeight = personalizeActions.width * scrRatioHW;
                        personalizeActions.height = (int)tempHeight;
                        break;
                    case 2:
                        personalizeActions.width = 2560;
                        tempHeight = personalizeActions.width * scrRatioHW;
                        personalizeActions.height = (int)tempHeight;
                        break;
                    case 3:
                        personalizeActions.width = 3840;
                        tempHeight = personalizeActions.width * scrRatioHW;
                        personalizeActions.height = (int)tempHeight;
                        break;
                }

                DialogResult confirmation = MessageBox.Show("Do you want to continue?", "CONFIRMATION", MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    Task applyChanges_Taskbar = Task.Factory.StartNew(() => taskbarActions.ApplyAction(
                            TaskbarAlign_Center.Checked,
                            TaskbarSize_Small.Checked,
                            SmallSearchIcon.Checked,
                            HideTaskViewIcon.Checked,
                            TurnOffMeetNow.Checked,
                            RemoveCortanaIcon.Checked,
                            RemoveBingWeather.Checked,
                            HideMSStoreIcon.Checked)
                    );

                    Task applyChanges_StartMenu = Task.Factory.StartNew(() => startMenuActions.ApplyAction(
                            TurnOffAppSuggestions.Checked,
                            TurnOffRecentApps.Checked,
                            ApplyAccentColor.Checked)
                    );

                    double scaleRatio = double.Parse(Display_ZoomComboBox.Text);
                    Task applyChanges_Personalize = Task.Factory.StartNew(() => personalizeActions.ApplyAction(
                            PersonalizeColorMode_Dark.Checked,
                            PersonalizeTransparentEffect_Enable.Checked,
                            PersonalizeDesktopIconArrange_Auto.Checked,
                            scaleRatio,
                            PersonalizeAccentColor_Enable.Checked,
                            EnableChangeResolutionScale.Checked)
                    );

                    Task.WaitAll(applyChanges_Taskbar, applyChanges_StartMenu, applyChanges_Personalize);

                    Task restartExplorer = Task.Factory.StartNew(() => Program.runCommand_Advanced("stop-process -name explorer –force"));
                    restartExplorer.Wait();

                    Thread.Sleep(1000);

                    DialogResult dialogResult = MessageBox.Show("To apply changes completely. You need to restart your computer.\nRestart now?", "Restart computer", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Program.runCommand_Advanced("Restart-Computer -Force");
                    }
                    else
                    {
                        MessageBox.Show("Remember to restart your computer to apply changes completely.", "Reccomendation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot apply changes.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            TaskbarAlign_Center.Checked = TaskbarSize_Small.Checked = PersonalizeColorMode_Dark.Checked = PersonalizeTransparentEffect_Enable.Checked = PersonalizeAccentColor_Enable.Checked = PersonalizeDesktopIconSize_Small.Checked = true;
            Program.CheckAll_CheckBox(this);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EnableChangeResolutionScale_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableChangeResolutionScale.Checked)
            {
                Display_ResolutionComboBox.Enabled = Display_ZoomComboBox.Enabled = true;
            }
            else
            {
                Display_ResolutionComboBox.Enabled = Display_ZoomComboBox.Enabled = false;
            }
        }

        private void ajustResolutionComboBox()
        {
            try
            {
                if (Screen.PrimaryScreen.Bounds.Width < 1920)
                {
                    Display_ResolutionComboBox.Items.Remove("Full HD");
                    Display_ResolutionComboBox.Items.Remove("2K");
                    Display_ResolutionComboBox.Items.Remove("4K");
                }
                else if (Screen.PrimaryScreen.Bounds.Width < 2560)
                {
                    Display_ResolutionComboBox.Items.Remove("2K");
                    Display_ResolutionComboBox.Items.Remove("4K");
                }
                else if (Screen.PrimaryScreen.Bounds.Width < 3840)
                {
                    Display_ResolutionComboBox.Items.Remove("4K");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot adjust Resolution combo box.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Change brightness directly
        int get_CurrentBrightnessLevel()
        {
            try
            {
                var mclass = new ManagementClass("WmiMonitorBrightness")
                {
                    Scope = new ManagementScope(@"\\.\root\wmi")
                };

                foreach (ManagementObject instance in mclass.GetInstances())
                {
                    return (byte)instance["CurrentBrightness"];
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot identify brightness level.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }
        void set_Brightness(int level)
        {
            try
            {
                Program.runCommand_Advanced(String.Format("(Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods).WmiSetBrightness(1,{0})", level));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot change brightness.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BrightnessTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            set_Brightness(BrightnessTrackBar.Value);
            BrightnessPercent.Text = String.Format("Brightness: {0}%", BrightnessTrackBar.Value.ToString());
        }
        #endregion
    }
}
