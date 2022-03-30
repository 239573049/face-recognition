using AutoPowerOn;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.IO;
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
using (var window = new Window("capture"))
{
    // 声明实例 Mat类
    Mat image = new Mat();

    // 进入读取视频每镇的循环
    while (true)
    {
        if (isOpen)
        {
            Thread.Sleep(5000);
        }
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
        }
        else
        {
            SendMessage(User32API.GetCurrentWindowHandle(), WM_SYSCOMMAND,
       SC_MONITORPOWER, 2); // 2 为关闭显示器， －1则打开显示器
            isOpen = false;
        }
        // 在picturebox中播放视频， 需要先转换成bitmap格式

        // 在Window窗口中播放视频
        window.ShowImage(image);

        Cv2.WaitKey(sleepTime);
    }
}