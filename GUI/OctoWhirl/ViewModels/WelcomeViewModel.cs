using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.GUI.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public string WelcomeMessage => $"Welcome back {Environment.UserName} !";

        public WelcomeViewModel()
        {

        }
    }
}
