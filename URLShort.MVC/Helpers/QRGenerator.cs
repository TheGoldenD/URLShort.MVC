using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;
//using ZXing.QrCode.Internal;

namespace URLShort.MVC.Helpers
{
    public static class GenerateQR
    {
        public static string FromUrl(string shortenedUrl)
        {
            // Create QR code
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(shortenedUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            using var qrCodeImage = qrCode.GetGraphic(8);

            // Convert to Base64 string
            using var ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Png);
            var qrBase64 = Convert.ToBase64String(ms.ToArray());
            
            return $"data:image/png;base64,{qrBase64}";
        }
    }
}
