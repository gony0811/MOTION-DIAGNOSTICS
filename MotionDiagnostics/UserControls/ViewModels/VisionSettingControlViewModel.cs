using EPLE.Data;
using EPLE.Manager;
using HalconDotNet;

using Microsoft.Win32;
using EPLE.ImageProcessing;
using Prism.Commands;
using PrismCommands;
using PropertyChanged;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static EPLE.ImageProcessing.HalconImageProcessing;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class VisionSettingControlViewModel
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly HalconImageProcessing imageProcessing;
        private string registeredImageFileNameWithNoExtension = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"final.jpg");
        private string modelFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "model.shm");

        public ImageSource ImageSource { get; set; }

        public bool IsLiveOn { get; set; }

        public VisionSettingControlViewModel(DataManager dataManager, DeviceManager deviceManager, HalconImageProcessing imageProcessing)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.imageProcessing = imageProcessing;

            imageProcessing.MarkFindEventHandler += FireMarkFindEvnet;

            LoadButtonClickCommand = new DelegateCommand(LoadButtonClick);
            LiveButtonClickCommand = new DelegateCommand(LiveButtonClick);
            SaveButtonClickCommand = new DelegateCommand(SaveButtonClick);
            FindButtonClickCommand = new DelegateCommand(FindButtonClick);
            RegisterButtonClickCommand = new DelegateCommand(RegisterButtonClick);
            UpdateButtonClickCommand = new DelegateCommand(UpdateButtonClick);
        }

        [DelegateCommand]
        public void Loaded()
        {
            //if (File.Exists(registeredImageFileNameWithNoExtension) && File.Exists(modelFileName))
            //{
            //    this.imageProcessing.ShapeModel.ReadShapeModel(modelFileName);
            //    ImageSource = new BitmapImage(new Uri(this.registeredImageFileNameWithNoExtension));
            //}
        }

        private void FireMarkFindEvnet(object sender, MarkFindEventArgs e)
        {
            VisionOffsetX = e.Offset_X;
            VisionOffsetY = e.Offset_Y;
        }

        public double VisionOffsetX { get; set; }

        public double VisionOffsetY { get; set; }

        public DelegateCommand LoadButtonClickCommand { get; private set; }
        public DelegateCommand LiveButtonClickCommand { get; private set; }
        public DelegateCommand SaveButtonClickCommand { get; private set; }
        public DelegateCommand FindButtonClickCommand { get; private set; }
        public DelegateCommand RegisterButtonClickCommand { get; private set; }
        public DelegateCommand UpdateButtonClickCommand { get; private set; }

        public void LoadButtonClick()
        {
            if (imageProcessing.IsGrabStart)
            {
                System.Windows.MessageBox.Show("카메라가 LIVE 상태이므로 이미지를 불러올 수 없습니다. LIVE 상태를 꺼주세요.", "Warning");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "TIF files (*.tif)|*.tif";
            if (openFileDialog.ShowDialog() == true)
            {
                imageProcessing.LoadImage(openFileDialog.FileName);
                imageProcessing.DisplayWindow(imageProcessing.Window);
            }
        }

        public void LiveButtonClick()
        {
            if (!IsLiveOn)
            {
                IsLiveOn = true;
                imageProcessing.GrapStart();
            } 
            else
            {
                IsLiveOn = false;
                imageProcessing.GrapStop();
            }
        }
        public void SaveButtonClick()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "TIF files (*.tif)|*.tif";
            if (saveFileDialog.ShowDialog() == true)
            {
                imageProcessing.SaveImage(saveFileDialog.FileName);
            }
        }

        public void FindButtonClick()
        {
            (double x, double y) = imageProcessing.Find(out bool result);

            VisionOffsetX = x;
            VisionOffsetY = y;

            this.dataManager.SET_DATA(DataNameHelper.MARK_X_OFFSET, x);
            this.dataManager.SET_DATA(DataNameHelper.MARK_Y_OFFSET, y);
        }

        public void RegisterButtonClick()
        {
            
            ImageSource = imageProcessing.RegisterModel();
        }

        public void UpdateButtonClick()
        {

        }

        private Bitmap LoadBitmap(string path)
        {
            if (!File.Exists(path)) return null;

            // open file in read only mode
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // get a binary reader for the file stream
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    // copy the content of the file into a memory stream
                    var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                    // make a new Bitmap object the owner of the MemoryStream
                    return new Bitmap(memoryStream);
                }
            }
        }
    }
}
