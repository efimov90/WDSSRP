using Microsoft.Win32;
using System;
using System.Drawing;
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

        private Point MouseOffset;
        private bool DraggingWindow = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var item in Directory.GetFiles(SavedProfilesDirectory, "*.xml", SearchOption.TopDirectoryOnly))
            {
                comboBox1.Items.Add(item);
            }
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
            if (comboBox1.Text != "New")
            {
                string currentFile = comboBox1.SelectedItem.ToString();
                if (File.Exists(currentFile))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(DesktopProfile));
                    TextReader reader = new StreamReader(currentFile);
                    dp = xml.Deserialize(reader) as DesktopProfile;
                    WriteSettings(dp);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseOffset.X = e.X;
            MouseOffset.Y = e.Y;
            DraggingWindow = true;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            DraggingWindow = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (DraggingWindow)
            {
                this.SetDesktopLocation(MousePosition.X - MouseOffset.X, MousePosition.Y - MouseOffset.Y);
            }
        }
    }
}
