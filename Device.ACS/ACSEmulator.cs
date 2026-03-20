using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Device
{
    internal class ACSEmulator
    {
        private const double homingVelocity = 10.0;
        public string Name { get; set; }
        public int Index { get; set; } = 0;

        public bool IsEnable { get; set; } = false;

        /// <summary>
        /// 단위 : mm/s 
        /// </summary>
        public double SetVelocity { get; set; } = 0.0;
        /// <summary>
        /// 단위 : count tick
        /// </summary>
        public double Encoder { get; set; } = 0.0;

        /// <summary>
        /// 단위 : mm
        /// </summary>
        public double ActualPosition { get; set; } = 0.0;

        public double ActualVelocity { get; set; } = 0.0;

        /// <summary>
        /// 단위 : mm
        /// </summary>
        public double SetPosition { get; set; } = 0.0;
        public bool IsCalibrated { get; set; } = false;

        public bool Busy { get; set; } = false;

        public bool IsLimitPlus { get; set; } = false;

        public bool IsLimitMinus { get; set; } = false;

        public bool InPosition { get; set; } = false;

        public bool Stop { get; set; } = false;

        private bool _eStop = false;

        private Timer EmulationTimer;

        public ACSEmulator(string Name, int index)
        {
            this.Name = Name;
            this.Index = index;
            EmulationTimer = new Timer(RunCallback, null, 0, 10);
        }

        private void StopCheck()
        {
            if (ActualVelocity == 0.0)
            {
                Stop = true;
            }
            else
            {
                Stop = false;
            }
        }

        private bool InPositionCheck()
        {
            if (Stop)
            {
                InPosition = true;
                Busy = false;
            }
            else if (Math.Abs(ActualPosition - SetPosition) < 0.1)
            {
                InPosition = true;
                Busy = false;
            }
            else
            {
                InPosition = false;
                Busy = true;
            }

            return InPosition;
        }

        private void RunCallback(object state)
        {
            InPositionCheck();
            StopCheck();

            if (Stop)
            {
                return;
            }

            if (IsEnable && !InPosition)
            {
                if (SetPosition > ActualPosition)
                {
                    ActualPosition += ActualVelocity * 0.01; /// mm/s * 0.01s
                }
                else if (SetPosition < ActualPosition)
                {
                    ActualPosition -= ActualVelocity * 0.01; /// mm/s * 0.01s
                }
                else
                {
                    // InPosition
                }
            }
        }

        public void EStop()
        {
            ActualVelocity = 0.0;
        }

        public void Enable(bool bEnable)
        {
            IsEnable = bEnable;
        }

        public void MoveRelative(double distance)
        {
            ActualVelocity = SetVelocity;
            SetPosition = ActualPosition + distance;
            _eStop = false;
        }

        public void MoveAbsolute(double position)
        {
            ActualVelocity = SetVelocity;
            SetPosition = position;
            _eStop = false;
        }

        public async void ExecuteHome()
        {
            ActualVelocity = homingVelocity;
            _eStop = false;

            await Task.Run(new Action(() => {

                while (!InPosition)
                {
                    // wait
                    Task.Delay(10);
                }

                IsCalibrated = true;

            }));
        }

        // 250121_mh.yun
        public void JogPlus()
        {
            ActualVelocity = SetVelocity;
            _eStop = false;
        }

        public void JogMinus()
        {
            ActualVelocity = SetVelocity * -1;
            _eStop = false;
        }

        public void JogStop()
        {
            ActualVelocity = 0.0;
            _eStop = true;
        }



    }
}
