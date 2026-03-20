using EPLE.Data;
using EPLE.Data.Entity;
using EPLE.Manager.Alarm;
using EPLE.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using static EPLE.ViewModel.AlarmVMList;
using System.Linq;
using EPLE.Data.Repository;
using Serilog;

namespace EPLE.Manager
{
    public class AlarmEventArgs : EventArgs
    {
        public List<AlarmVM> Alarms { get; set; }
        public AlarmVM Alarm { get; set; }
        public AlarmEventArgs(AlarmVM alarm)
        {
            Alarm = alarm;
        }

        public AlarmEventArgs(List<AlarmVM> alarms)
        {
            Alarms = alarms;
        }
    }
    public class AlarmManager
    {
        private readonly ILogger logger;
        private readonly object eventLock = new object();
        private readonly DataManager dataManager;
        private readonly DataRepository dataRepository;
        private readonly AlarmVMList alarmVMList;

        private EventHandler<AlarmEventArgs> setAlarmEvent;
        private EventHandler<AlarmEventArgs> resetAlarmEvent;

        public AlarmManager(ILogger logger, DataManager dataManager, DataRepository dataRepository, AlarmVMList alarmVMList)
        {
            this.logger = logger;
            this.dataManager = dataManager;
            this.dataRepository = dataRepository;
            this.alarmVMList = alarmVMList;

            foreach (var alarmConfig in dataRepository.AlarmConfig)
            {
                alarmVMList.AlarmList.Add(new AlarmVMList.AlarmVM(alarmConfig, dataRepository));
            }
        }

        public event EventHandler<AlarmEventArgs> SetAlarmEvent
        {
            add
            {
                lock (eventLock)
                {
                    setAlarmEvent += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    setAlarmEvent -= value;
                }
            }
        }

        public event EventHandler<AlarmEventArgs> ResetAlarmEvent
        {
            add
            {
                lock (eventLock)
                {
                    resetAlarmEvent += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    resetAlarmEvent -= value;
                }
            }
        }

        public void SetAlarm(string alarmName)
        {
            var alarm = alarmVMList.AlarmList.First(x => x.Name == alarmName);
            if (alarm != null)
            {
                alarm.Status = ALST.SET;
                alarm.SaveChanges();
                setAlarmEvent?.Invoke(this, new AlarmEventArgs(alarm));
            }
            else
            {
                alarm = new AlarmVMList.AlarmVM(new AlarmConfigEntity(), dataRepository);
                alarm.Name = alarmName;
                alarm.Status = ALST.SET;
                alarm.Text = "NOT_DEFINE_ALARM";
                alarm.Level = ALCD.HEAVY;
                alarm.Enable = ALED.ENABLE;
                alarm.Description = "NOT_DEFINE_ALARM";
                alarmVMList.AlarmList.Add(alarm);
                alarm.SaveChanges();
                setAlarmEvent?.Invoke(this, new AlarmEventArgs(alarm));
            }
        }
        public async Task<TaskResponseType> SetAlarmAsync(string alarmName)
        {
            var alarm = alarmVMList.AlarmList.FirstOrDefault(x => x.Name == alarmName);
            if (alarm != null)
            {
                alarm.Status = ALST.SET;
                alarm.SaveChanges();
                setAlarmEvent?.Invoke(this, new AlarmEventArgs(alarm));
            }
            else
            {
                alarm = new AlarmVMList.AlarmVM(new AlarmConfigEntity(), dataRepository)
                {
                    Name = alarmName,
                    Status = ALST.SET,
                    Text = "NOT_DEFINE_ALARM",
                    Level = ALCD.HEAVY,
                    Enable = ALED.ENABLE,
                    Description = "NOT_DEFINE_ALARM"
                };
                alarmVMList.AlarmList.Add(alarm);
                alarm.SaveChanges();
                setAlarmEvent?.Invoke(this, new AlarmEventArgs(alarm));
            }

            // TaskCompletionSource로 사용자 선택 결과 대기
            return await AlarmResponseAwaiter.GetUserResponseAsync(alarm);
        }


        public void ResetAlarm(string alarmName)
        {
            if (alarmVMList.AlarmList.Any(x => x.Name == alarmName))
            {
                var alarm = alarmVMList.AlarmList.First(x => x.Name == alarmName);

                alarm.Status = ALST.RESET;
                alarm.SaveChanges();
                resetAlarmEvent?.Invoke(this, new AlarmEventArgs(alarm));
            }
        }

        public void ResetAllAlarm()
        {
            foreach (var alarm in alarmVMList.AlarmList)
            {
                if (alarm.Status == ALST.SET)
                {
                    alarm.Status = ALST.RESET;
                    resetAlarmEvent?.Invoke(this, new AlarmEventArgs(alarm));
                }
            }
        }

        public List<AlarmVM> GetCurrentSetAlarms()
        {
            return alarmVMList.AlarmList.Where(x => x.Status == ALST.SET).ToList();
        }
    }
}
