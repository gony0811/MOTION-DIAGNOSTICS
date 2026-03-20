using EPLE.Data;
using EPLE.Manager;
using EPLE.Service;
using EPLE.ViewModel;
using EPLE.Core.Service;

using MotionDiagnostics.Properties;
using PrismCommands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MappingAndMeasureControlViewModel
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly DataManager dataManager;
        private readonly AccuracyMeasureService accuracyMeasureService = null;
        private readonly MeasureVMList measureVMList;
        private readonly MeasureManager measureManager;

        private readonly RepeatMeasureControlViewModel repeatMeasureControlViewModel;

        public List<Tuple<int, int>> Selections  { get; set; }

        public MappingAndMeasureControlViewModel(DataManager dataManager, AccuracyMeasureService accuracyMeasureService, MeasureManager measureManager, MeasureVMList measureVMList, RepeatMeasureControlViewModel repeatMeasureControlViewModel)
        {
            this.dataManager = dataManager;
            this.measureVMList = measureVMList;
            this.accuracyMeasureService = accuracyMeasureService;
            this.measureManager = measureManager;
            this.repeatMeasureControlViewModel = repeatMeasureControlViewModel;
        }

        [DelegateCommand]
        public async Task Measure()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_MEASURE");

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            var measurePoints = this.measureVMList.Measures.Where((item) => { return item.IsMeasurePoint; }).ToList();

            if (measurePoints == null || measurePoints.Count == 0)
            {
                MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_MEASURE_NOSELECTION"), title);
                return;
            }

            try
            {

                var MeasureResults = repeatMeasureControlViewModel.MeasureResults;

                MeasureResults.Clear();

                foreach (var pt in measurePoints)
                {
                    var selectedRow = pt.YPos;
                    var selectedColumn = pt.XPos;
                    this.accuracyMeasureService.SelectedCol = selectedColumn;
                    this.accuracyMeasureService.SelectedRow = selectedRow;
                    this.accuracyMeasureService.RepeatCount = 1;

                    //MeasureResults.Clear();
                    //await measureManager.ResetAllMeasureData();

                    var Result = await this.accuracyMeasureService.Measure(token);

                    foreach (var data in this.accuracyMeasureService.MeasureData)
                    {
                        var measure = this.measureVMList.Measures.Where((item) => { return item.XPos == selectedColumn && item.YPos == selectedRow; }).First();

                        var clone = measure.Clone();

                        clone.Name = $"{clone.Name}({MeasureResults.Count + 1})";
                        clone.ErrorX = data.Item1;
                        clone.ErrorY = data.Item2;

                        MeasureResults.Add(clone);
                    }

                    if (Result == Result.SUCCESS)
                    {
                        Debug.WriteLine($"Measuring X : {selectedColumn}, Y : {selectedRow} - Success.");
                    }
                    else if (Result == EPLE.Core.Service.Result.FAILED)
                    {
                        Debug.WriteLine($"Measuring X : {selectedColumn}, Y : {selectedRow} - Failed.");
                    }
                    else if (Result == Result.CANCELED)
                    {
                        MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_MEASURE_CANCELED"), title);
                        return;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_MEASURE_CANCELED"), title);
                Debug.WriteLine("측정 작업이 취소되었습니다.");
            }
        }

        private void Measure_Canceled(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }
    }
}
