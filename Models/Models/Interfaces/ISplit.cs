namespace Models.Models.Interfaces
{
    public interface ISplit : ICorporateAction
    {
        double? SplitRatio { get; set; }
    }
}
