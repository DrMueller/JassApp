using JassApp.DataAccess.Tables.Base;

namespace JassApp.DataAccess.Tables
{
    public class SpielerTable : TableBase
    {
        public ICollection<JassTeamSpielerTable> JassTeamSpieler { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}