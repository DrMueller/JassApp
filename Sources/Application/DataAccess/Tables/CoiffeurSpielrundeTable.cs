using JassApp.DataAccess.Tables.Base;
using JassApp.Domain.Coiffeur.Models;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class CoiffeurSpielrundeTable : TableBase
    {
        public CoiffeurSpielrundeTyp CoiffeurSpielrundeTyp { get; set; }
        public DateTime GestartetAm { get; set; }
        public ICollection<JassTeamTable> JassTeams { get; set; } = null!;
        public int Punktewert { get; set; }
        public ICollection<TrumpfrundeTable> Trumpfrunden { get; set; } = null!;
    }
}