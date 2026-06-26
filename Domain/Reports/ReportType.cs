namespace Domain.Reports
{
    /// <summary>Het soort rapport dat gegenereerd moet worden. Bepaalt welke ConcreteBuilder de factory teruggeeft.</summary>
    public enum ReportType
    {
        Deployment,
        Review
    }
}
