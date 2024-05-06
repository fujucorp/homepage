using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace eSchoolWeb
{
    /// <summary>
    /// 圖形驗證碼處理類別
    /// </summary>
    public class ValidatePic
    {
        #region [MDY:20200902] M202009_01 圖形驗證碼改為6碼，每個字的顏色隨機，並增加雜訊點與直線
        #region [OLD]
        //#region [MDY:20191023] M201910_02 修正圖形驗證碼弱點
        //#region Const
        //private const string _CodeChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //private const string _SessionKey = "ValidateCode";
        //#endregion

        //#region Member
        //private Random _Random = null;

        //private readonly object _SessionLock = new object();
        //#endregion

        //#region Constructor
        //public ValidatePic()
        //{
        //    _Random = new Random(DateTime.Now.Ticks.GetHashCode());
        //}
        //#endregion

        //#region Method
        ///// <summary>
        ///// 產生隨機的指定長度代碼
        ///// </summary>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //private string GenRandomCode(int length)
        //{
        //    if (length < 1)
        //    {
        //        return null;
        //    }

        //    int maxValue = _CodeChars.Length - 1;
        //    Char[] myChars = new char[length];
        //    for (int idx = 0; idx < myChars.Length; idx++)
        //    {
        //        myChars[idx] = _CodeChars[_Random.Next(maxValue)];
        //    }
        //    return new String(myChars);
        //}

        ///// <summary>
        ///// 產生指定長度的驗證圖形
        ///// </summary>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //public byte[] GenValidateImage(int length)
        //{
        //    byte[] imgContent = null;
        //    string validateCode = null;
        //    try
        //    {
        //        validateCode = this.GenRandomCode(length);
        //        if (!String.IsNullOrEmpty(validateCode))
        //        {
        //            using (Bitmap image = new Bitmap(validateCode.Length * 18 + 10, 30))
        //            using (Graphics g = Graphics.FromImage(image))
        //            {
        //                g.Clear(Color.White);

        //                #region 畫背景噪音線
        //                for (int idx = 0; idx < 25; idx++)
        //                {
        //                    int x1 = _Random.Next(image.Width);
        //                    int x2 = _Random.Next(image.Width);
        //                    int y1 = _Random.Next(image.Height);
        //                    int y2 = _Random.Next(image.Height);
        //                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
        //                }
        //                #endregion

        //                #region 畫驗證碼
        //                {
        //                    Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
        //                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
        //                    g.DrawString(validateCode, font, brush, 2, 2);
        //                }
        //                #endregion

        //                #region 畫前景噪音點
        //                for (int idx = 0; idx < 100; idx++)
        //                {
        //                    int x = _Random.Next(image.Width);
        //                    int y = _Random.Next(image.Height);
        //                    image.SetPixel(x, y, Color.FromArgb(_Random.Next()));
        //                }
        //                #endregion

        //                #region 畫邊框線
        //                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
        //                #endregion

        //                #region 將圖像轉成 Byte[]
        //                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //                {
        //                    image.Save(ms, ImageFormat.Gif);
        //                    imgContent = ms.ToArray();
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        imgContent = null;
        //    }
        //    finally
        //    {
        //        lock (_SessionLock)
        //        {
        //            if (imgContent != null)
        //            {
        //                HttpContext.Current.Session[_SessionKey] = validateCode;
        //            }
        //            else
        //            {
        //                HttpContext.Current.Session.Remove(_SessionKey);
        //            }
        //        }
        //    }
        //    return imgContent;
        //}

        ///// <summary>
        ///// 檢查驗證碼
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public bool CheckValidateCode(string code)
        //{
        //    bool isOK = false;
        //    lock (_SessionLock)
        //    {
        //        string validateCode = HttpContext.Current.Session[_SessionKey] as string;
        //        isOK = (!String.IsNullOrEmpty(validateCode) && validateCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        //        HttpContext.Current.Session.Remove(_SessionKey);
        //    }
        //    return isOK;
        //}
        //#endregion
        //#endregion

        //#region [OLD]
        ////public ValidatePic()
        ////{

        ////}

        ////public static string CreateRandomNum(int NumCount)
        ////{
        ////    string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        ////    string[] allCharArray = allChar.Split(',');//差分成陣列
        ////    string randomNum = "";
        ////    int temp = -1;//記錄上次亂數值的數值，儘量避免產生幾個相同的亂數
        ////    Random rand = new Random();
        ////    for (int i = 0; i < NumCount; i++)
        ////    {
        ////        if (temp != -1)
        ////        {
        ////            rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
        ////        }
        ////        int t = rand.Next(35);
        ////        if (temp == t)
        ////        {
        ////            return CreateRandomNum(NumCount);
        ////        }
        ////        temp = t;
        ////        randomNum += allCharArray[t];
        ////    }
        ////    return randomNum;
        ////}

        ////private static void CreateImage(string validateNum)
        ////{
        ////    if (validateNum == null || validateNum.Trim() == String.Empty)
        ////        return;
        ////    //生成bitmap圖像
        ////    Bitmap image = new Bitmap(validateNum.Length * 12 + 10, 22);
        ////    Graphics g = Graphics.FromImage(image);
        ////    try
        ////    {
        ////        //生成隨機生成器
        ////        Random random = new Random();
        ////        g.Clear(Color.White);
        ////        //畫圖片背景噪音線
        ////        for (int i = 0; i < 25; i++)
        ////        {
        ////            int x1 = random.Next(image.Width);
        ////            int x2 = random.Next(image.Width);
        ////            int y1 = random.Next(image.Height);
        ////            int y2 = random.Next(image.Height);
        ////            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
        ////        }
        ////        Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
        ////        LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
        ////        g.DrawString(validateNum, font, brush, 2, 2);
        ////        //畫圖片的前景噪音點
        ////        for (int i = 0; i < 100; i++)
        ////        {
        ////            int x = random.Next(image.Width);
        ////            int y = random.Next(image.Height);
        ////            image.SetPixel(x, y, Color.FromArgb(random.Next()));
        ////        }
        ////        //畫圖片的邊框線
        ////        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
        ////        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        ////        //將圖像保存到指定的流
        ////        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        ////        /*
        ////        Response.ClearContent();
        ////        Response.ContentType = "image/Gif";
        ////        Response.BinaryWrite(ms.ToArray());
        ////        */
        ////    }
        ////    finally
        ////    {
        ////        g.Dispose();
        ////        image.Dispose();
        ////    }
        ////}
        //#endregion
        #endregion

        #region Static Readonly
        #region [MDY:20200921] M202009_03 字碼清單改成只有數字
        #region [OLD]
        ///// <summary>
        ///// 字碼清單
        ///// </summary>
        //private static readonly string CodeChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endregion

        /// <summary>
        /// 字碼清單
        /// </summary>
        private static readonly string CodeChars = "0123456789";
        #endregion

        /// <summary>
        /// 字體清單
        /// </summary>
        private static readonly string[] FontFaces = new string[] { "Arial", "Georgia", "Consolas", "Comic Sans MS" };

        /// <summary>
        /// 驗證碼 Session Key
        /// </summary>
        private static readonly string SessionKey = "ValidateCode";

        #region [MDY:20220702] 沒用了
        ///// <summary>
        ///// 最大字體 Size
        ///// </summary>
        //private static readonly int MaxFontSize = 24;

        ///// <summary>
        ///// 最小字體 Size
        ///// </summary>
        //private static readonly int MinFontSize = 14;
        #endregion

        /// <summary>
        /// 最大旋轉角度
        /// </summary>
        private static readonly int MaxRotationAngle = 40;
        #endregion

        #region Member
        #region [MDY:20210401] 原碼修正
        #region [OLD]
        ///// <summary>
        ///// 亂數產生器
        ///// </summary>
        //private Random _Random = null;
        #endregion
        #endregion

        /// <summary>
        /// Session Lock 物件
        /// </summary>
        private readonly object _SessionLock = new object();
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 圖形驗證碼處理類別 物件
        /// </summary>
        public ValidatePic()
        {
        }
        #endregion

        #region 顏色處理 Method
        /// <summary>
        /// Color 轉 HLS
        /// </summary>
        /// <param name="color"></param>
        /// <param name="h"></param>
        /// <param name="l"></param>
        /// <param name="s"></param>
        private void RGBToHLS(Color color, out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = color.R / 255.0;
            double double_g = color.G / 255.0;
            double double_b = color.B / 255.0;

            // Get the maximum and minimum RGB components.
            double max = (double_r < double_g) ? double_g : double_r;
            if (max < double_b)
            {
                max = double_b;
            }

            double min = (double_r > double_g) ? double_g : double_r;
            if (min > double_b)
            {
                min = double_b;
            }

            double diff = max - min;
            l = (max + min) / 2.0;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5)
                {
                    s = diff / (max + min);
                }
                else
                {
                    s = diff / (2.0 - max - min);
                }

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max)
                {
                    h = b_dist - g_dist;
                }
                else if (double_g == max)
                {
                    h = 2 + r_dist - b_dist;
                }
                else
                {
                    h = 4 + g_dist - r_dist;
                }

                h = h * 60;
                if (h < 0)
                {
                    h += 360;
                }
            }
        }

        /// <summary>
        /// HLS 轉 ROG
        /// </summary>
        /// <param name="h"></param>
        /// <param name="l"></param>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        private Color HLSToColor(double h, double l, double s)
        {
            double p2;
            if (l <= 0.5)
            {
                p2 = l * (1 + s);
            }
            else
            {
                p2 = l + s - l * s;
            }

            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            Color color = Color.FromArgb((int)(double_r * 255.0), (int)(double_g * 255.0), (int)(double_b * 255.0));
            return color;
        }

        private double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360)
            {
                hue -= 360;
            }
            else if (hue < 0)
            {
                hue += 360;
            }

            if (hue < 60)
            {
                return q1 + (q2 - q1) * hue / 60;
            }
            if (hue < 180)
            {
                return q2;
            }
            if (hue < 240)
            {
                return q1 + (q2 - q1) * (240 - hue) / 60;
            }
            return q1;
        }

        /// <summary>
        /// 產生隨機深色
        /// </summary>
        /// <returns></returns>
        public Color GenRandomDeepColor()
        {
            #region [MDY:20210401] 原碼修正
            //RGB 色碼越小顏色越深
            int maxRValue = 100;  //紅色色碼
            int maxGValue = 100;  //綠色色碼
            int maxBValue = 160;  //籃色色碼
            Color color = Color.FromArgb(DataFormat.GetRandomValue(maxRValue), DataFormat.GetRandomValue(maxGValue), DataFormat.GetRandomValue(maxBValue));
            return color;
            #endregion
        }

        /// <summary>
        /// 取得指定顏色加上指定亮度程度後的顏色
        /// </summary>
        /// <param name="color">指定顏色</param>
        /// <param name="addValue">增加亮度程度，允許值-1.0 ~ 1.0，負值表示變暗</param>
        /// <returns></returns>
        public Color GetLightnessColor(Color color, double addValue)
        {
            double h, l, s;
            this.RGBToHLS(color, out h, out l, out s);
            l += (addValue % 1);
            if (l >= 1.0)
            {
                l = 1.0;
            }

            Color newColor = this.HLSToColor(h, l, s);
            return newColor;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 產生隨機號碼
        /// </summary>
        /// <param name="length">指定隨機號碼長度</param>
        /// <returns>傳回隨機號碼</returns>
        private string GenRandomCode(int length)
        {
            int maxValue = CodeChars.Length - 1;
            Char[] myChars = new char[length];
            for (int idx = 0; idx < myChars.Length; idx++)
            {
                #region [MDY:20210401] 原碼修正
                myChars[idx] = CodeChars[DataFormat.GetRandomValue(maxValue)];
                #endregion
            }
            return new String(myChars);
        }

        /// <summary>
        /// 產生隨機的驗證碼字元資訊
        /// </summary>
        /// <param name="code">指定驗證碼</param>
        /// <returns></returns>
        private CodeCharInfo[] GenRandomCharInfos(string code)
        {
            #region 設定字型
            FontFamily fontFamily = new FontFamily(GenericFontFamilies.Monospace);
            {
                #region [MDY:20210401] 原碼修正
                int fontFaceIndex = DataFormat.GetRandomValue(FontFaces.Length);
                #endregion

                if (fontFaceIndex < FontFaces.Length)
                {
                    fontFamily = new FontFamily(FontFaces[fontFaceIndex]);
                }
            }
            #endregion

            CodeCharInfo[] charInfos = new CodeCharInfo[code.Length];
            for (int idx = 0; idx < code.Length; idx++)
            {
                string codeChar = code.Substring(idx, 1);
                Color color = this.GenRandomDeepColor();

                #region [MDY:20210818] 改成固定字體大小為 20
                #region [OLD]
                //#region [MDY:20210401] 原碼修正
                //Font font = new Font(fontFamily, DataFormat.GetRandomValue(MinFontSize, MaxFontSize), FontStyle.Bold);
                //#endregion
                #endregion

                Font font = new Font(fontFamily, 20, FontStyle.Bold);
                #endregion

                charInfos[idx] = new CodeCharInfo(codeChar, color, font);
            }
            return charInfos;
        }

        /// <summary>
        /// 繪出指定的驗證碼
        /// </summary>
        private Bitmap DrawCharInfos(int imgWidth, int imgHeight, CodeCharInfo[] charInfos)
        {
            #region 設定字體顯示格式
            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip);
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            #endregion

            Bitmap img = new Bitmap(imgWidth, imgHeight);
            {
                img.MakeTransparent();
                using (Graphics graph = Graphics.FromImage(img))
                {
                    graph.Clear(Color.Transparent);
                    graph.PixelOffsetMode = PixelOffsetMode.Half;
                    graph.SmoothingMode = SmoothingMode.HighQuality;
                    graph.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                    graph.InterpolationMode = InterpolationMode.HighQualityBilinear;

                    int charWidth = Convert.ToInt32(imgWidth / charInfos.Length);
                    int minWidth = 5;
                    int baseHeight = Convert.ToInt32(imgHeight / 5);
                    Point oldPoint = new Point();
                    for (int idx = 0; idx < charInfos.Length; idx++)
                    {
                        CodeCharInfo charInfo = charInfos[idx];
                        Font myFont = charInfo.Font;
                        Int32 fontSize = Convert.ToInt32(myFont.Size);

                        #region 位置
                        #region [MDY:20210401] 原碼修正
                        Point point = new Point(DataFormat.GetRandomValue(minWidth, minWidth + 5), DataFormat.GetRandomValue(baseHeight + (fontSize / 2), imgHeight - (fontSize / 2)));
                        #endregion

                        //如果 X 座標小於字體的二分之一
                        if (point.X < fontSize / 2)
                        {
                            point.X = point.X + fontSize / 2;
                        }
                        //防止文字疊加
                        if (idx > 0 && (point.X - oldPoint.X < fontSize))
                        {
                            point.X = point.X + fontSize;
                        }
                        //防止 X 座標超過
                        if (point.X > img.Width - fontSize)
                        {
                            point.X = img.Width - fontSize;
                        }
                        oldPoint = point;
                        #endregion

                        #region 旋轉角度
                        #region [MDY:20210401] 原碼修正
                        float angle = DataFormat.GetRandomValue(MaxRotationAngle, MaxRotationAngle);
                        #endregion

                        graph.TranslateTransform(point.X, point.Y);  //移到指定位置
                        graph.RotateTransform(angle);
                        #endregion

                        #region 筆刷 & 繪字
                        Rectangle myRectangle = new Rectangle(0, 1, fontSize, fontSize);
                        Color color1 = charInfo.Color;
                        Color color2 = this.GetLightnessColor(color1, 0.4D);
                        using (LinearGradientBrush myBrush = new LinearGradientBrush(myRectangle, color1, color2, angle))
                        {
                            graph.DrawString(charInfo.CodeChar, myFont, myBrush, 1, 1, stringFormat);
                        }
                        #endregion

                        #region 旋轉回去
                        graph.RotateTransform(-angle);
                        graph.TranslateTransform(-point.X, -point.Y);  //移回位置
                        #endregion

                        minWidth += charWidth;
                    }
                }
            }
            return img;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 產生驗證碼圖形
        /// </summary>
        /// <param name="length"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public byte[] GenValidateImage(int length, out string errmsg)
        {
            if (length < 0)
            {
                errmsg = "驗證碼長度不足";
            }

            errmsg = null;
            string validateCode = null;
            byte[] imgContent = null;

            try
            {
                #region [MDY:20210401] 原碼修正
                #region [OLD]
                ////亂數產生器初始化
                //_Random = new Random(DateTime.Now.Ticks.GetHashCode());
                #endregion
                #endregion

                //產生隨機的驗證碼
                validateCode = this.GenRandomCode(length);
                //產生驗證碼的字元資訊陣列
                CodeCharInfo[] charInfos = this.GenRandomCharInfos(validateCode);

                int imgWidth = length * 30 + 10;
                int imgHeight = 40;
                using (Bitmap image = new Bitmap(imgWidth, imgHeight))
                using (Graphics graph = Graphics.FromImage(image))
                {
                    graph.SmoothingMode = SmoothingMode.HighQuality;
                    graph.TextRenderingHint = TextRenderingHint.ClearTypeGridFit; ;
                    graph.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    graph.CompositingQuality = CompositingQuality.HighQuality;

                    //背景清成白色
                    graph.Clear(Color.White);

                    #region 先畫一層(背景)噪點與噪線
                    {
                        #region 噪點
                        {
                            #region [MDY:20210825] 噪點百分比改成為 0.05
                            #region [OLD]
                            //decimal pixRate = 0.1M;  //噪點百分比
                            #endregion

                            decimal pixRate = 0.05M;  //噪點百分比
                            #endregion
                            int pixCount = Convert.ToInt32(imgHeight * imgWidth * pixRate);
                            using (SolidBrush brush = new SolidBrush(GenRandomDeepColor()))
                            {
                                for (int idx = 0; idx < pixCount; idx++)
                                {
                                    #region [MDY:20210401] 原碼修正
                                    int x = DataFormat.GetRandomValue(image.Width);
                                    int y = DataFormat.GetRandomValue(image.Height);
                                    //image.SetPixel(x, y, GenRandomDeepColor());

                                    #region [MDY:20210825] 噪點大小改成為 1 x 1
                                    #region [OLD]
                                    //int width = DataFormat.GetRandomValue(1, 3);
                                    //int height = DataFormat.GetRandomValue(1, 2);
                                    #endregion

                                    int width = 1;
                                    int height = 1;
                                    #endregion
                                    #endregion

                                    graph.FillRectangle(brush, x, y, width, height);
                                    brush.Color = GenRandomDeepColor();
                                }
                            }
                        }
                        #endregion

                        #region 噪線
                        {
                            int lineCount = 5;
                            int maxX1 = Convert.ToInt32(image.Width / 5) * 2;
                            int minX2 = Convert.ToInt32(image.Width / 5) * 3;
                            using (Pen pen = new Pen(GenRandomDeepColor()))
                            {
                                for (int idx = 0; idx < lineCount; idx++)
                                {
                                    #region [MDY:20210401] 原碼修正
                                    int x1 = DataFormat.GetRandomValue(maxX1);
                                    int x2 = DataFormat.GetRandomValue(minX2, image.Width);
                                    int y1 = DataFormat.GetRandomValue(image.Height);
                                    int y2 = DataFormat.GetRandomValue(image.Height);
                                    #endregion

                                    graph.DrawLine(pen, x1, y1, x2, y2);
                                    pen.Color = GenRandomDeepColor();
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 畫驗證碼
                    using (Bitmap img = DrawCharInfos(imgWidth, imgHeight, charInfos))
                    {
                        graph.DrawImageUnscaled(img, 0, 0);
                    }
                    #endregion

                    #region 再畫一層(前景)噪點
                    //{
                    //    int pixCount = 100;
                    //    using (SolidBrush brush = new SolidBrush(GenRandomDeepColor()))
                    //    {
                    //        int minX = image.Width / 5;
                    //        int maxX = minX * 4;
                    //        int minY = image.Height / 5;
                    //        int maxY = minY * 4;
                    //        for (int idx = 0; idx < pixCount; idx++)
                    //        {
                    //            int x = _Random.Next(minX, maxX);
                    //            int y = _Random.Next(minY, maxY);
                    //            image.SetPixel(x, y, GenRandomDeepColor());
                    //        }
                    //    }
                    //}
                    #endregion

                    #region 畫邊框線
                    {
                        graph.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                    }
                    #endregion

                    #region 將圖像轉成 Byte[]
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Gif);
                        imgContent = ms.ToArray();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                validateCode = null;
                imgContent = null;
                errmsg = ex.Message;
            }
            finally
            {
                lock (_SessionLock)
                {
                    if (imgContent != null)
                    {
                        HttpContext.Current.Session[SessionKey] = validateCode;
                    }
                    else
                    {
                        HttpContext.Current.Session.Remove(SessionKey);
                    }
                }
            }
            return imgContent;
        }

        /// <summary>
        /// 檢查驗證碼是否正確
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckValidateCode(string code)
        {
            bool isOK = false;
            lock (_SessionLock)
            {
                string validateCode = HttpContext.Current.Session[SessionKey] as string;
                isOK = (!String.IsNullOrEmpty(validateCode) && validateCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));
                HttpContext.Current.Session.Remove(SessionKey);
            }
            return isOK;
        }
        #endregion

        #region Inner Class
        /// <summary>
        /// 驗證碼字元資訊承載類別
        /// </summary>
        private class CodeCharInfo
        {
            #region Property
            /// <summary>
            /// 字碼
            /// </summary>
            public string CodeChar
            {
                get;
                set;
            }

            /// <summary>
            /// 顏色
            /// </summary>
            public Color Color
            {
                get;
                set;
            }

            /// <summary>
            /// 字體設定
            /// </summary>
            public Font Font
            {
                get;
                set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構 驗證碼字元資訊 承載類別
            /// </summary>
            public CodeCharInfo()
            {
            }


            /// <summary>
            /// 建構 驗證碼字元資訊 承載類別
            /// </summary>
            /// <param name="codeChar">指定字碼</param>
            /// <param name="color">指定顏色 / 漸層起始顏色</param>
            /// <param name="font">指定字體設定</param>
            public CodeCharInfo(string codeChar, Color color, Font font)
            {
                this.CodeChar = codeChar;
                this.Color = color;
                this.Font = font;
            }
            #endregion
        }
        #endregion
        #endregion
    }
}