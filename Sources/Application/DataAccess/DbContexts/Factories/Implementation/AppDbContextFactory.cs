using JassApp.Common.Settings.Provisioning.Services;
using JassApp.DataAccess.DbContexts.Contexts;
using JassApp.DataAccess.DbContexts.Contexts.Implementation;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.DbContexts.Factories.Implementation
{
    public class AppDbContextFactory(
        IDbContextOptionsFactory optionsFactory,
        ISettingsProvider appSettingsProvider)
        : IAppDbContextFactory
    {
        private readonly Lazy<DbContextOptions> _lazyOptions = new(() => optionsFactory
            .CreateForSqlServer(appSettingsProvider.AppSettings.ConnectionString)
        );

        public IAppDbContext Create()
        {
            return new AppDbContext(_lazyOptions.Value);
        }
    }
}