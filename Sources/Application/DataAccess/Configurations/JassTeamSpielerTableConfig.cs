using JassApp.DataAccess.Configurations.Base;
using JassApp.DataAccess.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassApp.DataAccess.Configurations
{
    public class JassTeamSpielerTableConfig : TableConfigBase<JassTeamSpielerTable>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<JassTeamSpielerTable> builder)
        {
            builder.Property(f => f.JassTeamId).IsRequired();
            builder.Property(f => f.SpielerId).IsRequired();
            builder.Property(f => f.IstStartSpieler).IsRequired();
            builder.Property(f => f.Position).IsRequired();

            builder.HasOne(f => f.Spieler)
                .WithMany(f => f.JassTeamSpieler)
                .HasForeignKey(f => f.SpielerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}