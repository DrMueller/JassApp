using JassApp.Domain.Models;

namespace JassApp.Domain.Services.Implementation
{
    public class TrumpfFactory : ITrumpfFactory
    {
        private static readonly Trumpf _drueliDrue = new(TrumpfTyp.DrueliDrue, "Drülidrü");
        private static readonly Trumpf _egge = new(TrumpfTyp.Ecken, "Ecken");
        private static readonly Trumpf _gschobna = new(TrumpfTyp.Gschobna, "Gschobna");
        private static readonly Trumpf _gustav = new(TrumpfTyp.Gustav, "Gustav");
        private static readonly Trumpf _herz = new(TrumpfTyp.Herz, "Herz");
        private static readonly Trumpf _kreuz = new(TrumpfTyp.Kreuz, "Kreuz");
        private static readonly Trumpf _misere = new(TrumpfTyp.Misere, "Misere");
        private static readonly Trumpf _oben = new(TrumpfTyp.Oben, "Oben");
        private static readonly Trumpf _schaufeln = new(TrumpfTyp.Schaufeln, "Schaufeln");
        private static readonly Trumpf _slalom = new(TrumpfTyp.Slalom, "Slalom");
        private static readonly Trumpf _tutti = new(TrumpfTyp.Tutti, "Tutti");
        private static readonly Trumpf _unten = new(TrumpfTyp.Unten, "Unten");
        private static readonly Trumpf _wahl = new(TrumpfTyp.Wahl, "Wahl");

        public IReadOnlyCollection<Trumpf> CreateWithGschobna()
        {
            return
            [
                _herz,
                _egge,
                _kreuz,
                _schaufeln,
                _gschobna,
                _oben,
                _unten,
                _slalom,
                _misere,
                _gustav,
                _drueliDrue,
                _tutti,
                _wahl
            ];
        }
    }
}