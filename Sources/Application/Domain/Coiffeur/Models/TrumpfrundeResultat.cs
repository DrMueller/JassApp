namespace JassApp.Domain.Coiffeur.Models
{
    public class TrumpfrundeResultat(JassTeamTyp teamTyp)
    {
        private string _rawInput = string.Empty;

        public bool IstKonterMatch
            =>
                !string.IsNullOrEmpty(RawInput) &&
                RawInput.EndsWith("**");

        public bool IstMatch =>
            !string.IsNullOrEmpty(RawInput) &&
            RawInput.EndsWith("*")
            && !IstKonterMatch;

        public int? Punkte
        {
            get
            {
                var rawPunkte = _rawInput.Replace("*", "");
                return int.TryParse(rawPunkte, out var punkte) ? punkte : null;
            }
        }

        public string RawInput
        {
            get => _rawInput;
            set
            {
                var rawPunkte = value.Replace("*", "");
                var canParse = int.TryParse(rawPunkte, out var punkte);
                if (!canParse)
                {
                    _rawInput = string.Empty;
                    return;
                }

                if (punkte > 16)
                {
                    _rawInput = "16";
                    return;
                }

                _rawInput = value;
            }
        }

        public JassTeamTyp TeamTyp { get; } = teamTyp;
    }
}