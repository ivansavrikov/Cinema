using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Cinema.Helpers
{
    public static class BytesConverter
    {
        public static async Task<BitmapImage> ToBitmapImage(byte[] imageBytes)
        {
            BitmapImage bitmapImage = new();

            using (MemoryStream ms = new(imageBytes))
            {
                ms.Position = 0;
                IRandomAccessStream randomAccessStream = ms.AsRandomAccessStream();
                await bitmapImage.SetSourceAsync(randomAccessStream);
            }

            return bitmapImage;
        }
    }
}