using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Properties;

namespace ToDo.Controls
{

    public class SampleCustomControl : Control
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header",
        typeof(string), typeof(SampleCustomControl),
        new PropertyMetadata("Neues Todo hinzufügen"));

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

        public static readonly DependencyProperty TextBoxDescriptionProperty =
            DependencyProperty.Register("TextBoxDescription", 
                typeof(string), typeof(SampleCustomControl), 
                new PropertyMetadata(""));

        public static readonly DependencyProperty TextBoxTagProperty =
            DependencyProperty.Register("TextBoxTag", 
                typeof(string), typeof(SampleCustomControl), 
                new PropertyMetadata("Tag"));

        public static readonly DependencyProperty ListNewTodoTagsProperty =
            DependencyProperty.Register("ListNewTodoTags",
                typeof(ObservableCollection<string>),
                typeof(SampleCustomControl), new PropertyMetadata());


        public static readonly DependencyProperty TaglistProperty =
            DependencyProperty.Register("Taglist", 
                typeof(IEnumerable<string>), 
                typeof(SampleCustomControl), new PropertyMetadata());

        public static readonly DependencyProperty ButtonDeleteTagCommandProperty =
    DependencyProperty.Register("ButtonDeleteTagCommand",
        typeof(ICommand), typeof(SampleCustomControl),
        new PropertyMetadata());



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
        public string TextBoxDescription
        {
            get { return (string)GetValue(TextBoxDescriptionProperty); }
            set { SetValue(TextBoxDescriptionProperty, value); }
        }

        public string TextBoxTag
        {
            get { return (string)GetValue(TextBoxTagProperty); }
            set { SetValue(TextBoxTagProperty, value); }

        }

        public ObservableCollection<string> ListNewTodoTags
        {
            get { return (ObservableCollection<string>)GetValue(ListNewTodoTagsProperty); }
            set { SetValue(ListNewTodoTagsProperty, value); }
        }

        public IEnumerable<string> Taglist
        {
            get { return (IEnumerable<string>)GetValue(TaglistProperty); }
            set { SetValue(TaglistProperty, value); }
        }

        public ICommand ButtonDeleteTagCommand
        {
            get { return (ICommand)GetValue(ButtonDeleteTagCommandProperty); }
            set { SetValue(ButtonDeleteTagCommandProperty, value); }
        }

        static SampleCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SampleCustomControl), new FrameworkPropertyMetadata(typeof(SampleCustomControl)));
        }





    }
}
