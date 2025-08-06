using JassApp.Domain.Models;

namespace JassApp.Domain.Services.Servants.Implementation
{
    public class TrumpfRundenFactory : ITrumpfRundenFactory
    {
        private static readonly IDictionary<TrumpfTyp, int> _trumpfDefaultSorting = new Dictionary<TrumpfTyp, int>
        {
            { TrumpfTyp.Herz, 1 },
            { TrumpfTyp.Ecken, 2 },
            { TrumpfTyp.Kreuz, 3 },
            { TrumpfTyp.Schaufeln, 4 },
            { TrumpfTyp.Gschobna, 5 },
            { TrumpfTyp.Differenzler, 6 },
            { TrumpfTyp.Oben, 7 },
            { TrumpfTyp.Unten, 8 },
            { TrumpfTyp.Slalom, 9 },
            { TrumpfTyp.Misere, 10 },
            { TrumpfTyp.Gustav, 11 },
            { TrumpfTyp.DrueliDrue, 12 },
            { TrumpfTyp.Tutti, 13 },
            { TrumpfTyp.Wahl, 14 }
        };

        public IReadOnlyCollection<Trumpfrunde> Create(CoiffeurSpielrundeTyp typ)
        {
            var trumpfs = Trumpf.CreateAll().ToList();

            if (typ == CoiffeurSpielrundeTyp.OhneBeides)
            {
                trumpfs.RemoveAll(f => f.Typ == TrumpfTyp.Gschobna || f.Typ == TrumpfTyp.Differenzler);
            }

            if (typ == CoiffeurSpielrundeTyp.WithDifferenzler)
            {
                trumpfs.RemoveAll(f => f.Typ == TrumpfTyp.Gschobna);
            }

            if (typ == CoiffeurSpielrundeTyp.WithGschobna)
            {
                trumpfs.RemoveAll(f => f.Typ == TrumpfTyp.Differenzler);
            }

            var sortedTruempfe = trumpfs
                .OrderBy(t => _trumpfDefaultSorting[t.Typ])
                .ToList();

            var punkteModifier = 1;

            var runden = sortedTruempfe
                .Select(f => new Trumpfrunde(punkteModifier++, f))
                .OrderBy(f => f.PunkteModifikator)
                .ToList();

            return runden;
        }
    }
}