using AutoPowerOn;
using OpenCvSharp;
using System.Runtime.InteropServices;

const uint WM_SYSCOMMAND = 0x0112;
const uint SC_MONITORPOWER = 0xF170;
[DllImport("user32")]
static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam); 
[DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
static extern IntPtr GetDesktopWindow();
var capture = new VideoCapture(0);
var face = new CascadeClassifier("haarcascade_frontalface_alt.xml");

//此处参考网上的读取方法
int sleepTime = (int)Math.Round(1000 / capture.Fps);
bool isOpen = false;
long sleep = 1;
using (var window = new Window("capture"))
{
    // 声明实例 Mat类
    Mat image = new Mat();

    // 进入读取视频每镇的循环
    while (true)
    {
        capture.Read(image);
        //判断是否还有没有视频图像 
        if (image.Empty())
            break;
        var data= face.DetectMultiScale(image);
        if(data!= null&&data.Count()>0)
        {
            SendMessage(User32API.GetCurrentWindowHandle(), WM_SYSCOMMAND,
       SC_MONITORPOWER, -1); // 2 为关闭显示器， －1则打开显示器
            isOpen=true;
            sleep = 1;
        }
        else
        {
            if (sleep == 50)
            {

                SendMessage(User32API.GetCurrentWindowHandle(), WM_SYSCOMMAND,
           SC_MONITORPOWER, 2); // 2 为关闭显示器， －1则打开显示器
                isOpen = false;
                sleep = 1;
            }
            else
            {
                sleep++;
            }
        }
        //window.ShowImage(image);
        Console.WriteLine("当前："+sleep);
        Cv2.WaitKey(sleepTime);
    }
}