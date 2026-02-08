namespace JassApp.Presentation.Shell.Errors.Exceptions
{
    public record AppError(
        string ErrorType,
        string ErrorMessage,
        string StrackTrace);
}