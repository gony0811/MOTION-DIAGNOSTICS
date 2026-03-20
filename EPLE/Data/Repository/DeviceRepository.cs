using EPLE.Data.Entity;
using EPLE.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data.Repository
{
    public class DeviceRepository : ISaveChanges
    {
        private readonly ILogger logger;
        public DeviceRepository(ILogger logger)
        {
            this.logger = logger;

            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    DeviceConfig = session.Query<DeviceConfigEntity>().AsEnumerable<DeviceConfigEntity>();
                }
            }
            catch (Exception)
            {
                logger.Error("[Error] DataManager Setting Failed!!!");
                throw;
            }
        }

        public IEnumerable<DeviceConfigEntity> DeviceConfig { get; set; }

        public void SaveChanges()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(DeviceConfig);
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
}
