using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace eSchoolWeb
{
    public partial class ValidatePicPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
            #region [OLD]
            //string validate_num = CreateRandomNum(4);
            //Session["ValidateNum"] = validate_num;
            //CreateImage(validate_num);
            #endregion

            ValidatePic pic = new ValidatePic();

            #region [MDY:20200902] M202009_01 驗證碼改成 6 碼
            string errmsg = null;
            byte[] imgContent = pic.GenValidateImage(6, out errmsg);
            #endregion

            if (imgContent != null)
            {
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(imgContent);
            }
            else
            {

            }
            #endregion
        }

        #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
        #region [OLD]
        //private string CreateRandomNum(int NumCount)
        //{
        //    string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        //    string[] allCharArray = allChar.Split(',');//差分成陣列
        //    string randomNum = "";
        //    int temp = -1;//記錄上次亂數值的數值，儘量避免產生幾個相同的亂數
        //    Random rand = new Random();
        //    for (int i = 0; i < NumCount; i++)
        //    {
        //        if (temp != -1)
        //        {
        //            rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
        //        }
        //        int t = rand.Next(allCharArray.Length - 1);
        //        if (temp == t)
        //        {
        //            return CreateRandomNum(NumCount);
        //        }
        //        temp = t;
        //        randomNum += allCharArray[t];
        //    }
        //    return randomNum;
        //}

        //private void CreateImage(string validateNum)
        //{
        //    if (validateNum == null || validateNum.Trim() == String.Empty)
        //        return;
        //    int fontSize = 12;
        //    //生成bitmap圖像
        //    Bitmap image = new Bitmap(validateNum.Length * fontSize + 20, 22);
        //    Graphics g = Graphics.FromImage(image);
        //    try
        //    {
        //        //生成隨機生成器
        //        Random random = new Random();
        //        g.Clear(Color.White);
        //        //畫圖片背景噪音線
        //        for (int i = 0; i < 25; i++)
        //        {
        //            int x1 = random.Next(image.Width);
        //            int x2 = random.Next(image.Width);
        //            int y1 = random.Next(image.Height);
        //            int y2 = random.Next(image.Height);
        //            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
        //        }
        //        Font font = new Font("Arial", fontSize, (FontStyle.Bold | FontStyle.Italic));
        //        LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
        //        g.DrawString(validateNum, font, brush, 2, 2);
        //        //畫圖片的前景噪音點
        //        for (int i = 0; i < 100; i++)
        //        {
        //            int x = random.Next(image.Width);
        //            int y = random.Next(image.Height);
        //            image.SetPixel(x, y, Color.FromArgb(random.Next()));
        //        }
        //        //畫圖片的邊框線
        //        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        //將圖像保存到指定的流
        //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

        //        Response.ClearContent();
        //        Response.ContentType = "image/Gif";
        //        Response.BinaryWrite(ms.ToArray());
        //    }
        //    finally
        //    {
        //        g.Dispose();
        //        image.Dispose();
        //    }
        //}
        #endregion
        #endregion
    }
}