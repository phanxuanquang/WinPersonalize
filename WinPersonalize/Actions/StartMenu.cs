using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPersonalize
{
    internal class StartMenu
    {
        private string commandReg, pathReg, nameReg;
        public void ApplyAction(bool TurnOffAppSuggestions_Enable, bool TurnOffRecentApps_Enable, bool ApplyAccentColor_Enable)
        {
            TurnOffAppSuggestions(TurnOffAppSuggestions_Enable);
            TurnOffRecentApps(TurnOffRecentApps_Enable);
            ApplyAccentColor(ApplyAccentColor_Enable);
        }
        void TurnOffAppSuggestions(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";

                    nameReg = "SubscribedContent-338388Enabled";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");

                    nameReg = "SubscribedContent-338388Enabled";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");

                    nameReg = "SubscribedContent-88000237Enabled";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");

                    nameReg = "SubscribedContent-88000326Enabled";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot turn off App Suggesstion.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void TurnOffRecentApps(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

                    nameReg = "ShowRecent";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");

                    nameReg = "ShowFrequent";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot turn off Recent Apps.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ApplyAccentColor(bool enable)
        {
            try
            {
                commandReg = "Set-ItemProperty";
                nameReg = "ColorPrevalence";
                pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";

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
                MessageBox.Show("Cannot show accent color on Start Menu and Taskbar.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
