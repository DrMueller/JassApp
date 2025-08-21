using JassApp.DataAccess.Configurations.Base;
using JassApp.DataAccess.Tables;
using Microsoft.EntityFrameworkCore;
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

            builder.HasOne(f => f.JassTeamSpieler1)
                .WithMany()
                .HasForeignKey(f => f.JassTeamSpieler1Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.JassTeamSpieler2)
                .WithMany()
                .HasForeignKey(f => f.JassTeamSpieler2Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}