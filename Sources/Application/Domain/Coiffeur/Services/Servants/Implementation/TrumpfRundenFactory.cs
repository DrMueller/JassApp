using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Domain.Coiffeur.Services.Servants.Implementation
{
    public class TrumpfRundenFactory : ITrumpfRundenFactory
    {
        private static readonly IDictionary<CoiffeurTrumpfTyp, int> _trumpfDefaultSorting = new Dictionary<CoiffeurTrumpfTyp, int>
        {
            { CoiffeurTrumpfTyp.Herz, 1 },
            { CoiffeurTrumpfTyp.Ecken, 2 },
            { CoiffeurTrumpfTyp.Kreuz, 3 },
            { CoiffeurTrumpfTyp.Schaufeln, 4 },
            { CoiffeurTrumpfTyp.Gschobna, 5 },
            { CoiffeurTrumpfTyp.Differenzler, 6 },
            { CoiffeurTrumpfTyp.Oben, 7 },
            { CoiffeurTrumpfTyp.Unten, 8 },
            { CoiffeurTrumpfTyp.Slalom, 9 },
            { CoiffeurTrumpfTyp.Misere, 10 },
            { CoiffeurTrumpfTyp.Gustav, 11 },
            { CoiffeurTrumpfTyp.DrueliDrue, 12 },
            { CoiffeurTrumpfTyp.Tutti, 13 },
            { CoiffeurTrumpfTyp.Wahl, 14 }
        };

        public IReadOnlyCollection<CoiffeurTrumpfrunde> Create(CoiffeurSpielrundeTyp typ)
        {
            var trumpfs = CoiffeurTrumpf.CreateAll().ToList();

            if (typ == CoiffeurSpielrundeTyp.OhneBeides)
            {
                trumpfs.RemoveAll(f => f.Typ is CoiffeurTrumpfTyp.Gschobna or CoiffeurTrumpfTyp.Differenzler);
            }

            if (typ == CoiffeurSpielrundeTyp.WithDifferenzler)
            {
                trumpfs.RemoveAll(f => f.Typ == CoiffeurTrumpfTyp.Gschobna);
            }

            if (typ == CoiffeurSpielrundeTyp.WithGschobna)
            {
                trumpfs.RemoveAll(f => f.Typ == CoiffeurTrumpfTyp.Differenzler);
            }

            var sortedTruempfe = trumpfs
                .OrderBy(t => _trumpfDefaultSorting[t.Typ])
                .ToList();

            var punkteModifier = 1;

            var runden = sortedTruempfe
                .Select(f => new CoiffeurTrumpfrunde(new TrumpfrundeId(0), punkteModifier++, f))
                .OrderBy(f => f.PunkteModifikator)
                .ToList();

            return runden;
        }
    }
}