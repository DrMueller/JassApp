namespace JassApp.Domain.Spieler.BusinessObjects
{
    public record SpielerHistoryEntryBo(
        string Spieler1Name,
        string Spieler2Name,
        DateTime GespieltAm);
}