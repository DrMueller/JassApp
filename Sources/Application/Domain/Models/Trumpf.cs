using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Models
{
    public enum TrumpfTyp
    {
        Herz = 0,
        Ecken = 1,
        Kreuz = 2,
        Schaufeln = 3,
        Gschobna = 4,
        Oben = 5,
        Unten = 6,
        Slalom = 7,
        Misere = 8,
        Gustav = 9,
        DrueliDrue = 10,
        Tutti = 11,
        Wahl = 12
    }

    [PublicAPI]
    public class Trumpf
    {
        public Trumpf(
            TrumpfTyp typ,
            string name)
        {
            Guard.StringNotNullOrEmpty(() => name);

            Typ = typ;
            Name = name;
        }

        public string Name { get; }
        public TrumpfTyp Typ { get; }
    }
}