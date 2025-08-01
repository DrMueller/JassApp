using JassApp.DataAccess.Configurations.Base;
using JassApp.DataAccess.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassApp.DataAccess.Configurations
{
    public class SpielerTableConfig : TableConfigBase<SpielerTable>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SpielerTable> builder)
        {
            builder.Property(f => f.Name).IsRequired();
        }
    }
}
