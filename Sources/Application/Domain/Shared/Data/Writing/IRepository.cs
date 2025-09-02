using System.Diagnostics.CodeAnalysis;

namespace JassApp.Domain.Shared.Data.Writing
{
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interface for easier generic handling")]
    public interface IRepository;
}