using EPLE.Data;
using EPLE.Data.Entity;
using EPLE.ViewModel;
using System.Collections.Generic;
using static EPLE.ViewModel.AlarmVMList;
using System.Linq;
using EPLE.Data.Repository;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EPLE.Manager
{
    public class MeasureManager
    {
        private readonly ILogger logger;
        private readonly DataRepository dataRepository;
        private readonly MeasureVMList measureVMList;

        public MeasureManager(ILogger logger, DataRepository dataRepository, MeasureVMList measureVMList)
        {
            this.logger = logger;
            this.dataRepository = dataRepository;
            this.measureVMList = measureVMList;

            foreach (var measureConfig in dataRepository.MeasureConfig)
            {
                measureVMList.Measures.Add(new MeasureVMList.MeasureVM(measureConfig, dataRepository));
            }
        }


        public void ResetAllMeasureData()
        {
            foreach (var measure in measureVMList.Measures)
            {
                measure.MeasureResult = MeasureResult.NONE;
                measure.ErrorX = 0;
                measure.ErrorY = 0;
                measure.UpdateTime = "";
            }

            dataRepository.SaveChanges();
        }

        /// <summary>
        /// 측정할 Wafer의 측정 위치 정보를 생성 한다.
        /// 웨이퍼는 원형으로 생각하고, 원의 좌측 상단을 기준으로 X, Y 좌표를 생성한다.
        /// 웨이퍼 반지름까지 거리를 계산하고 반경 내에 있는 좌표는 IsMeasurePoint를 true로 설정한다.
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public void CreateMeasureData(int rowCount, int colCount)
        {
            // 1) 파라미터 유효성 체크
            if (rowCount <= 0 || colCount <= 0)
                return;

            // 3) 중심점(짝수/홀수 모두 자연스럽게 중앙이 되도록 double로 계산)
            double centerRow = (rowCount - 1) / 2.0;
            double centerCol = (colCount - 1) / 2.0;

            // 4) 반지름 = 가로/세로 중 작은 쪽의 절반
            //    (필요에 따라 -1 등을 해 조절할 수도 있음)
            double radius = Math.Min(rowCount, colCount) / 2.0;

            var measureConfig = new List<MeasureConfigEntity>();
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {

                    // 5) 현재 좌표(row, col)와 중심점 사이의 거리 계산
                    double dist = Math.Sqrt(
                        Math.Pow(row - centerRow, 2) +
                        Math.Pow(col - centerCol, 2)
                    );

                    // 원 내부이면 Wafer Surface 생성, 아니면 dummy 생성
                    if (dist <= radius)
                    {
                        var measure = new MeasureConfigEntity
                        {
                            Name = $"MEASURE_{row:D4}-{col:D4}_W",
                            XPos = row,
                            YPos = col,
                            IsMeasurePoint = true,
                            ErrorX = 0.0,
                            ErrorY = 0.0,
                            MeasureResult = MeasureResult.NONE
                        };

                        measureConfig.Add(measure);
                    }
                    else
                    {
                        var dummy = new MeasureConfigEntity
                        {
                            Name = $"MEASURE_{row:D4}-{col:D4}_D",
                            XPos = row,
                            YPos = col,
                            IsMeasurePoint = false,
                            ErrorX = 0.0,
                            ErrorY = 0.0,
                            MeasureResult = MeasureResult.NONE
                        };

                        measureConfig.Add(dummy);
                    } 
                }
            }

            dataRepository.DeleteAll<MeasureConfigEntity>();
            dataRepository.AddRange<MeasureConfigEntity>(measureConfig);
            

            measureVMList.Measures.Clear();

            foreach (var measure in measureConfig)
            {
                measureVMList.Measures.Add(new MeasureVMList.MeasureVM(measure, dataRepository));
            }
        }
    }
}
