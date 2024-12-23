using EPLE.Data.Entity;
using EPLE.Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data
{
    public class DataRepository : ISaveChanges
    {
        private readonly ILogger<DataRepository> logger;
        private readonly DataContext dataContext;

        public DataRepository(ILogger<DataRepository> logger, DataContext dataContext)
        {
            this.logger = logger;
            this.dataContext = dataContext;

            try
            {
                dataContext.DeviceConfig.Load();
                logger.LogInformation("DeviceInfo Init: {@Init}", dataContext.DeviceConfig);

                dataContext.DataConfig.Load();
                logger.LogInformation("DataConfig Init: {@Init}", dataContext.DataConfig);
            }
            catch (Exception)
            {
                logger.LogInformation("[Error] DataManager Setting Failed!!!");
                throw;
            }
        }

        public IEnumerable<DataConfigEntity> DataConfig { get { return dataContext.DataConfig.Local; } }

        public IEnumerable<DeviceConfigEntity> DeviceConfig { get { return dataContext.DeviceConfig.Local; } }

        public void SaveChanges() => dataContext.SaveChanges();
    }
}
