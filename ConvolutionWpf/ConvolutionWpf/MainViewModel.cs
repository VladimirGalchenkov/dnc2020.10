using System;
using System.IO;
using System.Windows.Media.Imaging;
using Catel.Data;
using Catel.MVVM;
using ConvolutionWpf.Commands;
using Microsoft.Win32;

namespace ConvolutionWpf
{
    public class MainViewModel: ViewModelBase
    {
        public MainViewModel()
        {
            LoadCmd = new Command(LoadCommand);
            SaveCmd = new Command(SaveCommand);
            ResetCmd = new Command(ResetCommand);

            GrayScaleCmd = new GrayScaleCommand(() => Image);
            BlurCmd = new BlurCommand(() => Image);
            LogCmd = new LogCommand(() => Image);
            ContrastCmd = new ContrastCommand(() => Image);
            FlipCmd = new FlipCommand(() => Image);
            NegateCmd = new NegateCommand(() => Image);
            SobelEdgesCmd = new SobelEdgeDetectionCommand(() => Image);
            ImpulseNoiseCmd = new ImpulseNoiseCommand(() => Image);
            EdgesCmd = new EdgeDetectionCommand(() => Image);

            FlipCmd.OnImageChanged += img => Image = img;
        }

        private WriteableBitmap _originalImage;

        public Command GrayScaleCmd { get; }

        public Command BlurCmd { get; }

        public Command LogCmd { get; }

        public Command ContrastCmd { get; }

        public FlipCommand FlipCmd { get; }

        public Command EdgesCmd { get; }

        public Command NegateCmd { get; }

        public Command LoadCmd { get; }

        public Command SaveCmd { get; }

        public Command ResetCmd { get; }

        public Command SobelEdgesCmd { get; }

        public Command ImpulseNoiseCmd { get; }

        public static PropertyData ImagePathProperty = RegisterProperty("Image", typeof(WriteableBitmap));

        public WriteableBitmap Image
        {
            get => GetValue<WriteableBitmap>(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        private void LoadCommand()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Images|*.png;*.jpg;*jpeg;*.bmp";
            if (dlg.ShowDialog() == true)
            {
                Image = new WriteableBitmap(new BitmapImage(new Uri(dlg.FileName)));
            }

            _originalImage = Image.Clone();
        }

        private void ResetCommand()
        {
            Image = _originalImage.Clone();
        }

        private void SaveCommand()
        {
            var dlg = new SaveFileDialog() { DefaultExt = ".png" };
            dlg.Filter = "Png|*.png";
            if (dlg.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Image));

                using (var fileStream = new FileStream(dlg.FileName, FileMode.Create))
                    encoder.Save(fileStream);
            }
        }
    }
}