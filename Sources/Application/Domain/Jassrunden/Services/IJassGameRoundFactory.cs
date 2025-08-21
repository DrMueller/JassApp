using JassApp.Domain.Jassrunden.Models.Jass;

namespace Mmu.Jsc.Domain.Areas.Services
{
    public interface IJassGameRoundFactory
    {
        JassGameRound CreateOneRound();
    }
}