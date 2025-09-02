using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Coiffeur.Models
{
    public enum CoiffeurTrumpfTyp
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
    public class CoiffeurTrumpf
    {
        public static readonly CoiffeurTrumpf Differenzler = new(CoiffeurTrumpfTyp.Differenzler, "Differenzler");

        public static readonly CoiffeurTrumpf DrueliDrue = new(CoiffeurTrumpfTyp.DrueliDrue, "Drülidrü");
        public static readonly CoiffeurTrumpf Egge = new(CoiffeurTrumpfTyp.Ecken, "Ecken");
        public static readonly CoiffeurTrumpf Gschobna = new(CoiffeurTrumpfTyp.Gschobna, "Gschobna");
        public static readonly CoiffeurTrumpf Gustav = new(CoiffeurTrumpfTyp.Gustav, "Gustav");
        public static readonly CoiffeurTrumpf Herz = new(CoiffeurTrumpfTyp.Herz, "Herz");
        public static readonly CoiffeurTrumpf Kreuz = new(CoiffeurTrumpfTyp.Kreuz, "Kreuz");
        public static readonly CoiffeurTrumpf Misere = new(CoiffeurTrumpfTyp.Misere, "Misere");
        public static readonly CoiffeurTrumpf Oben = new(CoiffeurTrumpfTyp.Oben, "Oben");
        public static readonly CoiffeurTrumpf Schaufeln = new(CoiffeurTrumpfTyp.Schaufeln, "Schaufeln");
        public static readonly CoiffeurTrumpf Slalom = new(CoiffeurTrumpfTyp.Slalom, "Slalom");
        public static readonly CoiffeurTrumpf Tutti = new(CoiffeurTrumpfTyp.Tutti, "Tutti");
        public static readonly CoiffeurTrumpf Unten = new(CoiffeurTrumpfTyp.Unten, "Unten");
        public static readonly CoiffeurTrumpf Wahl = new(CoiffeurTrumpfTyp.Wahl, "Wahl");

        public CoiffeurTrumpf(
            CoiffeurTrumpfTyp typ,
            string name)
        {
            Guard.StringNotNullOrEmpty(() => name);

            Typ = typ;
            Name = name;
        }

        public string Name { get; }
        public CoiffeurTrumpfTyp Typ { get; }

        public static IReadOnlyCollection<CoiffeurTrumpf> CreateAll()
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

        public static CoiffeurTrumpf CreateFromTyp(CoiffeurTrumpfTyp typ)
        {
            return typ switch
            {
                CoiffeurTrumpfTyp.Differenzler => Differenzler,
                CoiffeurTrumpfTyp.DrueliDrue => DrueliDrue,
                CoiffeurTrumpfTyp.Ecken => Egge,
                CoiffeurTrumpfTyp.Gschobna => Gschobna,
                CoiffeurTrumpfTyp.Gustav => Gustav,
                CoiffeurTrumpfTyp.Herz => Herz,
                CoiffeurTrumpfTyp.Kreuz => Kreuz,
                CoiffeurTrumpfTyp.Misere => Misere,
                CoiffeurTrumpfTyp.Oben => Oben,
                CoiffeurTrumpfTyp.Schaufeln => Schaufeln,
                CoiffeurTrumpfTyp.Slalom => Slalom,
                CoiffeurTrumpfTyp.Tutti => Tutti,
                CoiffeurTrumpfTyp.Unten => Unten,
                CoiffeurTrumpfTyp.Wahl => Wahl,
                _ => throw new ArgumentOutOfRangeException(nameof(typ), typ, null)
            };
        }
    }
}