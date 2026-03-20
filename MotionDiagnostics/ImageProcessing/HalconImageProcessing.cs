using EPLE.Data;
using EPLE.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


//namespace MotionDiagnostics.ImageProcessing
//{


//    public class HalconImageProcessing
//    {
//        public class MarkFindEventArgs
//        {
//            public double Offset_X { get; set; }
//            public double Offset_Y { get; set; }
//            public double Score { get; set; }
//        }

//            #region Properties        
//            /// <summary>
//            /// 표시 Window 지정 
//            /// </summary>
//            public HWindow Window { get; set; }

//        /// <summary>
//        /// 프레임 그래버
//        /// </summary>
//        public HFramegrabber Framegrabber { get; set; }
//        /// <summary>
//        /// 이미지
//        /// </summary>
//        public HImage Image { get; set; }
//        public string ImageType { get; set; }
//        /// <summary>
//        /// 이미지 가로 크기
//        /// </summary>
//        public int ImageWidth { get; set; }
//        /// <summary>
//        /// 이미지 세로 크기
//        /// </summary>
//        public int ImageHeight { get; set; }
//        public HRegion Rectangle { get; set; }
//        public HRegion ModelRegion { get => modelRegion; set => modelRegion = value; }
//        public HShapeModel ShapeModel { get => shapeModel; set => shapeModel = value; }

//        public EventHandler<MarkFindEventArgs> MarkFindEventHandler;


//        private HRegion modelRegion;
//        private HShapeModel shapeModel;

//        public bool IsOpen { get; private set; } = false;

//        public bool IsInitialized { get; private set; } = false;

//        public bool IsGrabStart { get; private set; } = false;

//        public HDrawingObject ROI;

//        private List<HDrawingObject> drawing_objects = new List<HDrawingObject>();
//        private HDrawingObject selected_drawing_object = new HDrawingObject(250, 250, 100);

//        private DispatcherTimer imageGrapTimer = new DispatcherTimer();

//        //private double Row, Column;
//        //private double Rect1Row, Rect1Col, Rect2Row, Rect2Col;
//        //private double RectPhi, RectLength1, RectLength2;

//        private string registeredImageFileNameWithNoExtension = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"final.jpg");
//        private string modelFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "model.shm");
//        private readonly DataManager dataManager;
//        private readonly IDialogCoordinator dialogCoordinator;
//        #endregion

//        public HalconImageProcessing(DataManager dataManager, IDialogCoordinator dialogCoordinator)
//        {
//            this.dataManager = dataManager;
//            this.dialogCoordinator = dialogCoordinator;
//        }

//        //public HalconImageProcessing(HWindow window)
//        //{
//        //    Window = window;

//        //    #region Loading test image.
//        //    //Image = new HImage("pcb");
//        //    //Image.DispObj(Window);
//        //    #endregion
//        //}

//        public void GrapStart()
//        {
//            imageGrapTimer.Start();
//            IsGrabStart = true;
//        }

//        public void GrapStop()
//        {
//            imageGrapTimer.Stop();
//            IsGrabStart = false;
//        }

//        public void Open(HWindow targetWindow, string configurationFileWithoutExtension, bool simulator)
//        {
//            try
//            {
//                Window = targetWindow;

//                ROI = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, 100, 100, 210, 210);
//                var row1 = ROI.GetDrawingObjectParams("row1");
//                var column1 = ROI.GetDrawingObjectParams("column1");
//                var row2 = ROI.GetDrawingObjectParams("row2");
//                var column2 = ROI.GetDrawingObjectParams("column2");

//                ROI.SetDrawingObjectParams("color", "green");
//                this.AttachDrawObj(ROI/*selected_drawing_object*/);
//                ShapeModel = new HShapeModel();



//                if (!simulator)
//                {
//                    Framegrabber = new HFramegrabber("MultiCam", 1, 1, 0, 0, 0, 0, "default", -1, "default", -1, "false", configurationFileWithoutExtension, "0", 1, -1);
//                    IsInitialized = Framegrabber.IsInitialized();
//                    Image = Framegrabber.GrabImage();

//                    DisplayWindow(Window);
//                    //Image.DispObj(Window);

//                    imageGrapTimer = new DispatcherTimer();
//                    imageGrapTimer.Interval = TimeSpan.FromMilliseconds(100);
//                    imageGrapTimer.Tick += ImageGrapTimer_Tick;
//                }

//                IsOpen = true;

//                //Window.SetDraw("margin");
//                //Window.SetLineWidth(3);

//                //Rectangle = new HRegion(188.0, 182, 298, 412);
//                //Rectangle.AreaCenter(out Row, out Column);
//                //Rect1Row = Row - 102;
//                //Rect1Col = Column + 5;
//                //Rect2Row = Row + 107;
//                //Rect2Col = Column + 5;
//                //RectPhi = 0;
//                //RectLength1 = 170;
//                //RectLength2 = 5;

//                //Window.SetColor("blue");
//                //Window.SetDraw("margin");
//                //Rectangle.DispObj(Window);
//            }
//            catch (Exception ex)
//            {
//                IsOpen = false;
//                throw ex;
//            }
//        }

//        public void DisplayWindow(HWindow tartget)
//        {
//            Window = tartget;
//            string imageType = string.Empty;
//            int imageWidth = 0;
//            int imageHeight = 0;

//            Image.GetImagePointer1(out imageType, out imageWidth, out imageHeight);
//            ImageType = imageType;
//            ImageWidth = imageWidth;
//            ImageHeight = imageHeight;
//            Window.SetPart(0, 0, imageHeight - 1, ImageWidth - 1);
//            Window.DispObj(Image);
//        }
//        private void ImageGrapTimer_Tick(object sender, EventArgs e)
//        {
//            if (IsInitialized)
//            {
//                GrabImage();
//            }
//        }

//        private void GrabImage()
//        {
//            Image = Framegrabber.GrabImage();
//            string imageType = string.Empty;
//            int imageWidth = 0;
//            int imageHeight = 0;
//            Image.GetImagePointer1(out imageType, out imageWidth, out imageHeight);
//            ImageType = imageType;
//            ImageWidth = imageWidth;
//            ImageHeight = imageHeight;
//            //Window.SetPart(0, 0, imageHeight - 1, ImageWidth - 1);

//            string color = "red";
//            var row = ImageHeight / 2.0;
//            var column = ImageWidth / 2.0;
//            var size = 40;
//            var angle = 0;
//            Window.SetColor(color);
//            HOperatorSet.DispCross(Window, row, column, size, angle);

//            Image.DispObj(Window);
//        }

//        public void SaveImage(string fileName)
//        {
//            HOperatorSet.WriteImage(Image, "tiff", 0, fileName);
//        }

//        public void LoadImage(string fileName)
//        {
//            HObject img;
//            HOperatorSet.ReadImage(out img, fileName);
//            this.Image = new HImage(img);
//            //img.DispObj(Window);
//        }

//        public (double x, double y) Find(out bool find_result)
//        {
//            find_result = false;

//            if (Image == null)
//            {
//                MessageBox.Show("등록된 이미지가 없습니다.");
//                return (0.0, 0.0);
//            }

//            HOperatorSet.GaussImage(Image, out var ho_SmoothedImage, 3);
//            HImage hImage = new HImage(ho_SmoothedImage);

//            ShapeModel.FindShapeModel(
//                //image: Builder.HalconImageProcessing.Image, 
//                image: hImage,
//                angleStart: 0,
//                angleExtent: new HTuple(1.0).TupleRad().D,
//                minScore: 0.7,
//                numMatches: 1,
//                maxOverlap: 0.5,
//                subPixel: "least_squares_very_high", // least_squares
//                numLevels: 4,
//                greediness: 0.9,  // default: 0.9
//                out var RowCheck,
//                out var ColumnCheck,
//                out var AngleCheck,
//                out var Score);

//            if (RowCheck.ToDArr().Length <= 0)
//            {
//                find_result = false;
//                return (0.0, 0.0);
//            }

//            var sc = Math.Round(Score.D, 4);

//            //Window.ClearWindow();
//            Window.DispObj(Image);
//            Window.SetColor("red");

//            int offset = 10;
//            Window.DispLine(RowCheck + offset, ColumnCheck, RowCheck - offset, ColumnCheck);
//            Window.DispLine(RowCheck, ColumnCheck + offset, RowCheck, ColumnCheck - offset);
//            Window.DispText(new HTuple($"{sc:F3}"), "image", new HTuple(RowCheck - offset - 10), new HTuple(ColumnCheck), new HTuple("blue"), new HTuple(), new HTuple());

//            HOperatorSet.GetImageSize(Image, out var width, out var height);
//            var cx = width.D / 2.0;
//            var cy = height.D / 2.0;

//            bool result;    
//            double resolution_x = this.dataManager.GET_DOUBLE(DataNameHelper.CAMERA_RESOLUTION_X, out result);
//            double resolution_y = this.dataManager.GET_DOUBLE(DataNameHelper.CAMERA_RESOLUTION_Y, out result);

//            var diff_x = (ColumnCheck.D - cx) * resolution_x / 50.0;
//            var diff_y = (RowCheck.D - cy) * resolution_y / 50.0 * -1;
//            find_result = true;

//            this.dataManager.SET_DATA(DataNameHelper.MARK_X_OFFSET, diff_x);
//            this.dataManager.SET_DATA(DataNameHelper.MARK_Y_OFFSET, diff_y);

//            MarkFindEventHandler?.Invoke(this, new MarkFindEventArgs() { Offset_X = diff_x, Offset_Y = diff_y, Score = Score });

//            return (diff_x, diff_y);

//            //lblXOffsetResult.Text = $"{diff_x:F6}";
//            //lblYOffsetResult.Text = $"{diff_y:F6}";
//        }

//        public BitmapImage RegisterModel()
//        {
//            try
//            {
//                HImage ImgReduced;

//                var col1 = ROI.GetDrawingObjectParams("column1");
//                var col2 = ROI.GetDrawingObjectParams("column2");
//                var row1 = ROI.GetDrawingObjectParams("row1");
//                var row2 = ROI.GetDrawingObjectParams("row2");
//                HRegion Rectangle1 = new HRegion(row1, col1, row2, col2);

//                Window.SetColor("red");
//                Window.SetDraw("margin");
//                Window.SetLineWidth(3);

//                if (Image == null)
//                {
//                    MessageBox.Show("등록 대상 이미지가 없습니다.");
//                    return null;    
//                }

//                ImgReduced = Image?.ReduceDomain(Rectangle1);
//                var imag = ImgReduced?.CropPart(row1, col1, col2 - col1, row2 - row1);
//                //imag.WriteImage("tiff", new HTuple(0), new HTuple(@"d:\test.tif"));                               

//                if (System.IO.File.Exists(registeredImageFileNameWithNoExtension))
//                {
//                    System.IO.File.Delete(registeredImageFileNameWithNoExtension);
//                }

//                imag?.WriteImage("jpeg", new HTuple(0), new HTuple(registeredImageFileNameWithNoExtension));

//                //ImgReduced.CreateShapeModel(5, 0, 360, 0.1, "", "", 10, 5);

//                ImgReduced?.InspectShapeModel(out modelRegion, 1, 30); // 30 -> 50
//                ShapeModel = new HShapeModel(
//                    template: ImgReduced,
//                    numLevels: 10,
//                    angleStart: 0,
//                    angleExtent: new HTuple(1.0).TupleRad().D,
//                    angleStep: new HTuple(1.0).TupleRad().D,
//                    optimization: "none",
//                    metric: "ignore_global_polarity",
//                    contrast: 10,  // 10 -> 20 -> 30
//                    minContrast: 5 // 5 -> 10 -> 20
//                    );

//                //  쉐이프 모델 저장/불러오기와 별도로 이미지도 관리 할 수 있도록 하자
//                //ShapeModel.ReadShapeModel(@"d:\model.shm");

//                ShapeModel.WriteShapeModel(modelFileName);

//                //if (pbRegisteredImage != null)
//                //{
//                //    pbRegisteredImage.Image = null;
//                //    pbRegisteredImage.Image.Dispose();
//                //}                

//                //var registeredImageFileNameWithNoExtension = JmpApplication.Root + @"final";
//                //ImgReduced.WriteImage("tiff", new HTuple(0), new HTuple(registeredImageFileNameWithNoExtension));

//                var bitmap = LoadBitmap(registeredImageFileNameWithNoExtension);

//                return ConvertBitmapToBitmapImage(bitmap);
//            }
//            catch (HalconException hex)
//            {   
//                MessageBox.Show(hex.Message);
//                return null;
//            }
//        }

//        private Bitmap LoadBitmap(string path)
//        {
//            if (!File.Exists(path)) return null;

//            // open file in read only mode
//            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
//            {
//                // get a binary reader for the file stream
//                using (BinaryReader reader = new BinaryReader(stream))
//                {
//                    // copy the content of the file into a memory stream
//                    var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
//                    // make a new Bitmap object the owner of the MemoryStream
//                    return new Bitmap(memoryStream);
//                }
//            }
//        }

//        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
//        {
//            using (MemoryStream memoryStream = new MemoryStream())
//            {
//                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
//                memoryStream.Position = 0;

//                BitmapImage bitmapImage = new BitmapImage();
//                bitmapImage.BeginInit();
//                bitmapImage.StreamSource = memoryStream;
//                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
//                bitmapImage.EndInit();
//                bitmapImage.Freeze(); // BitmapImage를 읽기 전용으로 만듭니다.

//                return bitmapImage;
//            }
//        }

//        private void AttachDrawObj(HDrawingObject obj)
//        {
//            drawing_objects.Add(obj);
//            // The HALCON/C# interface offers convenience methods that
//            // encapsulate the set_drawing_object_callback operator.
//            //obj.OnDrag(user_actions.SobelFilter);
//            //obj.OnAttach(user_actions.SobelFilter);
//            //obj.OnResize(user_actions.SobelFilter);
//            //obj.OnSelect(OnSelectDrawingObject);
//            //obj.OnAttach(user_actions.SobelFilter);
//            if (selected_drawing_object == null)
//                selected_drawing_object = obj;

//            if (Window != null)
//                Window.AttachDrawingObjectToWindow(obj);
//        }
//    }
//}
