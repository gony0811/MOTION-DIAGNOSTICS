using EPLE.Manager;
using EPLE.ImageProcessing;
using PropertyChanged;
using PrismCommands;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows.Threading;
using System;
using System.Windows;
using HalconDotNet;


namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class HalconImageProcessingControlViewModel
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly HalconImageProcessing imageProcessing;
        private HWindow hWindow;
        private DispatcherTimer timer = new DispatcherTimer();

        public HalconImageProcessingControlViewModel(DataManager dataManager, DeviceManager deviceManager, HalconImageProcessing imageProcessing)
        {
            this.deviceManager = deviceManager;
            this.dataManager = dataManager;
            this.imageProcessing = imageProcessing;
        }

        public void Open(HWindow targetWindow, string configurationFileWithoutExtension, bool simulator = false)
        {
            if (!imageProcessing.IsOpen)
                imageProcessing.Open(targetWindow, configurationFileWithoutExtension, simulator);

            hWindow = targetWindow;
        }
    }
}
