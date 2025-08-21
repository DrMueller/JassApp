using JassApp.DataAccess.Tables.Base;
using JassApp.Domain.Coiffeur.Models;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class TrumpfrundeTable : TableBase
    {
        public int CoiffeurSpielrundeId { get; set; }
        public int PunkteModifikator { get; set; }
        public string? ResultatTeam1 { get; set; }
        public string? ResultatTeam2 { get; set; }
        public CoiffeurTrumpfTyp CoiffeurTrumpfTyp { get; set; }
    }
}