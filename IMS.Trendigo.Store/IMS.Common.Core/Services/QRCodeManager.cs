using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace IMS.Common.Core.Services
{
    public class QRCodeManager
    {
        public Bitmap WriteQRCode(int format, string text, string logoId = "", long? merchantId = null)
        {
            var barcodeWriter = new BarcodeWriter();
            var encodingOptions = new EncodingOptions { Width = format, Height = format, Margin = 0, PureBarcode = false };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Renderer = new BitmapRenderer();
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            Bitmap bitmap = barcodeWriter.Write(text);

            
            if (!string.IsNullOrEmpty(logoId) && merchantId.HasValue)
            {
                //Bitmap overlay = new Bitmap(Application.StartupPath + "/logo.png");
                Bitmap overlay = new Bitmap(Application.StartupPath + "images/merchant/" + merchantId.ToString() + "/logo/" + logoId + ".jpg");
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawImage(overlay, new Point((bitmap.Width - overlay.Width) / 2, (bitmap.Height - overlay.Height) / 2));
                return bitmap;
            }
            else
            {
                Graphics g = Graphics.FromImage(bitmap);
                return bitmap;
            }
        }

        //public bool ReadQRCode(Bitmap bmp)
        //{
        //    BarcodeReader reader = new BarcodeReader();
        //    var result = reader.Decode(bmp);
        //    if (result == null)
        //        return false;
        //    return result.Text == url;
        //}
    }
}
