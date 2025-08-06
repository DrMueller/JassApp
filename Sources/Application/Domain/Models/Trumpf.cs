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
        Differenzler = 5,
        Oben = 6,
        Unten = 7,
        Slalom = 8,
        Misere = 9,
        Gustav = 10,
        DrueliDrue = 11,
        Tutti = 12,
        Wahl = 13
    }

    [PublicAPI]
    public class Trumpf
    {
        public static readonly Trumpf Differenzler = new(TrumpfTyp.Differenzler, "Differenzler");

        public static readonly Trumpf DrueliDrue = new(TrumpfTyp.DrueliDrue, "Drülidrü");
        public static readonly Trumpf Egge = new(TrumpfTyp.Ecken, "Ecken");
        public static readonly Trumpf Gschobna = new(TrumpfTyp.Gschobna, "Gschobna");
        public static readonly Trumpf Gustav = new(TrumpfTyp.Gustav, "Gustav");
        public static readonly Trumpf Herz = new(TrumpfTyp.Herz, "Herz");
        public static readonly Trumpf Kreuz = new(TrumpfTyp.Kreuz, "Kreuz");
        public static readonly Trumpf Misere = new(TrumpfTyp.Misere, "Misere");
        public static readonly Trumpf Oben = new(TrumpfTyp.Oben, "Oben");
        public static readonly Trumpf Schaufeln = new(TrumpfTyp.Schaufeln, "Schaufeln");
        public static readonly Trumpf Slalom = new(TrumpfTyp.Slalom, "Slalom");
        public static readonly Trumpf Tutti = new(TrumpfTyp.Tutti, "Tutti");
        public static readonly Trumpf Unten = new(TrumpfTyp.Unten, "Unten");
        public static readonly Trumpf Wahl = new(TrumpfTyp.Wahl, "Wahl");

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

        public static IReadOnlyCollection<Trumpf> CreateAll()
        {
            return
            [
                Herz,
                Egge,
                Kreuz,
                Schaufeln,
                Gschobna,
                Differenzler,
                Oben,
                Unten,
                Slalom,
                Misere,
                Gustav,
                DrueliDrue,
                Tutti,
                Wahl
            ];
        }
    }
}