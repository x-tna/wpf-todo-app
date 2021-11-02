using System;
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

namespace ToDo.Controls
{
    public partial class SampleUserControl : UserControl
    {

        public static readonly DependencyProperty HeaderUCProperty =
            DependencyProperty.Register("HeaderUC", 
                typeof(string), typeof(SampleUserControl), 
                new PropertyMetadata("Neuen Eintrag hinzufügen"));

        public static readonly DependencyProperty ButtonContentUCProperty =
            DependencyProperty.Register("ButtonContentUC", 
                typeof(string), typeof(SampleUserControl), 
                new PropertyMetadata("Hinzufügen"));

        public static readonly DependencyProperty ButtonCommandUCProperty =
            DependencyProperty.Register("ButtonCommandUC", 
                typeof(ICommand), typeof(SampleUserControl), 
                new PropertyMetadata());

        public static readonly DependencyProperty TextBoxTextUCProperty =
            DependencyProperty.Register("TextBoxTextUC", 
                typeof(string), typeof(SampleUserControl),
                new PropertyMetadata("Neues Todo"));


        public string HeaderUC
        {
            get { return (string)GetValue(HeaderUCProperty); }
            set { SetValue(HeaderUCProperty, value); }
        }

        public string ButtonContentUC
        {
            get { return (string)GetValue(ButtonContentUCProperty); }
            set { SetValue(ButtonContentUCProperty, value); }
        }

        public ICommand ButtonCommandUC
        {
            get { return (ICommand)GetValue(ButtonCommandUCProperty); }
            set { SetValue(ButtonCommandUCProperty, value); }
        }

        public string TextBoxTextUC
        {
            get { return (string)GetValue(TextBoxTextUCProperty); }
            set { SetValue(TextBoxTextUCProperty, value); }
        }



        public SampleUserControl()
        {
            InitializeComponent();
        }
    }
}
