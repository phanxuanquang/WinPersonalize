using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPersonalize
{
    internal class Taskbar
    {
        private string commandReg, pathReg, nameReg;
        public enum aligin { left, center, right }
        public enum size { medium, small }
        public enum state { normal, transparent };
        public void ApplyAction(bool TaskbarAlign_Enable, bool TaskbarSize_Enable, bool SmallSearchIcon_Enable, bool HideTaskViewIcon_Enable, bool TurnOffMeetNow_Enable, bool RemoveCortanaIcon_Enable, bool RemoveBingWeather_Enable, bool HideMSStoreIcon_Enable)
        {
            TaskbarAlign(TaskbarAlign_Enable);
            TaskbarSize(TaskbarSize_Enable);

            SmallSearchIcon(SmallSearchIcon_Enable);
            HideTaskViewIcon(HideTaskViewIcon_Enable);
            TurnOffMeetNow(TurnOffMeetNow_Enable);
            RemoveCortanaIcon(RemoveCortanaIcon_Enable);
            RemoveBingWeather(RemoveBingWeather_Enable);
            HideMSStoreIcon(HideMSStoreIcon_Enable);
        }

        private void TaskbarAlign(bool enable)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "CMD.exe";
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;

                if (enable == true)
                {
                    try
                    {
                        p.StartInfo.Arguments = "/C taskkill /F /IM \"TranslucentTB.exe\" /T";
                        p.Start();
                        p.StartInfo.Arguments = "/C Translucent.exe /norestart /sp- /verysilent /allusers";
                        p.Start();
                    }
                    catch { }

                }
                else
                {
                    p.StartInfo.Arguments = "/C taskkill /F /IM \"TranslucentTB.exe\" /T";
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot apply transparent effect to the Taskbar.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TaskbarSize(bool enable)
        {
            try
            {
                commandReg = "Set-ItemProperty";
                pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
                nameReg = "TaskbarSmallIcons";

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
                MessageBox.Show("Cannot change Taskbar icon size.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SmallSearchIcon(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";
                    nameReg = "SearchboxTaskbarMode";

                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot change Search icon size.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HideTaskViewIcon(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
                    nameReg = "ShowTaskViewButton";

                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot change hide Task view icon.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TurnOffMeetNow(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
                    nameReg = "HideSCAMeetNow";

                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot turn off Meet now.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveCortanaIcon(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
                    nameReg = "ShowCortanaButton";

                    Program.runCommand(commandReg, pathReg, nameReg, "-Type DWord -Value 0");

                    Program.runCommand_Advanced("Get-AppxPackage -allusers Microsoft.549981C3F5F10 | Remove-AppxPackage");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot remove Cortana icon from the Taskbar.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveBingWeather(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    Program.runCommand_Advanced("Get-AppxPackage *bingweather* | Remove-AppxPackage.");

                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Feeds";

                    nameReg = "ShellFeedsTaskbarViewMode";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 2");

                    nameReg = "IsFeedsAvailable";
                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot remove News and Interests widget from the Taskbar.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HideMSStoreIcon(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    commandReg = "Set-ItemProperty";
                    pathReg = @"HKLM:\SOFTWARE\Policies\Microsoft\Windows\Explorer";
                    nameReg = "NoPinningStoreToTaskbar";

                    Program.runCommand(commandReg, pathReg, nameReg, "-Value 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot hide Microsoft Store icon.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
