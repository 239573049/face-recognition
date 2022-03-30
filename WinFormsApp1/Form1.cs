using System.Runtime.InteropServices;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        const uint WM_SYSCOMMAND = 0x0112;
        const uint SC_MONITORPOWER = 0xF170;
        [DllImport("user32")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SendMessage(this.Handle, WM_SYSCOMMAND,
       SC_MONITORPOWER, 2); // 2 为关闭显示器， －1则打开显示器
        }
    }
}