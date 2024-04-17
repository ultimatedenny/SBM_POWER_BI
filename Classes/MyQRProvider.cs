using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.IO;
using TwoFactorAuthNet.Providers.Qr;

namespace SBM_POWER_BI.Classes
{
    public class MyQRProvider : IQrCodeProvider
    {
        public string GetMimeType()
        {
            return "image/png";
        }

        public byte[] GetQrCodeImage(string text, int size)
        {
            var encoder = new QrEncoder();
            var qrCode = encoder.Encode(text);
            var renderer = new GraphicsRenderer(new FixedCodeSize(size, QuietZoneModules.Two));
            byte[] result;
            using (var stream = new MemoryStream())
            {
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                result = stream.ToArray();
            }
            return result;
        }
    }
}