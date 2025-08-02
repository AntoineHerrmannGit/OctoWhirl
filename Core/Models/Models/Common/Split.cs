namespace OctoWhirl.Core.Models.Common
{
    public class Split : CorporateAction
    {
        public override CorporateActionType ActionType { get => CorporateActionType.Split; }
        public double SplitRatio { get; set; }
    }
}
