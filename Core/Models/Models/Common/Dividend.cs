namespace OctoWhirl.Core.Models.Common
{
    public class Dividend : CorporateAction
    {
        public override CorporateActionType ActionType { get => CorporateActionType.Dividend; }
        public double DividendAmount { get; set; }
    }
}
