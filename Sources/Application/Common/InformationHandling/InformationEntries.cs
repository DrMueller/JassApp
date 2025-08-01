namespace JassApp.Common.InformationHandling
{
    public class InformationEntries
    {
        private readonly IReadOnlyCollection<InformationEntry> _values;

        private InformationEntries(IReadOnlyCollection<InformationEntry> values)
        {
            _values = values;
        }

        public static InformationEntries Empty { get; } = new(new List<InformationEntry>());

        public IReadOnlyCollection<string> ErrorMessages => Select(InformationType.Error);
        public bool HasErrors => _values.Any(f => f.Type == InformationType.Error);

        public bool HasErrorsOrWarnings => _values.Any(f => f.Type == InformationType.Error
                                                            || f.Type == InformationType.Warning);

        public IReadOnlyCollection<string> InfoMessages => Select(InformationType.Information);

        public bool IsEmpty => !_values.Any();
        public IReadOnlyCollection<string> WarningMessages => Select(InformationType.Warning);

        public static InformationEntries CreateFromError(string errorMessage)
        {
            return new InformationEntries(
                new List<InformationEntry>
                {
                    new(InformationType.Error, errorMessage)
                });
        }

        public static InformationEntries CreateFromWarning(string warningMessage)
        {
            return new InformationEntries(
                new List<InformationEntry>
                {
                    new(InformationType.Warning, warningMessage)
                });
        }

        public static InformationEntries CreateFromInfo(string infoMessage)
        {
            return new InformationEntries(
                new List<InformationEntry>
                {
                    new(InformationType.Information, infoMessage)
                });
        }

        public static InformationEntries CreateNew()
        {
            return new InformationEntries(new List<InformationEntry>());
        }

        public InformationEntries AddError(string errorMessage)
        {
            return new InformationEntries(CombineWithCurrent(InformationType.Error, errorMessage));
        }

        public InformationEntries AddInformation(string infoMessage)
        {
            return new InformationEntries(CombineWithCurrent(InformationType.Information, infoMessage));
        }

        public InformationEntries AddWarning(string warningMessage)
        {
            return new InformationEntries(CombineWithCurrent(InformationType.Warning, warningMessage));
        }

        public InformationEntries MergeWith(InformationEntries? other)
        {
            if (other is null)
            {
                return this;
            }

            return new InformationEntries(_values.Concat(other._values).ToList());
        }

        public async Task<InformationEntries> MergeWithAsync(Func<Task<InformationEntries?>> func)
        {
            var other = await func();
            if (other is null)
            {
                return this;
            }

            return new InformationEntries(_values.Concat(other._values).ToList());
        }

        private IReadOnlyCollection<InformationEntry> CombineWithCurrent(InformationType type, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return _values;
            }

            return _values.Concat(new List<InformationEntry> { new(type, message) }).ToList();
        }

        private IReadOnlyCollection<string> Select(InformationType type)
        {
            return _values
                .Where(f => f.Type == type)
                .Select(f => f.Message)
                .OrderBy(f => f)
                .ToList();
        }
    }
}