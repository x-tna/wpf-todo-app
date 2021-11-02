using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDo.Controls
{


    public class SampleCustomControl : Control
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header",
        typeof(string), typeof(SampleCustomControl),
        new PropertyMetadata("Neuen Eintrag hinzufügen"));

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent",
                typeof(string), typeof(SampleCustomControl),
                new PropertyMetadata("Hinzufügen"));

        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand",
                typeof(ICommand), typeof(SampleCustomControl),
                new PropertyMetadata());

        public static readonly DependencyProperty TextBoxTextProperty =
            DependencyProperty.Register("TextBoxText",
                typeof(string), typeof(SampleCustomControl),
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

        static SampleCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SampleCustomControl), new FrameworkPropertyMetadata(typeof(SampleCustomControl)));
        }
    }
}
