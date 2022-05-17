using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPersonalize
{
    internal class Personalize
    {
        private string commandReg, pathReg, nameReg;
        public enum DesktopIconSize { small, medium, large }
        public DesktopIconSize desktopIconSize;
        public void ApplyAction(bool DarkMode_Enable, bool TransparentEffect_Enable, bool PersonalizeDesktopIconArrange_Auto_Enable, double scaleRatio, bool AccentColor_Enable, bool EnableChangeResolutionScale)
        {
            Change_DesktopIconSize();

            DarkMode_Activate(DarkMode_Enable);
            AccentColor(AccentColor_Enable);
            TransparentEffect(TransparentEffect_Enable);
            PersonalizeDesktopIconArrange_Auto(PersonalizeDesktopIconArrange_Auto_Enable);
            Set_DesktopResolution(EnableChangeResolutionScale);
            Set_DesktopScale(scaleRatio, EnableChangeResolutionScale);
        }
        private void DarkMode_Activate(bool enable)
        {
            try
            {
                commandReg = "Set-ItemProperty";
                pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                nameReg = "AppsUseLightTheme";

                if (enable == true)
                {
                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 0");
                }
                else
                {
                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot change Windows Mode.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AccentColor(bool enable)
        {
            try
            {
                commandReg = "Set-ItemProperty";
                nameReg = "ColorPrevalence";
                pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\DWM";

                if (enable == true)
                {
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 1");
                }
                else
                {
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot apply Accent Color.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TransparentEffect(bool enable)
        {
            try
            {
                commandReg = "Set-ItemProperty";
                pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                nameReg = "EnableTransparency";

                if (enable == true)
                {
                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 1");
                }
                else
                {
                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot apply Transparent effect.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Change_DesktopIconSize()
        {
            try
            {
                commandReg = "Set-ItemProperty";
                pathReg = @"HKCU:\Software\Microsoft\Windows\Shell\Bags\1\Desktop";
                nameReg = "IconSize";

                switch (desktopIconSize)
                {
                    case DesktopIconSize.small:
                        Program.runCommand(commandReg, pathReg, nameReg, "-Value 20");
                        break;
                    case DesktopIconSize.medium:
                        Program.runCommand(commandReg, pathReg, nameReg, "-Value 30");
                        break;
                    case DesktopIconSize.large:
                        Program.runCommand(commandReg, pathReg, nameReg, "-Value 60");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot change Desktop icon size.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PersonalizeDesktopIconArrange_Auto(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKCU:\Software\Microsoft\Windows\Shell\Bags\1\Desktop";
                    nameReg = "FFlags";

                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 1075839525");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot arrange Desktop icons.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public double width;
        public int height;
        private void Set_DesktopResolution(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    string command = String.Format("Set-DisplayResolution -Width {0} -Height {1} -Force", width, height);
                    Program.runCommand_Advanced(command);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot adjust Resolution.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Set_DesktopScale(double ratio, bool enable)
        {
            try
            {
                var DPI = ratio / 100 * 96;

                commandReg = "Set-ItemProperty";
                pathReg = @"HKCU:\Control Panel\Desktop\WindowMetrics";

                nameReg = "Win8DpiScaling";
                Program.runCommand(commandReg, pathReg, nameReg, "-Value 1");

                nameReg = "LogPixels";
                Program.runCommand(commandReg, pathReg, nameReg, "-Value " + DPI.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot adjust monitor scale.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
