using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadStation
{
    public partial class ImageShow : Form, IDisposable
    {
        string[] ImagePath;
        int iCurrentIndex = 0;
        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    dSkinPictureBox1.Image?.Dispose();
                    components?.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        public ImageShow(string[] paths)
        {
            InitializeComponent();
            ImagePath = paths;
            this.FormClosing += (s, e) =>
            {
                dSkinPictureBox1.Image?.Dispose();
            };
        }
        private void DSkinButton_Last_Click(object sender, EventArgs e)
        {
            if (ImagePath.Length == 1) return;
            iCurrentIndex++;
            if (iCurrentIndex > ImagePath.Length - 1)
            {
                iCurrentIndex = 0;
            }
            showPicture();
        }

        private void DSkinButton_Next_Click(object sender, EventArgs e)
        {
            if (ImagePath.Length == 1) return;
            iCurrentIndex--;
            if (iCurrentIndex < 0)
            {
                iCurrentIndex = ImagePath.Length - 1;
            }
            showPicture();
        }

        private void DSkinButton_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ImageShow_Shown(object sender, EventArgs e)
        {
            showPicture();
        }

        private async void showPicture()
        {
            // 释放旧图像
            if (dSkinPictureBox1.Image != null)
            {
                dSkinPictureBox1.Image.Dispose();
                dSkinPictureBox1.Image = null;
            }
            byte[] zipBytes = await Task.Run(() => HttpHelper.GetSingleRaw(ImagePath[iCurrentIndex]));
            byte[] imageBytes = ExtractSingleFileFromZip(zipBytes);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                dSkinPictureBox1.Image = Image.FromStream(ms);
            }
        }

        private byte[] ExtractSingleFileFromZip(byte[] zipBytes)
        {
            using (var zipStream = new MemoryStream(zipBytes))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                var entry = archive.Entries.FirstOrDefault();
                if (entry == null) throw new Exception("zip 包为空");
                using (var entryStream = entry.Open())
                using (var ms = new MemoryStream())
                {
                    entryStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
