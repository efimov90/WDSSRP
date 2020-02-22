using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WDSSRP
{
    public partial class MainForm : Form
    {
        private readonly string SavedProfilesDirectory = "Profiles";

        private RegistryProfileModel DesktopProfileModel;

        private Point MouseOffset;
        private bool DraggingWindow = false;

        AboutBox AboutBox = new AboutBox();


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text=="New")
            {
                comboBox1.Items.Add(SaveProfile());
            }
        }

        private void makeScreenshot(string filename)
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);

            this.Hide();
            System.Threading.Thread.Sleep(200);

            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                              Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap as Image))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }

                bitmap.Save(filename);
            }

            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
            this.Show();
        }

        private string SaveProfile()
        {
            string filename = "Settings " + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.fffffff");

            if (!Directory.Exists(SavedProfilesDirectory))
            {
                Directory.CreateDirectory(SavedProfilesDirectory);
            }

            makeScreenshot(SavedProfilesDirectory + "\\" + @filename + ".bmp");

            XmlSerializer xml = new XmlSerializer(typeof(DesktopProfile));
            TextWriter writer = new StreamWriter(SavedProfilesDirectory + "\\" + @filename + ".xml");
            xml.Serialize(writer, DesktopProfileModel.GetDesktopProfile());
            return SavedProfilesDirectory + "\\" + @filename;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string currentFile = comboBox1.SelectedItem.ToString();
            if (File.Exists(currentFile))
            {
                XmlSerializer xml = new XmlSerializer(typeof(DesktopProfile));
                TextReader reader = new StreamReader(currentFile);
                DesktopProfileModel.SetDesktopProfile(xml.Deserialize(reader) as DesktopProfile);
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

        private void button1_Click(object sender, EventArgs e)
        {
            AboutBox.ShowDialog();
        }
    }
}
