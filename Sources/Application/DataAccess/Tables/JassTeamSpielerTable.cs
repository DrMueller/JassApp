using JassApp.DataAccess.Tables.Base;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class JassTeamSpielerTable : TableBase
    {
        public bool IstStartSpieler { get; set; }
        public JassTeamTable JassTeam { get; set; } = null!;
        public int JassTeamId { get; set; }
        public SpielerTable Spieler { get; set; } = null!;
        public int SpielerId { get; set; }
    }
}