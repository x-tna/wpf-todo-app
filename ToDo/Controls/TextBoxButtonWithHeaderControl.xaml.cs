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
    public partial class TextBoxButtonWithHeaderControl : UserControl
    {


        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", 
                typeof(string), typeof(TextBoxButtonWithHeaderControl), 
                new PropertyMetadata("Neuen Eintrag hinzufügen"));

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", 
                typeof(string), typeof(TextBoxButtonWithHeaderControl), 
                new PropertyMetadata("Hinzufügen"));

        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", 
                typeof(ICommand), typeof(TextBoxButtonWithHeaderControl), 
                new PropertyMetadata());

        public static readonly DependencyProperty TextBoxTextProperty =
            DependencyProperty.Register("TextBoxText", 
                typeof(string), typeof(TextBoxButtonWithHeaderControl),
                new PropertyMetadata("Neues Todo"));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public string TextBoxText
        {
            get { return (string)GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }



        public TextBoxButtonWithHeaderControl()
        {
            InitializeComponent();
        }
    }
}
