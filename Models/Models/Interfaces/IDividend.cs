namespace Models.Models.Interfaces
{
    public interface IDividend : ICorporateAction
    {
        DateTime? ExecutionDate { get; set; }
        double? Value { get; set; }
    }
}
