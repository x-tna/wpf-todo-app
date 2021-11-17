using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Commands;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.ViewModels
{
    public class TodoItemViewModel : ViewModelBase
    {
        private readonly ITodoItemService _todoitemService;
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly IEnumerable<TodoItem> _allTodos;

        public string TodaysTodos { get; set; }

        public string Name
        {
            get { return TodoItem.Name; }
            set { TodoItem.Name = value; }
        }
        public string Description
        {
            get { return TodoItem.Description; }
            set { TodoItem.Description = value; }
        }

        public DateTime TimeStamp
        {
            get { return TodoItem.Timestamp; }
            set { TodoItem.Timestamp = value; }
        }

        public bool IsDone
        {
            get { return TodoItem.IsDone; }
            set
            {
                TodoItem.IsDone = value;
                mainWindowViewModel.WriteTodosAsync();

                mainWindowViewModel.CountTodaysActiveTodos();
                mainWindowViewModel.DeleteTodoCommand?.RaisCanExecuteChanged();

            }

        }

        public ObservableCollection<string> Tags { get; set; }

        public string NewTag 
        {
            get { return TodoItem.NewTag; }

            set
            {
                TodoItem.NewTag = value;
                AddNewTag();

            }
        }

        public ParameterCommand<string> AddTagCommand { get; }
        public ParameterCommand<string> DeleteTagCommand { get; }


        public TodoItem TodoItem { get; }

        public TodoItemViewModel(
            TodoItem todoitem,
            ITodoItemService todoitemService,
            IEnumerable<TodoItemViewModel> allTodos,
            MainWindowViewModel mainWindowViewModel)
        {
            TodoItem = todoitem;
            _todoitemService = todoitemService;
            this.mainWindowViewModel = mainWindowViewModel;
            _allTodos = allTodos.Select(vm => vm.TodoItem);

            AddTagCommand = new ParameterCommand<string>(AddTag, CanAddTag);
            DeleteTagCommand = new ParameterCommand<string>(DeleteSelectedTag, CanDeleteTag);

            Tags = new ObservableCollection<string>(todoitem.Tags);

            NewTag = "+";


        }

        private void AddTag()
        {
            TodoItem.Tags.Clear();
            foreach(var tag in Tags)
            {
                TodoItem.Tags.Add(tag);
            }
 
        }

        private bool CanAddTag()
        {
            return true;
        }

        private bool CanDeleteTag(string tag)
        {
            return true; 
        }

        private void DeleteSelectedTag(string tag)
        {
            TodoItem.Tags.Remove(tag);
            Tags.Remove(tag);
            
            RaisePropertyChanged("");
            mainWindowViewModel.RaisePropertyChanged("");

            mainWindowViewModel.WriteTodosAsync();
        }

        private void AddNewTag()
        {
            if (!String.IsNullOrWhiteSpace(NewTag) & !NewTag.Equals("+") & !Tags.Contains(NewTag))
            {
                Tags.Add(NewTag);
                TodoItem.Tags.Add(NewTag);

                mainWindowViewModel.WriteTodosAsync();

                NewTag = "+";
                mainWindowViewModel.RaisePropertyChanged("");
            }

        }

    }
}
