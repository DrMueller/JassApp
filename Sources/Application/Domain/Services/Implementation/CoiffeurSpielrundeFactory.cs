using JassApp.Domain.Models;

namespace JassApp.Domain.Services.Implementation
{
    public class CoiffeurSpielrundeFactory(ITrumpfFactory trumpFactory) : ICoiffeurSpielrundeFactory
    {
        private static readonly IDictionary<TrumpfTyp, int> _trumpfDefaultSorting = new Dictionary<TrumpfTyp, int>
        {
            { TrumpfTyp.Herz, 1 },
            { TrumpfTyp.Ecken, 2 },
            { TrumpfTyp.Kreuz, 3 },
            { TrumpfTyp.Schaufeln, 4 },
            { TrumpfTyp.Gschobna, 5 },
            { TrumpfTyp.Oben, 6 },
            { TrumpfTyp.Unten, 7 },
            { TrumpfTyp.Slalom, 8 },
            { TrumpfTyp.Misere, 9 },
            { TrumpfTyp.Gustav, 10 },
            { TrumpfTyp.DrueliDrue, 11 },
            { TrumpfTyp.Tutti, 12 },
            { TrumpfTyp.Wahl, 13 }
        };

        public CoiffeurSpielrunde CreateGschobna(int punkteWert, JassTeam team1, JassTeam team2)
        {
            var trumpfs = trumpFactory.CreateWithGschobna();

            var sortedTruempfe = trumpfs
                .OrderBy(t => _trumpfDefaultSorting[t.Typ])
                .ToList();

            var punkteModifier = 1;

            var runden = sortedTruempfe
                .Select(f => new Trumpfrunde(punkteModifier++, f))
                .OrderBy(f => f.PunkteModifikator)
                .ToList();

            return new CoiffeurSpielrunde(
                punkteWert,
                runden,
                team1,
                team2);
        }
    }
}