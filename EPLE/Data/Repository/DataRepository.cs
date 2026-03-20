using EPLE.Data.Entity;
using EPLE.Interface;
using NHibernate;
using NHibernate.Linq;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace EPLE.Data.Repository
{
    public class DataRepository : ISaveChanges
    {
        private readonly ILogger logger;
        private ISession session;
        public DataRepository(ILogger logger)
        {
            this.logger = logger;
            this.session = NHibernateHelper.OpenSession();
            try
            {
                LoadData();
            }
            catch (Exception)
            {
                logger.Error("[Error] DataManager Setting Failed!!!");
                throw;
            }
        }

        public void LoadData()
        {
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Clear(); // NHibernate 프록시 제거 (중요!)

                    var dataConfigList = session.Query<DataConfigEntity>().AsEnumerable();
                    DataConfig = new HashSet<DataConfigEntity>(dataConfigList);
                    Console.WriteLine($"[DEBUG] DataConfig Loaded. Count: {DataConfig.Count}");

                    var deviceConfigList = session.Query<DeviceConfigEntity>().AsEnumerable();
                    DeviceConfig = new HashSet<DeviceConfigEntity>(deviceConfigList);
                    Console.WriteLine($"[DEBUG] DeviceConfig Loaded. Count: {DeviceConfig.Count}");

                    var alarmConfigList = session.Query<AlarmConfigEntity>().AsEnumerable();
                    AlarmConfig = new HashSet<AlarmConfigEntity>(alarmConfigList);
                    Console.WriteLine($"[DEBUG] AlarmConfig Loaded. Count: {AlarmConfig.Count}");

                    var measureConfigList = session.Query<MeasureConfigEntity>().AsEnumerable();
                    MeasureConfig = new HashSet<MeasureConfigEntity>(measureConfigList);
                    Console.WriteLine($"[DEBUG] MeasureConfig Loaded. Count: {MeasureConfig.Count}");

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.Error("LoadData Error: {@Error}", ex);
            }
           
        }

        public bool ChangeMotionDevice(string deviceName)
        {
            var device = DeviceConfig.FirstOrDefault(x => x.DeviceName == deviceName);

            if (device == null)
            {
                logger.Error("ChangeMotionDevice Error: Device Not Found");
                return false;
            }

            var motionData = DataConfig.Where(x => x.Module == "MOTION").ToList();

            if (motionData == null)
            {
                logger.Error("ChangeMotionDevice Error: Motion Data Not Found");
                return false;
            }

            foreach (var data in motionData)
            {
                data.DeviceName = deviceName;
            }

            this.SaveChanges();
            
            return true;
        }

        public virtual IEnumerable<DataConfigEntity> DataConfig_ { get; set; }
        public virtual IEnumerable<AlarmConfigEntity> AlarmConfig_ { get; set; }
        public virtual ISet<DataConfigEntity> DataConfig { get; set; }
        public virtual ISet<DeviceConfigEntity> DeviceConfig { get; set; } 
        public virtual ISet<AlarmConfigEntity> AlarmConfig { get; set; }

        public virtual ISet<MeasureConfigEntity> MeasureConfig { get; set; }

        public DataConfigEntity GetData(string name)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Query<DataConfigEntity>().FirstOrDefault(x => x.Name == name);
            }
        }

        public void DeleteAll<T>()
        {
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Query<T>().Delete<T>();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error("DeleteAll Error: {@Error}", ex);
                    transaction.Rollback();
                }
            }
        }

        public void UpdateRange<T>(IEnumerable<T> entities)
        {
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        session.Update(entity);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateRange Error: {@Error}", ex);
                    transaction.Rollback();
                }
            }
        }

        public void AddRange<T>(IEnumerable<T> entities)
        {
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        session.Save(entity);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error("AddRange Error: {@Error}", ex);
                    transaction.Rollback();
                }
            }
        }
        public void AddDeviceConfig(DeviceConfigEntity newEntity)
        {
            DeviceConfig.Add(newEntity);
            session.Save(newEntity);
        }

        public void UpdateDeviceConfig(DeviceConfigEntity updatedEntity)
        {
            var entity = DeviceConfig.FirstOrDefault(e => e.Id == updatedEntity.Id);
            if (entity != null)
            {
                DeviceConfig.Remove(entity);
                DeviceConfig.Add(updatedEntity);
                session.Update(updatedEntity);
            }
        }

        public void DeleteDeviceConfig(int id)
        {
            var entity = DeviceConfig.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                DeviceConfig.Remove(entity);
                session.Delete(entity);
            }
        }

        public void AddMeasureConfig(MeasureConfigEntity newEntity)
        {
            MeasureConfig.Add(newEntity);
            session.Save(newEntity);
        }

        public void UpdateMeasureConfig(MeasureConfigEntity updatedEntity)
        {
            var entity = MeasureConfig.FirstOrDefault(e => e.Id == updatedEntity.Id);
            if (entity != null)
            {
                MeasureConfig.Remove(entity);
                MeasureConfig.Add(updatedEntity);
                session.Update(updatedEntity);
            }
        }

        public void DeleteMeasureConfig(int id)
        {
            var entity = MeasureConfig.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                MeasureConfig.Remove(entity);
                session.Delete(entity);
            }
        }

        public void SaveChanges()
        {
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Flush(); // 한 번만 실행하여 변경된 모든 엔티티를 DB에 반영
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error("SaveChanges Error: {@Error}", ex);
                    transaction.Rollback();
                }
            }
        }
    }
}
