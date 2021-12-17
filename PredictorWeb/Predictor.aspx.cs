using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        string imgFileUrl = "http://127.0.0.1:8080/";
        string pyFileUrl = "C:\\Users\\莊舒雅\\Desktop\\Test_jj\\PredictorWeb\\PredictorWeb\\bin\\";
        //List<string> imgName = new List<string>(); //存照片名
        List<string> resultString = new List<string>(); //存分析結果
        double[,] gnbArray = new double[48, 2];
        double[,] gpbArray = new double[48, 2];
        double[,] gpcArray = new double[48, 2];
        double[,] chainArray = new double[48, 2];
        double[,] yeastArray = new double[48, 2];
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

            DateTime today = DateTime.Today;
            Console.WriteLine(today.ToString("yyMMddhhmmss"));
            string newName = Path.GetFileName(FileUpload1.FileName) + today.ToString("yyMMddhhmmss");
            string newPath= folderPath + newName;

            //Save the File to the Directory (Folder).
            FileUpload1.SaveAs(newPath+".JPG");

            //Display the Picture in Image control.
            //Image1.ImageUrl = "~/Images/" + newName+".JPG";

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
                    
                    pic.Save(newPath+ "-" + imgCount.ToString() + ".JPG");

                    images.Add(imgFileUrl + "images/"+newName + "-" + imgCount.ToString() + ".JPG");
                    imgCount++;
                }

            }
            ListView1.DataSource= images;

            ListView1.DataBind();
            Prediction();
        }

        private void Prediction()
        {
            int gnbCount = 0;
            int gpbCount = 0;
            int gpcCount = 0;
            int chainCount = 0;
            int yeastCount = 0;

            Label1.Text = "分析中..........";
            FileUpload1.Enabled = false;
            PredictButton.Enabled = false;

            runPythonCode("gnbPredict.exe");
            loadTxt("gnb.txt", 1);
        }
        public void runPythonCode(string fileName) //分類機
        {
            //----------------run python exe file
            //string pyexePath = "C:\\Users\\莊舒雅\\Downloads\\Code-20211212T070334Z-001\\Code\\predictor2\\bin\\Debug\\gnbPredict.exe"; //fileName;
            Process p = new Process();
            p.StartInfo.FileName = pyFileUrl+ fileName;//需要執行的檔案路徑
            p.StartInfo.UseShellExecute = false; //必需
            p.StartInfo.RedirectStandardOutput = true;//輸出引數設定
            p.StartInfo.RedirectStandardInput = true;//傳入引數設定
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = " ";//引數以空格分隔，如果某個引數為空，可以傳入””
            p.Start();
            string outputText = p.StandardOutput.ReadToEnd();
            outputText = outputText.Replace(Environment.NewLine, string.Empty);
            p.WaitForExit();//關鍵，等待外部程式退出後才能往下執行}
            p.Close();
        } //run python code to predicted

        public void loadTxt(string fileN, int classT) //讀檔
        {
            string fileName = fileN;
            StreamReader sr = new StreamReader(pyFileUrl+fileName);
            while (!sr.EndOfStream)
            {
                // 每次讀取一行，直到檔尾
                // 讀取文字到 line 變數
                string line = sr.ReadLine();
                //textBox1.Text += line+"\n";
                resultString.Add(line);
                //readCount++;       
            }
            sr.Close();
            //int readCount = 0;
            foreach (string word in resultString)
            {
                string[] resStr = word.Split(':');

                string tempStr = resStr[0];
                tempStr = tempStr.Split('.')[0];
                tempStr = tempStr.Split('-')[tempStr.Split('-').Length - 1];
                int index = int.Parse(tempStr);

                string res = resStr[1];
                string res2 = resStr[2];
                if (classT == 1)
                {
                    gnbArray[index, 0] = Convert.ToDouble(res);
                    gnbArray[index, 1] = Convert.ToDouble(res2);
                }
                else if (classT == 2)
                {
                    gpbArray[index, 0] = Convert.ToDouble(res);
                    gpbArray[index, 1] = Convert.ToDouble(res2);
                }
                else if (classT == 3)
                {
                    gpcArray[index, 0] = Convert.ToDouble(res);
                    gpcArray[index, 1] = Convert.ToDouble(res2);
                }
                else if (classT == 4)
                {
                    chainArray[index, 0] = Convert.ToDouble(res);
                    chainArray[index, 1] = Convert.ToDouble(res2);
                }
                else if (classT == 5)
                {
                    yeastArray[index, 0] = Convert.ToDouble(res);
                    yeastArray[index, 1] = Convert.ToDouble(res2);
                }
            }
        }
    }
}