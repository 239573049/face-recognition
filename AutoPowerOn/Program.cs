using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.IO;

var capture = new VideoCapture(0);
var face = new CascadeClassifier("haarcascade_frontalface_alt.xml");

//此处参考网上的读取方法
int sleepTime = (int)Math.Round(1000 / capture.Fps);

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

        Debug.WriteLine(JsonConvert.SerializeObject(data[0]));
        }
        // 在picturebox中播放视频， 需要先转换成bitmap格式

        // 在Window窗口中播放视频
        window.ShowImage(image);

        Cv2.WaitKey(sleepTime);
    }
}