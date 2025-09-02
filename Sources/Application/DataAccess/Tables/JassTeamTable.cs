using JassApp.DataAccess.Tables.Base;
using JassApp.Domain.Coiffeur.Models;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class JassTeamTable : TableBase
    {
        public int CoiffeurSpielrundeId { get; set; }

        public ICollection<JassTeamSpielerTable> JassTeamSpieler { get; set; } = new List<JassTeamSpielerTable>();
        public JassTeamTyp JassTeamTyp { get; set; }
    }
}