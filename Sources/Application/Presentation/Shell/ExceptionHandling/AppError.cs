namespace JassApp.Presentation.Shell.ExceptionHandling
{
    public record AppError(
        string ErrorType,
        string ErrorMessage,
        string StrackTrace);
}