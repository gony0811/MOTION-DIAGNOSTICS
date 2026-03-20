using EPLE.Core.Service;
using EPLE.Data;
using EPLE.Manager;
using EPLE.Service;
using EPLE.ViewModel;

using MotionDiagnostics.Properties;
using PrismCommands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Controls.Primitives;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RepeatMeasureControlViewModel : IDataErrorInfo
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public string Error => throw new NotImplementedException();

        private readonly DataManager dataManager;
        private readonly AccuracyMeasureService accuracyMeasureService = null;
        private readonly MeasureVMList measureVMList;
        private readonly MeasureManager measureManager;

        public RepeatMeasureControlViewModel(DataManager dataManager, AccuracyMeasureService accuracyMeasureService, MeasureManager measureManager, MeasureVMList measureVMList)
        {
            this.dataManager = dataManager;
            this.measureVMList = measureVMList;
            this.accuracyMeasureService = accuracyMeasureService;
            this.measureManager = measureManager;
        }


        public string this[string columnName]
        {
            get
            {
                if (columnName == "RepeatCount")
                {
                    if (RepeatCount <= 0)
                    {
                        return "repeat must be greater than 0";
                    }
                }
                else if (columnName == "SelectedRow")
                {
                    if (SelectedRow < 0)
                    {
                        return "Selected row must be greater than or equal to 0";
                    }
                }
                else if (columnName == "SelectedColumn")
                {
                    if (SelectedColumn < 0)
                    {
                        return "Selected column must be greater than equal to 0";
                    }
                }

                return "";
            }
        }

        public int SelectedRow { get; set; }
        public int SelectedColumn { get; set; }
        public int RepeatCount { get; set; }

        public MeasureVMList.MeasureVM MeasureResult { get; set; }

        public ObservableCollection<MeasureVMList.MeasureVM> MeasureResults { get; set; } = new ObservableCollection<MeasureVMList.MeasureVM>();

        [DelegateCommand]
        public async Task RepeatMeasure()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_REPEATMEASURE");

            try
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;

                this.accuracyMeasureService.SelectedCol = SelectedColumn;
                this.accuracyMeasureService.SelectedRow = SelectedRow;
                this.accuracyMeasureService.RepeatCount = RepeatCount;

                //MeasureResults.Clear();
                //await measureManager.ResetAllMeasureData();

                var Result = await this.accuracyMeasureService.StandingRepeatMeasure(token);

                if (Result == Result.SUCCESS)
                {
                    foreach (var data in this.accuracyMeasureService.MeasureData)
                    {
                        var measure = this.measureVMList.Measures.Where((item) => { return item.XPos == SelectedColumn && item.YPos == SelectedRow; }).First();

                        var clone = measure.Clone();

                        clone.Name = $"{clone.Name}({MeasureResults.Count + 1})";
                        clone.ErrorX = data.Item1;
                        clone.ErrorY = data.Item2;

                        MeasureResults.Add(clone);
                    }
                }

                if (Result == EPLE.Core.Service.Result.FAILED)
                {
                    System.Windows.MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_REPEATMEASURE_FAILED"), title);
                }
                else if (Result == Result.CANCELED)
                {
                    System.Windows.MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_REPEATMEASURE_CANCELED"), title);
                }
                else if (Result == Result.SUCCESS)
                {
                    System.Windows.MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_REPEATMEASURE_SUCCESS"), title);
                }
            }
            catch (OperationCanceledException)
            {
                System.Windows.MessageBox.Show(Resources.ResourceManager.GetString("MB_MSG_REPEATMEASURE_CANCELED"), title);

                Debug.WriteLine("측정 작업이 취소되었습니다.");
            }
            finally
            {
                MeasureResult = this.measureVMList.Measures.Where((item) => { return item.XPos == SelectedColumn && item.YPos == SelectedRow; }).First();
            }


        }

        private void RepeatMeasure_Canceled(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }
    }
}
