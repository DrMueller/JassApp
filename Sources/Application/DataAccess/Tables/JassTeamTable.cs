using JassApp.DataAccess.Tables.Base;
using JassApp.Domain.Coiffeur.Models;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class JassTeamTable : TableBase
    {
        public int CoiffeurSpielrundeId { get; set; }
        public JassTeamSpielerTable JassTeamSpieler1 { get; set; } = new();
        public int JassTeamSpieler1Id { get; set; }
        public JassTeamSpielerTable JassTeamSpieler2 { get; set; } = new();
        public int JassTeamSpieler2Id { get; set; }
        public JassTeamTyp JassTeamTyp { get; set; }
    }
}