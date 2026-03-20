using EPLE.Core.Device.Interface;
using OpenCvSharp;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Management;
using System.Threading;
using Serilog;

namespace Device
{
    public class CAM : IDeviceHandler
    {
        private ILogger logger;
        private DevMode devMode;
        private VideoCapture videoCapture;
        private VideoWriter videoWriter;
        private Bitmap picture;
        private int cameraIndex = 0;

        private Timer grabTimer;

        public bool DeviceAttach(string arguments)
        {
            videoCapture = new VideoCapture();

            var fps = (int)Math.Round(1000 / 30.0);

            if (videoCapture.Open(cameraIndex))
            {
                logger.Information("Camera Device Attached");
                devMode = DevMode.CONNECT;
                picture = new Bitmap(1920, 1080);
                this.grabTimer = new Timer(new TimerCallback(GrabTimerCallback), null, 0, fps);
                return true;
            }
            else
            {
                devMode = DevMode.ERROR;
                return false;
            }
        }

        public bool DeviceDettach()
        {
            if (videoCapture != null && videoCapture.IsOpened())
            {
                videoCapture.Release();
                logger.Information("Camera Device Dettached.");
                devMode = DevMode.DISCONNECT;
                grabTimer.Dispose();
                return true;
            }
            else
            {
                logger.Information("Already Camera Device Dettached.");
                devMode = DevMode.DISCONNECT;
                return false;
            }
        }

        public void DeviceInit(ILogger logger)
        {
            this.logger = logger;
            this.devMode = DevMode.DISCONNECT;
        }

        private void GrabTimerCallback(object state)
        {
            try
            {
                if (!videoCapture.IsOpened()) return;

                Mat frame = new Mat();

                try
                {
                    //videoCapture.Read(frame);
                }
                catch (OpenCVException ex)
                {
                    logger.Error(ex, "OpenCVException in videoCapture.Read");
                    devMode = DevMode.ERROR;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception in videoCapture.Read");
                    devMode = DevMode.ERROR;
                }

                frame = ImageProcessing.DrawingCrosshair(frame);

                if (frame.Empty()) return;
                else
                {
                    var bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
                    picture = (Bitmap)bitmap.Clone();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in GrabTimerCallback");
            }
        }

        public bool DeviceReset()
        {
            throw new NotImplementedException();
        }

        public object GET_DATA_IN(string command, ref bool result)
        {
            if (devMode == DevMode.CONNECT)
            {
                result = true;
                return picture;
            }
            else
            {
                result = false;
                return "";
            }
        }

        public double GET_DOUBLE_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public int GET_INT_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public string GET_STRING_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public DevMode IsDevMode()
        {
            return devMode;
        }

        public void SET_DATA_OUT(string command, object value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_DOUBLE_OUT(string command, double value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_INT_OUT(string command, int value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_STRING_OUT(string command, string value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public static List<CameraDevice> GetAllConnectedCameras()
        {
            var cameras = new List<CameraDevice>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                int openCvIndex = 0;
                foreach (var device in searcher.Get())
                {
                    cameras.Add(new CameraDevice()
                    {
                        Name = device["Caption"].ToString(),
                        Status = device["Status"].ToString(),
                        DeviceId = device["DeviceId"].ToString(),
                        OpenCvId = openCvIndex
                    });
                    ++openCvIndex;
                }
            }

            return cameras;
        }

        public class CameraDevice
        {
            public int OpenCvId { get; set; }

            public string Name { get; set; }
            public string DeviceId { get; set; }
            public string Status { get; set; }
        }
    }
}
