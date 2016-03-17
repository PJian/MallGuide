using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ResourceManagementService.converter
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class ImgConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String path = (String)value;
            if (path != null && !"".Equals(path.Trim()))
            {

                BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open));
                FileInfo fileInfo = new FileInfo(path);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(bytes);
                bitmap.EndInit();
                //ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
                //BitmapFrame source = imageSourceConverter.ConvertFrom(stream) as BitmapFrame;
               // img_preview.Source = bitmap;
                //BitmapImage bi = new BitmapImage();
                //bi.BeginInit();
                //bi.CacheOption = BitmapCacheOption.OnLoad; //增加这一行
                //bi.UriSource = new Uri(path, UriKind.Absolute);
                //bi.EndInit();
                return bitmap;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
