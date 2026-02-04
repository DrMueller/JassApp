using JassApp.DataAccess.Tables.Base;
using JassApp.Domain.Coiffeur.Models;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Tables
{
    [PublicAPI("EF Core")]
    public class TrumpfrundeTable : TableBase
    {
        public int CoiffeurSpielrundeId { get; set; }
        public CoiffeurTrumpfTyp CoiffeurTrumpfTyp { get; set; }
        public bool IstKonterMatchTeam1 { get; set; }
        public bool IstKonterMatchTeam2 { get; set; }
        public bool IstMatschTeam1 { get; set; }
        public bool IstMatschTeam2 { get; set; }
        public int PunkteModifikator { get; set; }
        public int? ResultatTeam1 { get; set; }
        public int? ResultatTeam2 { get; set; }
    }
}