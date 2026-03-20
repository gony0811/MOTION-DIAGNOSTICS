using MotionDiagnostics.Model;
using MotionDiagnostics.UserControls.ViewModels;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;


namespace MotionDiagnostics.UserControls
{
    public partial class WaferControl : UserControl
    {
        private const double ZoomFactor = 1.1; // 줌 배율
        private const double MinScale = 0.5;   // 최소 스케일
        private const double MaxScale = 5.0;  // 최대 스케일

        private Point _startPoint;
        private bool _isDragging;
        private List<Button> _selectedButtonList = new List<Button>();
        private Button _centerButton;

        private WaferControlViewModel waferViewModel;

        //public static DependencyProperty IsMultiSelectionProperty = DependencyProperty.Register("IsMultiSelection", typeof(bool), typeof(WaferControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits));

        public static DependencyProperty SelectionsProperty = DependencyProperty.Register("Selections", typeof(IList), typeof(WaferControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits));

        private static DependencyProperty SelectedRowProperty = DependencyProperty.Register("SelectedRow", typeof(int), typeof(WaferControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits));

        private static DependencyProperty SelectedColumnProperty = DependencyProperty.Register("SelectedColumn", typeof(int), typeof(WaferControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits));

        private static DependencyProperty SelectionChangedProperty = DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(WaferControl), new PropertyMetadata(null));

        // 기존 코드...

        // RoutedEvent 정의
        public static readonly RoutedEvent WaferSelectionChangedEvent = EventManager.RegisterRoutedEvent(
            "WaferSelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WaferControl));

        // 이벤트 CLR 래퍼
        public event RoutedEventHandler WaferSelectionChanged
        {
            add { AddHandler(WaferSelectionChangedEvent, value); }
            remove { RemoveHandler(WaferSelectionChangedEvent, value); }
        }

        //public bool IsMultiSelection
        //{
        //    get { return (bool)GetValue(IsMultiSelectionProperty); }
        //    set { SetValue(IsMultiSelectionProperty, value); }
        //}

        public int SelectedRow
        {
            get { return (int)GetValue(SelectedRowProperty); }
            set
            {
                SetValue(SelectedRowProperty, value);
            }
        }

        public int SelectedColumn
        {
            get { return (int)GetValue(SelectedColumnProperty); }
            set
            {
                SetValue(SelectedColumnProperty, value);
            }
        }

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedProperty); }
            set { SetValue(SelectionChangedProperty, value); }
        }

        public IList Selections
        {
            get { return (IList)GetValue(SelectionsProperty); }
            set { SetValue(SelectionsProperty, value); }
        }


        // 2024.12.30 sean.kim
        // WafercOntrol UserControl 생성자에 WaferControlViewModel을 전달받아 ViewModel을 초기화
        public WaferControl()
        {
            InitializeComponent();
            Selections = new List<Tuple<int, int>>();
        }

        private void DrawCircularLayout()
        {
            if (waferViewModel?.WaferGrids == null || WaferCanvas == null) return;

            WaferCanvas.Children.Clear();
            foreach (var row in waferViewModel.WaferGrids)
            {
                foreach (var cell in row)
                {
                    if (cell != null && cell.ButtonInfo != null)
                    {
                        var button = cell.ButtonInfo;
                        var pos = button.Tag as Tuple<int, int>;
                        button.ToolTip = $"row:{pos.Item1}, col:{pos.Item2}";
                        AddButton(button);
                    }
                }
            }

            var center_col = waferViewModel.WaferGridColumnCount / 2 - 1;
            var center_row = waferViewModel.WaferGridRowCount / 2 - 1;

            waferViewModel.WaferGrids[center_row][center_col].ButtonInfo.Background = Brushes.LightGreen;
            _centerButton = waferViewModel.WaferGrids[center_row][center_col].ButtonInfo;

            DrawAxes();
        }

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            WaferControlViewModel vm = DataContext as WaferControlViewModel;

            // 멀티 셀렉션 버튼 Down 상태가 아닌 경우 초기화 후 선택
            if (vm.IsMultiSelection == false)
            {
                if (_selectedButtonList.Count > 0)
                {
                    foreach (var item in _selectedButtonList)
                    {
                        var button = item;
                        button.Background = Brushes.Transparent;
                    }

                    _selectedButtonList.Clear();
                }

                if (Selections.Count > 0)
                {
                    Selections.Clear();
                }

                if (sender is Button selected)
                {
                    var position = (Tuple<int, int>)selected.Tag;
                    selected.Background = Brushes.Red;
                    SelectedRow = position.Item1;
                    SelectedColumn = position.Item2;
                    _selectedButtonList.Add(selected);
                    Selections.Add(position);
                }
            }
            else if (vm.IsMultiSelection == true)
            {
                if (sender is Button selected)
                {
                    var position = (Tuple<int, int>)selected.Tag;
                    selected.Background = Brushes.Red;
                    _selectedButtonList.Add(selected);
                    SelectedRow = position.Item1;
                    SelectedColumn = position.Item2;
                    Selections.Add(position);
                }
            }

            // WaferSelectionChanged 이벤트 발생
            RaiseEvent(new RoutedEventArgs(WaferSelectionChangedEvent));
        }

        // 화살표 및 축 그리기 (초기 설정)
        private void DrawAxes()
        {
            // X 축
            Line xAxis = new Line
            {
                X1 = 10,
                Y1 = 40,
                X2 = 40,
                Y2 = 40,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            Panel.SetZIndex(xAxis, 10); // 높은 Z-Index 설정

            // X축 화살표
            Polygon xArrow = new Polygon
            {
                Points = new PointCollection { new Point(40, 40), new Point(35, 35), new Point(35, 45) },
                Fill = Brushes.Red
            };
            Panel.SetZIndex(xArrow, 10);

            // X축 라벨
            TextBlock xLabel = new TextBlock
            {
                Text = "X",
                Foreground = Brushes.Red,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(xLabel, 40);
            Canvas.SetTop(xLabel, 35);
            Panel.SetZIndex(xLabel, 10);

            // Y 축
            Line yAxis = new Line
            {
                X1 = 10,
                Y1 = 40,
                X2 = 10,
                Y2 = 10,
                Stroke = Brushes.Green,
                StrokeThickness = 2
            };
            Panel.SetZIndex(yAxis, 10);

            // Y축 화살표 (위 방향으로 변경)
            Polygon yArrow = new Polygon
            {
                Points = new PointCollection { new Point(10, 10), new Point(5, 15), new Point(15, 15) },
                Fill = Brushes.Green
            };
            Panel.SetZIndex(yArrow, 10);

            // Y축 라벨
            TextBlock yLabel = new TextBlock
            {
                Text = "Y",
                Foreground = Brushes.Green,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(yLabel, 15);
            Canvas.SetTop(yLabel, 0);
            Panel.SetZIndex(yLabel, 10);

            // 추가
            AxesCanvas.Children.Add(xAxis);
            AxesCanvas.Children.Add(xArrow);
            AxesCanvas.Children.Add(xLabel);
            AxesCanvas.Children.Add(yAxis);
            AxesCanvas.Children.Add(yArrow);
            AxesCanvas.Children.Add(yLabel);
        }

        private void AddButton(Button button)
        {
            var position = (Tuple<int, int>)button.Tag;
            // 버튼을 Canvas에 배치
            Canvas.SetLeft(button, position.Item2 * waferViewModel.buttonSize);
            Canvas.SetBottom(button, position.Item1 * waferViewModel.buttonSize);
            button.IsHitTestVisible = true;
            button.PreviewMouseLeftButtonDown += Button_Clicked;
            Panel.SetZIndex(button, 1);
            WaferCanvas.Children.Add(button);
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (CanvasScaleTransform == null) return;

            // 현재 스케일 가져오기
            double currentScale = CanvasScaleTransform.ScaleX;

            // 줌 인/아웃 계산
            if (e.Delta > 0 && currentScale < MaxScale)
            {
                // 줌 인
                CanvasScaleTransform.ScaleX *= ZoomFactor;
                CanvasScaleTransform.ScaleY *= ZoomFactor;
            }
            else if (e.Delta < 0 && currentScale > MinScale)
            {
                // 줌 아웃
                CanvasScaleTransform.ScaleX /= ZoomFactor;
                CanvasScaleTransform.ScaleY /= ZoomFactor;
            }

            // Zoom 중점 위치 보정
            Point mousePosition = e.GetPosition(CanvasScrollViewer);
            WaferCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null) return;

            waferViewModel = DataContext as WaferControlViewModel;
            this.waferViewModel.WaferGridRefresh();
            DrawCircularLayout();
        }


        private void Viewbox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Viewbox viewbox = sender as Viewbox;
            if (viewbox != null && viewbox.ContextMenu != null)
            {
                viewbox.ContextMenu.IsOpen = true;
            }
        }

        private void MenuItem_Resize_Click(object sender, RoutedEventArgs e)
        {
            CanvasScaleTransform.ScaleX = 1.0;
            CanvasScaleTransform.ScaleY = 1.0;

            WaferCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
            CanvasTranslateTransform.X = 0;
            CanvasTranslateTransform.Y = 0;
        }

        private void WaferViewbox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(WaferCanvas);
            _isDragging = true;
            WaferViewbox.CaptureMouse();
        }

        private void WaferViewbox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPoint = e.GetPosition(CanvasScrollViewer);

                double offsetX = currentPoint.X - _startPoint.X;
                double offsetY = currentPoint.Y - _startPoint.Y;

                CanvasTranslateTransform.X = offsetX;
                CanvasTranslateTransform.Y = offsetY;
            }

            if (waferViewModel.IsDragSelection)
            {
                Mouse.OverrideCursor = Cursors.Cross;
            }
            else
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void WaferViewbox_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            WaferViewbox.ReleaseMouseCapture();
        }

        private void WaferViewbox_MouseEnter(object sender, MouseEventArgs e)
        {
            if(waferViewModel.IsDragSelection)
                Mouse.OverrideCursor = Cursors.Cross;
            else
                Mouse.OverrideCursor = null;
        }

        private void WaferViewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
    }
}