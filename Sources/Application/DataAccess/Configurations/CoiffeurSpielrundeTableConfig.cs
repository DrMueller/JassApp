using JassApp.DataAccess.Configurations.Base;
using JassApp.DataAccess.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassApp.DataAccess.Configurations
{
    public class CoiffeurSpielrundeTableConfig : TableConfigBase<CoiffeurSpielrundeTable>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CoiffeurSpielrundeTable> builder)
        {
            builder.Property(f => f.GestartetAm)
                .IsRequired();

            builder.Property(f => f.Punktewert)
                .IsRequired();

            builder.Property(f => f.CoiffeurSpielrundeTyp)
                .IsRequired();

            builder
                .HasMany(f => f.Trumpfrunden)
                .WithOne()
                .HasForeignKey(f => f.CoiffeurSpielrundeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(f => f.JassTeams)
                .WithOne()
                .HasForeignKey(f => f.CoiffeurSpielrundeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}