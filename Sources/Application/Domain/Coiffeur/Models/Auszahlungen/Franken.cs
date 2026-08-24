namespace JassApp.Domain.Coiffeur.Models.Auszahlungen
{
    public class Franken(int punkte, int additionalFranken, int punkteWertInRappen)
    {
        public string Description
        {
            get
            {
                var rappen = punkte * punkteWertInRappen;
                var franken = rappen / 100m;
                franken += additionalFranken;

                return $"{franken:0.00} Fr.";
            }
        }
    }
}