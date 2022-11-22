using System.Xml.Serialization;
using System.Collections.Generic;

namespace NodeManager.Web.Models.Entities
{
    public class RevitView
    {
        #region xml ignore
        [XmlIgnoreAttribute]
        public RevitProject Parent { get; set; }

        //[XmlIgnoreAttribute]
        //public BitmapImage ImageSource { get; set; }
        #endregion
        public int ViewId { get; set; }
        public string ViewName { get; set; }
        public string Project { get; set; }
        public string ImagePath { get; set; }
        public int Scale { get; set; }
        public List<string> Tags { get; set; }
        public List<RevitParameter> Parameters { get; set; }


        //public RevitView(View view)
        //{
        //    Tags = new List<string>();
        //    Parameters = new List<RevitParameter>();

        //    ViewId = view.Id.IntegerValue;
        //    ViewName = view.Name;
        //    Scale = view.Scale;
        //}

        public RevitView()
        {
            Tags = new List<string>();
            Parameters = new List<RevitParameter>();
        }

        //public void SetImage()
        //{
        //    try
        //    {
        //        string docTitle = Parent.Name;
        //        string fileName = $"{docTitle}-{ViewId}.jpg";
        //        var path = Path.Combine(Path.GetTempPath(), $@"NodeManagerTemp\Images\{fileName}");
        //        var bitmap = new Bitmap(path);

        //        using (MemoryStream memory = new MemoryStream())
        //        {
        //            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Gif);
        //            memory.Position = 0;
        //            ImageSource = new BitmapImage();
        //            ImageSource.BeginInit();
        //            ImageSource.StreamSource = memory;
        //            ImageSource.CacheOption = BitmapCacheOption.OnLoad;
        //            ImageSource.EndInit();
        //        }
        //        bitmap.Dispose();
        //        //var bmpSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //        //bitmap.Dispose();
        //        //ImageSource = (Bitmap)bitmap.Clone();
        //    }
        //    catch
        //    {

        //    }
        //}

        public void Dispose()
        {
            Parent = null;
            //ImageSource = null;
            Tags.Clear();
        }
    }
}
