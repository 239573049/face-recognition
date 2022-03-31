using AutoPowerOn;
using OpenCvSharp;

var capture = new VideoCapture(0);
var face = new CascadeClassifier("haarcascade_frontalface_alt.xml");//人脸模型库

//此处参考网上的读取方法
int sleepTime = (int)Math.Round(1000 / capture.Fps);

long sleep = 1;
// 声明实例 Mat类
Mat image = new Mat();

// 进入读取视频每镇的循环
while (true)
{
    capture.Read(image);
    //判断是否还有没有视频图像 
    if (image.Empty())
        break;
    var data = face.DetectMultiScale(image);
    if (data != null && data.Count() > 0)
    {
        User32API.SendMessage(User32API.GetCurrentWindowHandle(), User32API.WM_SYSCOMMAND,
   User32API.SC_MONITORPOWER, -1); // 2 为关闭显示器， －1则打开显示器
        sleep = 1;
    }
    else
    {
        if (sleep == 50)
        {
            User32API.SendMessage(User32API.GetCurrentWindowHandle(), User32API.WM_SYSCOMMAND,
       User32API.SC_MONITORPOWER, 2); // 2 为关闭显示器， －1则打开显示器
            sleep = 1;
        }
        else
        {
            sleep++;
        }
    }
    Cv2.WaitKey(sleepTime);
}