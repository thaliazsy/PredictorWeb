using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;

namespace PredictorWeb
{
    public partial class Predictor : System.Web.UI.Page
    {
        string imgFileName;
        List<string> imgName = new List<string>(); //存照片名
        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList a = new ArrayList();
        }

        //public void imgCut(string imgP)
        //{

        //    List<BitmapImage> imageList = new List<BitmapImage>();


        //    if (ListBox1.Items.Count > 0)
        //        ListBox1.Items.Clear();
        //    display.Dispose();
        //    display.ImageSize = new Size(140, 140);
        //    display.ColorDepth = (ColorDepth)24;
        //    display2.Dispose();
        //    display2.ImageSize = new Size(200, 200);
        //    display2.ColorDepth = (ColorDepth)24;

        //    Image myImg = Image.FromFile(imgP);
        //    int picWidth = 200;
        //    int picHeight = 200;
        //    int imgCount = 0;

        //    string[] imgPath = imgP.Split('\\');
        //    string imgN = imgPath[imgPath.Length - 1].Replace(".JPG", "");
        //    imgFileName = imgN;
        //    for (int idxY = 0; idxY < 6; idxY++)//Y軸迴圈
        //    {
        //        for (int idxX = 0; idxX < 8; idxX++)//X軸迴圈
        //        {
        //            Bitmap pic = new Bitmap(picWidth, picHeight);
        //            //建立圖片
        //            Graphics graphic = Graphics.FromImage(pic);
        //            //建立畫板
        //            graphic.DrawImage(myImg,
        //                     //將被切割的圖片畫在新圖片上面，第一個參數是被切割的原圖片
        //                     new Rectangle(0, 0, picWidth, picHeight),
        //                     //指定繪製影像的位置和大小，基本上是同pic大小
        //                     new Rectangle(picWidth * idxX, picHeight * idxY, picWidth, picHeight),
        //                     //指定被切割的圖片要繪製的部分
        //                     GraphicsUnit.Pixel);
        //            ListBox1.Items.Add("", imgCount);
        //            ListBox1.Items.Add()

        //            ListBox1.Items[imgCount].ToolTipText = imgN + "-" + imgCount.ToString() + ".JPG";
        //            imgName.Add(listView1.Items[imgCount].ToolTipText);

        //            display.Images.Add(pic);
        //            display2.Images.Add(pic);

        //            imgCount++;
        //        }

        //    }
        //    listView1.LargeImageList = display;
        //    myImg.Dispose();

        //    // save img
        //    for (int i = display2.Images.Count - 1; i >= 0; i--) //存分割完照片200x200
        //    {
        //        string saveDir = "imgTemp"; //儲存路徑
        //        string savePath = "";

        //        savePath = saveDir + "\\" + imgName[i];
        //        display2.Images[i].Save(savePath);
        //        dirPath.Add(savePath); //儲存路徑，方便刪除時使用
        //    }
        //} //cut image

        protected void Button1_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Images/");

            //Check whether Directory (Folder) exists.
            if (!Directory.Exists(folderPath))
            {
                //If Directory (Folder) does not exists Create it.
                Directory.CreateDirectory(folderPath);
            }
            //determine wether duplicate name
            DateTime today = DateTime.Today;
            Console.WriteLine(today.ToString("yyMMddhhmmss"));
            string newName = Path.GetFileName(FileUpload1.FileName) + today.ToString("yyMMddhhmmss");
            string newPath= folderPath + newName;
            //if (File.Exists(savePath))
            //{
            //    savePath = folderPath + Path.GetFileName(FileUpload1.FileName+"_1");
            //}

            //Save the File to the Directory (Folder).
            FileUpload1.SaveAs(newPath+".JPG");

            //Display the Picture in Image control.
            Image1.ImageUrl = "~/Images/" + newName+".JPG";

            //change 


            //String imagePath = HttpRuntime.AppDomainAppPath + "images/" + Path.GetFileName(FileUpload1.FileName) + today.ToString("yyyyMMdd")+ ".JPG";
            Image myImg = Image.FromFile(newPath + ".JPG");

            int picWidth = 200;
            int picHeight = 200;
            int imgCount = 0;

            int idxY = 0;
            int idxX = 0;

            List<String> images = new List<string>();
            List<Bitmap> display = new List<Bitmap>();
            for (idxY = 0; idxY < 6; idxY++)//Y軸迴圈
            {
                for (idxX = 0; idxX < 8; idxX++)//X軸迴圈
                {
                    //BitmapImage pic = new BitmapImage(picWidth, picHeight);
                    Bitmap pic = new Bitmap(picWidth, picHeight);
                    //建立圖片
                    Graphics graphic = Graphics.FromImage(pic);
                    graphic.DrawImage(myImg,
                                     //將被切割的圖片畫在新圖片上面，第一個參數是被切割的原圖片
                                     new Rectangle(0, 0, picWidth, picHeight),
                                     //指定繪製影像的位置和大小，基本上是同pic大小
                                     new Rectangle(picWidth * idxX, picHeight * idxY, picWidth, picHeight),
                                     //指定被切割的圖片要繪製的部分
                                     GraphicsUnit.Pixel);
                    //imgName.Add(imgN + "-" + imgCount.ToString());
                   
                   // string savePath = HttpRuntime.AppDomainAppPath + imgName[imgCount]+ ".JPG";
                    
                    pic.Save(newPath+ "-" + imgCount.ToString() + ".JPG");

                    //  Image1.ImageUrl = folderPath + imgName[imgCount];
                    images.Add("http://127.0.0.1:8080/" + "images/"+newName + "-" + imgCount.ToString() + ".JPG");
                    display.Add(pic);
                imgCount++;
                    
                }

            }
            //ListView1.size = new Size(1600, 1200);
            ListView1.DataSource= images;
            ListView1.DataBind();
            RepeaterImages.DataSource = images;
            RepeaterImages.DataBind();
        }
    }
}