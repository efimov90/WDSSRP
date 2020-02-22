using Microsoft.Win32;

namespace WDSSRP
{
    class RegistryProfileModel
    {
        private readonly string DesktopSettingsRegistryPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\Shell\\Bags\\1\\Desktop";

        public DesktopProfile GetDesktopProfile()
        {
            DesktopProfile profile = new DesktopProfile();
            profile.FFlags = Registry.GetValue(DesktopSettingsRegistryPath, "FFlags", null) as int?;
            profile.GroupByDirection = Registry.GetValue(DesktopSettingsRegistryPath, "GroupByDirection", null) as int?;
            profile.GroupByKey_FMTID = Registry.GetValue(DesktopSettingsRegistryPath, "GroupByKey:FMTID", null) as string;
            profile.GroupByKey_PID = Registry.GetValue(DesktopSettingsRegistryPath, "GroupByKey:PID", null) as int?;
            profile.GroupView = Registry.GetValue(DesktopSettingsRegistryPath, "GroupView", null) as int?;
            profile.IconLayouts = (byte[])Registry.GetValue(DesktopSettingsRegistryPath, "IconLayouts", null);
            profile.IconSize = Registry.GetValue(DesktopSettingsRegistryPath, "IconSize", null) as int?;
            profile.LogicalViewMode = Registry.GetValue(DesktopSettingsRegistryPath, "LogicalViewMode", null) as int?;
            profile.Mode = Registry.GetValue(DesktopSettingsRegistryPath, "Mode", null) as int?;
            profile.Sort = (byte[])Registry.GetValue(DesktopSettingsRegistryPath, "Sort", null);
            return profile;
        }

        public void SetDesktopProfile(DesktopProfile profile)
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
    }
}
