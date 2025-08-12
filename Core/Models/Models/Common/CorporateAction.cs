namespace OctoWhirl.Core.Models.Common
{
    public abstract class CorporateAction
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
// Reference and Currency might be null before assignment, e.g compute a correlation between 2 references that result in a new reference.
        public abstract CorporateActionType ActionType { get; }
        public DateTime TimeStamp { get; set; }
        public string Reference { get; set; }
    }
}
