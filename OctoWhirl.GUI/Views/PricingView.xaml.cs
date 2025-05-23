﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OctoWhirl.GUI.ViewModels;

namespace OctoWhirl.GUI.Views
{
    /// <summary>
    /// Logique d'interaction pour PricingView.xaml
    /// </summary>
    public partial class PricingView : UserControl
    {
        public PricingView(PricingViewModel pricingViewModel)
        {
            InitializeComponent();
            DataContext = pricingViewModel;
        }
    }
}
