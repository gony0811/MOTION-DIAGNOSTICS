using EPLE.Data.Entity;
using EPLE.Interface;
using NHibernate;
using NHibernate.Linq;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace EPLE.Data.Data.Repository
{
    public class MeasureRepository : ISaveChanges
    {
        private readonly ILogger logger;
        public MeasureRepository(ILogger logger)
        {
            this.logger = logger;

            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    MeasureConfig = session.Query<MeasureConfigEntity>().AsEnumerable<MeasureConfigEntity>();
                }
            }
            catch (Exception)
            {
                logger.Error("[Error] DataManager Setting Failed!!!");
                throw;
            }
        }

        public IEnumerable<MeasureConfigEntity> MeasureConfig { get; set; }

        public void SaveChanges()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(MeasureConfig);
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
