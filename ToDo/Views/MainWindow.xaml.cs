using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using ToDo.IoC;
using ToDo.Services;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = IocContainer.Container.Resolve<MainWindowViewModel>();

        }


    }
}
