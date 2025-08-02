namespace OctoWhirl.Core.Models.Common
{
    public abstract class CorporateAction
    {
        public abstract CorporateActionType ActionType { get; }
        public DateTime TimeStamp { get; set; }
        public string Reference { get; set; }
    }
}
