using JassApp.DataAccess.Configurations.Base;
using JassApp.DataAccess.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassApp.DataAccess.Configurations
{
    public class JassTeamTableConfig : TableConfigBase<JassTeamTable>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<JassTeamTable> builder)
        {
            builder.Property(f => f.JassTeamTyp)
                .IsRequired();

            builder.Property(f => f.CoiffeurSpielrundeId)
                .IsRequired();
        }
    }
}