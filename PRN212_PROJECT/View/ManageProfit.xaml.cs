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
using System.Windows.Shapes;

namespace PRN212_PROJECT.View
{
    /// <summary>
    /// Interaction logic for ManageProfit.xaml
    /// </summary>
    public partial class ManageProfit : Window
    {
        public ManageProfit()
        {
            InitializeComponent();
            DataContext = new PRN212_PROJECT.View_Model.ManageProfitVM();
        }
    }
}
