using JassApp.DataAccess.Tables.Base;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class SpielerTable : TableBase
    {
        public ICollection<JassTeamSpielerTable> JassTeamSpieler { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}