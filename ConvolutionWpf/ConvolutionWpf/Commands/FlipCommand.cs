using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.MVVM;

namespace ConvolutionWpf.Commands
{
    public class FlipCommand : Command
    {
        private readonly Func<WriteableBitmap> _imageFactory;

        public event Action<WriteableBitmap> OnImageChanged;

        public FlipCommand(Func<WriteableBitmap> imageFactory)
            : base(() => { })
        {
            _imageFactory = imageFactory;
        }

        public void ExecuteCommand()
        {
            var image = _imageFactory();
            if (image == null)
                return;

            

            var pixels = new byte[image.PixelHeight * image.BackBufferStride];
            image.CopyPixels(pixels, image.BackBufferStride, 0);

            var imageRes = new WriteableBitmap(2 * image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, image.Format, image.Palette);
            var resultPixels = new byte[imageRes.PixelHeight * imageRes.BackBufferStride];

			//todo

            imageRes.WritePixels(new Int32Rect(0, 0, imageRes.PixelWidth, imageRes.PixelHeight), resultPixels, imageRes.BackBufferStride, 0);

            OnImageChanged?.Invoke(imageRes);
        }

        protected override void Execute(object parameter, bool ignoreCanExecuteCheck)
        {
            ExecuteCommand();
        }
    }
}