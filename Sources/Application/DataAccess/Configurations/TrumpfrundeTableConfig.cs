using JassApp.DataAccess.Configurations.Base;
using JassApp.DataAccess.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassApp.DataAccess.Configurations
{
    public class TrumpfrundeTableConfig : TableConfigBase<TrumpfrundeTable>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TrumpfrundeTable> builder)
        {
            builder.Property(f => f.CoiffeurTrumpfTyp).IsRequired();
            builder.Property(f => f.PunkteModifikator).IsRequired();
            builder.Property(f => f.CoiffeurSpielrundeId).IsRequired();

            builder.Property(f => f.ResultatTeam1).HasMaxLength(10);
            builder.Property(f => f.ResultatTeam2).HasMaxLength(10);
        }
    }
}