using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WDSSRP
{
    public partial class MainForm : Form
    {
        private readonly string DesktopSettingsRegistryPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\Shell\\Bags\\1\\Desktop";

        private readonly string SavedProfilesDirectory = "Profiles";

        private DesktopProfile dp;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private DesktopProfile ReadSettings()
        {
            DesktopProfile dp = new DesktopProfile();
            dp.FFlags = Registry.GetValue(DesktopSettingsRegistryPath, "FFlags", null) as int?;
            dp.GroupByDirection = Registry.GetValue(DesktopSettingsRegistryPath, "GroupByDirection", null) as int?;
            dp.GroupByKey_FMTID = Registry.GetValue(DesktopSettingsRegistryPath, "GroupByKey:FMTID", null) as string;
            dp.GroupByKey_PID = Registry.GetValue(DesktopSettingsRegistryPath, "GroupByKey:PID", null) as int?;
            dp.GroupView = Registry.GetValue(DesktopSettingsRegistryPath, "GroupView", null) as int?;
            dp.IconLayouts = (byte[]) Registry.GetValue(DesktopSettingsRegistryPath, "IconLayouts", null);
            dp.IconSize = Registry.GetValue(DesktopSettingsRegistryPath, "IconSize", null) as int?;
            dp.LogicalViewMode = Registry.GetValue(DesktopSettingsRegistryPath, "LogicalViewMode", null) as int?;
            dp.Mode = Registry.GetValue(DesktopSettingsRegistryPath, "Mode", null) as int?;
            dp.Sort = (byte[]) Registry.GetValue(DesktopSettingsRegistryPath, "Sort", null);
            return dp;
        }

        private void WriteSettings(DesktopProfile profile)
        {
            Registry.SetValue(DesktopSettingsRegistryPath, "FFlags", profile.FFlags, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "GroupByDirection", profile.GroupByDirection, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "GroupByKey:FMTID", profile.GroupByKey_FMTID, RegistryValueKind.String);
            Registry.SetValue(DesktopSettingsRegistryPath, "GroupByKey:PID", profile.GroupByKey_PID, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "GroupView", profile.GroupView, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "IconLayouts", profile.IconLayouts, RegistryValueKind.Binary);
            Registry.SetValue(DesktopSettingsRegistryPath, "IconSize", profile.IconSize, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "LogicalViewMode", profile.LogicalViewMode, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "Mode", profile.Mode, RegistryValueKind.DWord);
            Registry.SetValue(DesktopSettingsRegistryPath, "Sort", profile.Sort, RegistryValueKind.Binary);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dp = ReadSettings();
            if (comboBox1.Text=="New")
            {
                if (!Directory.Exists(SavedProfilesDirectory))
                {
                    Directory.CreateDirectory(SavedProfilesDirectory);
                }
                XmlSerializer xml = new XmlSerializer(typeof(DesktopProfile));
                TextWriter writer = new StreamWriter(SavedProfilesDirectory+"\\New.xml");
                xml.Serialize(writer, dp);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            WriteSettings(dp);
        }
    }
}
