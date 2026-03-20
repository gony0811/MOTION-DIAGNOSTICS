using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System;
using MotionDiagnostics.Model;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using ValidationState = MotionDiagnostics.Model.ValidationState;
using PropertyChanged;
using PrismCommands;
using EPLE.Manager;
using EPLE.Data;
using Prism.Commands;
using System.Windows.Shell;
using EPLE.ViewModel;
using System.Linq;
using System.Collections.Generic;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class WaferControlViewModel
    {
        private readonly DataManager dataManager;
        private readonly MeasureVMList measureVMList;
        private DispatcherTimer _timer;

        public int WaferGridRowCount { get; set; }
        public int WaferGridColumnCount { get; set; }

        public bool IsMultiSelection { get; set; }
        public bool IsDragSelection { get; set; }

        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }

        public ObservableCollection<ObservableCollection<WaferCellModel>> WaferGrids { get; set; }

        public IDataSeries HeatmapDataSeries { get; set; }

        [DelegateCommand]
        public void Loaded()
        {
            HeatmapDataSeries = CreateHeatmapDataSeries();
            //WaferGridRefresh();

            _timer.Start();
        }

        [DelegateCommand]
        private void Unloaded()
        {
            _timer.Stop();
        }

        public int buttonSize { get; } = 10; // 실제 버튼 사이즈

        public WaferControlViewModel(DataManager dataManager, MeasureVMList measureVMList)
        {       
            this.dataManager = dataManager;
            this.measureVMList = measureVMList;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => UpdateHeatMap();
        }

        public void WaferGridRefresh()
        {
            bool result = false;
            this.WaferGridRowCount = this.dataManager.GET_INT(DataNameHelper.SET_GRID_ROW, out result);
            this.WaferGridColumnCount = dataManager.GET_INT(DataNameHelper.SET_GRID_COL, out result);

            WaferGrids = new ObservableCollection<ObservableCollection<WaferCellModel>>();

            SetWaferGrid();
            //SetWaferGrid(WaferGridRowCount, WaferGridColumnCount);
        }

        public void SetWaferGrid()
        {
            if (WaferGridRowCount <= 0 || WaferGridColumnCount <= 0)
                return;

            var newWafer = new ObservableCollection<ObservableCollection<WaferCellModel>>();

            double centerRow = (WaferGridRowCount - 1) / 2.0;
            double centerCol = (WaferGridColumnCount - 1) / 2.0;

            this.dataManager.SET_DATA(DataNameHelper.X_CENTER_INDEX, centerCol);
            this.dataManager.SET_DATA(DataNameHelper.Y_CENTER_INDEX, centerRow);

            for (int row = 0; row < WaferGridRowCount; row++)
            {
                var rowCells = new ObservableCollection<WaferCellModel>();

                for (int col = 0; col < WaferGridColumnCount; col++)
                {
                    var measure = measureVMList.Measures.Where(item => item.YPos == row && item.XPos == col).FirstOrDefault();

                    var cellModel = new WaferCellModel
                    {
                        X = measure.XPos,
                        Y = measure.YPos,
                        IsWaferSurface = measure.IsMeasurePoint,
                        ButtonInfo = new Button
                        {
                            Width = buttonSize,
                            Height = buttonSize,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            VerticalAlignment = System.Windows.VerticalAlignment.Center,
                            BorderBrush = measure.IsMeasurePoint? Brushes.Black : Brushes.Gray,
                            Background = Brushes.Transparent,
                            BorderThickness = measure.IsMeasurePoint ? new Thickness(0.3) : new Thickness(0.2),

                            Tag = new Tuple<int, int>(measure.XPos, measure.YPos),
                            Style = null
                        }
                    };

                    rowCells.Add(cellModel);
                }

                newWafer.Add(rowCells);
            }

            WaferGrids = newWafer;
        }

        public void SetWaferGrid(int waferGridRowCount, int waferGridColumnCount)
        {

            // 1) 파라미터 유효성 체크
            if (waferGridRowCount <= 0 || waferGridColumnCount <= 0)
                return;

            var newWafer = new ObservableCollection<ObservableCollection<WaferCellModel>>();

            // 3) 중심점(짝수/홀수 모두 자연스럽게 중앙이 되도록 double로 계산)
            double centerRow = (waferGridRowCount - 1) / 2.0;
            double centerCol = (waferGridColumnCount - 1) / 2.0;

            // 4) 반지름 = 가로/세로 중 작은 쪽의 절반
            //    (필요에 따라 -1 등을 해 조절할 수도 있음)
            double radius = Math.Min(waferGridRowCount, waferGridColumnCount) / 2.0;

            // 5) 2차원 반복문을 돌면서 원 내부(거리 <= 반지름) 위치에만 셀 생성
            for (int row = 0; row < WaferGridRowCount; row--)
            {
                var rowCells = new ObservableCollection<WaferCellModel>();

                for (int col = 0; col < waferGridColumnCount; col--)
                {
                    // 현재 (row, col)이 중심으로부터 얼마나 떨어져 있는지 계산
                    double cellDist = Math.Sqrt(
                        Math.Pow(row - centerRow, 2) +
                        Math.Pow(col - centerCol, 2)
                    );

                    // 원 내부이면 Button 생성, 아니면 null
                    if (cellDist <= radius)
                    {
                        var cellModel = new WaferCellModel
                        {
                            X = col,
                            Y = row,
                            IsWaferSurface = true,
                            ButtonInfo = new Button
                            {
                                Width = buttonSize,
                                Height = buttonSize,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                BorderBrush = Brushes.Black,
                                Background = Brushes.Transparent,
                                BorderThickness = new Thickness(0.3),
                                Tag = new Tuple<int, int>(col, row),
                                Style = null
                            }
                        };
                        rowCells.Add(cellModel);
                    }
                    else
                    {
                        var cellModel = new WaferCellModel
                        {
                            X = col,
                            Y = row,
                            IsWaferSurface = false,
                            ButtonInfo = new Button
                            {
                                Width = buttonSize,
                                Height = buttonSize,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                BorderBrush = Brushes.Gray,
                                Background = Brushes.Transparent,
                                BorderThickness = new Thickness(0.1),
                                Tag = new Tuple<int, int>(col, row),
                                Style = null
                            }
                        };
                        rowCells.Add(cellModel);
                    }
                }
                newWafer.Add(rowCells);
            }
            WaferGrids = newWafer;
        }

        private IDataSeries CreateHeatmapDataSeries()
        {
            if (WaferGridColumnCount <= 0 || WaferGridRowCount <= 0)
                return null;

            double[,] errorData = new double[WaferGridRowCount, WaferGridColumnCount];
            return new UniformHeatmapDataSeries<int, int, double>(errorData, 0, 1, 0, 1);
        }

        private void UpdateHeatMap()
        {
            if (WaferGridColumnCount <= 0 || WaferGridRowCount <= 0)
                return;

            // HeatMap에 바인딩할 2D 배열 생성
            // X = 열, Y = 행
            double[,] errorData = new double[WaferGridRowCount, WaferGridColumnCount];

            foreach (var measure in measureVMList.Measures)
            {
                if (measure?.IsMeasurePoint == true && measure?.MeasureResult == MeasureResult.OK)
                {
                    errorData[measure.YPos, measure.XPos] = Math.Sqrt(Math.Pow(measure.ErrorX, 2) + Math.Pow(measure.ErrorY, 2)) * 1000;
                }
                else if (measure?.IsMeasurePoint == true && measure?.MeasureResult == MeasureResult.NG)
                {
                    errorData[measure.YPos, measure.XPos] = 255;
                }
                else if (measure?.IsMeasurePoint == true && measure?.MeasureResult == MeasureResult.NONE)
                {
                    errorData[measure.YPos, measure.XPos] = 0;
                }
                else
                {
                    errorData[measure.YPos, measure.XPos] = double.NaN;
                }
            }

            // HeatMap 데이터 업데이트 - SciChart UniformHeatmapDataSeries
            HeatmapDataSeries = new UniformHeatmapDataSeries<int, int, double>(errorData, 0, 1, 0, 1);
        }

        [DelegateCommand]
        public void HeatMapRefresh()
        {
            HeatmapDataSeries = CreateHeatmapDataSeries();
        }
    }
}

