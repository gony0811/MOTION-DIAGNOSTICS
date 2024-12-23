using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EPLE.Data
{
    internal class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            // SQLite 데이터베이스 제공자 설정
            optionsBuilder.UseJetOleDb(@$"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Data\db\config.accdb;");
            return new DataContext(optionsBuilder.Options);
        }
    }
}
