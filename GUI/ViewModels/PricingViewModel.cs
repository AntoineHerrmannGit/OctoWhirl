using MvvmHelpers;
using OctoWhirl.GUI.ViewModels.Technical;

namespace OctoWhirl.GUI.ViewModels
{
    public class PricingViewModel : BaseViewModel
    {
        private readonly IStatusService _statusService;

        public PricingViewModel(IStatusService statusService)
        {
            _statusService = statusService;
        }
    }
}
