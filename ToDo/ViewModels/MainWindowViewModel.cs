using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Commands;
using ToDo.Models;
using ToDo.Properties;
using ToDo.Services;

namespace ToDo.ViewModels
{
    public static class ExtensionTest
    {
        public static IEnumerable<TodoItem> Where2<TodoItem>(this IEnumerable<TodoItem> todoitems, Func<TodoItem,bool> FunctionTrue)
        {

            var result = new List<TodoItem>();

            foreach (var item in todoitems)
            {
                if(FunctionTrue(item))
                {
                    result.Add(item);
                }
            }

            return result;

        }

        public static IEnumerable<TodoItemViewModel> Select2<TodoItem, TodoItemViewModel>(this IEnumerable<TodoItem> todoitems, Func<TodoItem, TodoItemViewModel> selector)
        {
            var result = new List<TodoItemViewModel>();

            foreach (var item in todoitems)
            {
                result.Add(selector(item));
            }

            return result;
        }

    }

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IDateTimeService _dateTimeService;

        private string _newTodoName;
        public string NewTodoName
        {
            get { return _newTodoName; }
            set
            {
                _newTodoName = value;
                AddTodoCommand?.RaisCanExecuteChanged();
                RaisePropertyChanged(nameof(NewTodoName));

            }
        }

        private string _newTodoDescription;
        public string NewTodoDescription
        { 
            get { return _newTodoDescription; }
            set
            {
                _newTodoDescription = value;
                RaisePropertyChanged(nameof(NewTodoDescription));
            }
        }

        private string _newTag;
        public string NewTag
        {
            get { return _newTag; }
            set 
            {
                _newTag = value;
                AddNewTag();
                RaisePropertyChanged(nameof(NewTag));
               

            }
        }

        private ObservableCollection<string> _newTodoTags;
        public ObservableCollection<string> NewTodoTags
        {
            get { return _newTodoTags; }
            set
            {
                _newTodoTags = value;
                RaisePropertyChanged(nameof(NewTodoTags));
            }
        }

        public IEnumerable<string> TodoTags
        {
            get { return TodoItems.SelectMany(item => item.Tags).Distinct();  }
           
        }

        private string _todoTagFilter;
        public string TodoTagFilter
        {
            get { return _todoTagFilter; }
            set 
            { 
                _todoTagFilter = value;
                RaisePropertyChanged(nameof(TodoTagFilter));
                ShowFilteredItems();
            }
        }


        private ObservableCollection<TodoItemViewModel> _todoItems;
        public ObservableCollection<TodoItemViewModel> TodoItems
        {
            get { return _todoItems; }
            set
            {
                _todoItems = value;
                RaisePropertyChanged(nameof(TodoItems));

            }
        }

        private int _numberOfTodaysActiveTodos;
        public int NumberOfTodaysActiveTodos
        {
            get { return _numberOfTodaysActiveTodos; }
            set
            {
                _numberOfTodaysActiveTodos = value;
                RaisePropertyChanged(nameof(NumberOfTodaysActiveTodos));

            }
        }


        public ActionCommand AddTodoCommand { get; }
        public ActionCommand ShowAllCommand { get; }
        public ActionCommand ShowActiveCommand { get; }
        public ActionCommand ShowDoneCommand { get; }
        public ParameterCommand<TodoItemViewModel> DeleteTodoCommand { get; }
        public ParameterCommand<string> DeleteNewTagCommand { get; }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set 
            { 
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }


        public MainWindowViewModel(
            ITodoItemService todoItemService,
            IDateTimeService dateTimeService)
        {
            _todoItemService = todoItemService;
            _dateTimeService = dateTimeService;

            IsBusy = false;

            AddTodoCommand = new ActionCommand(AddNewTodo, CanAddNewTodo);
            ShowAllCommand = new ActionCommand(ShowAll, CanShowAll);
            ShowActiveCommand = new ActionCommand(ShowActive, CanShowActive);
            ShowDoneCommand = new ActionCommand(ShowDone, CanShowDone);

            DeleteTodoCommand = new ParameterCommand<TodoItemViewModel>(DeleteSelectedTodo, CanDeleteTodo);
            DeleteNewTagCommand = new ParameterCommand<string>(DeleteSelectedTag, CanDeleteTag);


            TodoItems = new ObservableCollection<TodoItemViewModel>();
            var todoItemModels = _todoItemService.ReadTodos();

            foreach (var item in todoItemModels.OrderBy(item => item.IsDone).ThenBy(item => item.Timestamp))
            {
                TodoItems.Add(CreateTodoViewModel(item));
            }

            CountTodaysActiveTodos();

            NewTodoTags = new ObservableCollection<string>();


            NewTodoName = Resources.MainWindowVMNewTodo;
            NewTodoDescription = Resources.MainWindowVMNewDescription;
            NewTag = "+";
        }

        private bool CanDeleteTag(string tag)
        {
            return true;
        }

        private void DeleteSelectedTag(string tag)
        {
            NewTodoTags.Remove(tag);
        }


        private void ShowFilteredItems()
        {
            if(!String.IsNullOrWhiteSpace(TodoTagFilter))
            {
                TodoItems = new ObservableCollection<TodoItemViewModel>(_todoItemService
                                .ReadTodos()
                                .Where(item => item.Tags.Contains(TodoTagFilter))
                                .Select(CreateTodoViewModel));
            }


        }

        private bool CanShowDone()
        {
            return true;
        }
        private void ShowDone()
        {

            TodoItems = new ObservableCollection<TodoItemViewModel>(_todoItemService
                .ReadTodos()
                .Where(item => item.IsDone)
                .Select(CreateTodoViewModel));

        }

        private bool CanShowActive()
        {
            return true;
        }
        private void ShowActive()
        {
            TodoItems = new ObservableCollection<TodoItemViewModel>(_todoItemService
                .ReadTodos()
                .Where(item => !item.IsDone)
                .Select(CreateTodoViewModel));
        }

        private bool CanShowAll()
        {
            return true;
        }
        private void ShowAll()
        {
            TodoItems = new ObservableCollection<TodoItemViewModel>(_todoItemService
                .ReadTodos()
                .Select(CreateTodoViewModel));


        }

        private TodoItemViewModel CreateTodoViewModel(TodoItem todoItem)
        {
            return new TodoItemViewModel(todoItem, _todoItemService, TodoItems, this);
        }

        private bool CanAddNewTodo()
        {
            return (!String.IsNullOrWhiteSpace(NewTodoName)) & !String.Equals(NewTodoName, Resources.MainWindowVMNewTodo);
        }
        private void AddNewTodo()
        {
            if (!String.IsNullOrWhiteSpace(NewTodoName))
            {
                if (NewTodoDescription == Resources.MainWindowVMNewDescription)
                {
                    NewTodoDescription = string.Empty;
                }


                var newItem = new TodoItem()
                {
                    Name = NewTodoName,
                    Description = NewTodoDescription,
                    IsDone = false,
                    Timestamp = _dateTimeService.Now(),
                    Tags = NewTodoTags.ToList()
                };

                TodoItems.Add(CreateTodoViewModel(newItem));

                WriteTodosAsync();

                CountTodaysActiveTodos();

                RaisePropertyChanged(nameof(TodoTags));

                

                NewTodoName = Resources.MainWindowVMNewTodo;
                NewTodoDescription = Resources.MainWindowVMNewDescription;
                NewTodoTags.Clear();

            }
        }

        private bool CanDeleteTodo(TodoItemViewModel todoItem)
        {
            return todoItem.IsDone;
        }
        private void DeleteSelectedTodo(TodoItemViewModel todoItem)
        {
            if(todoItem.IsDone)
            {
                TodoItems.Remove(todoItem);
                WriteTodosAsync();
                CountTodaysActiveTodos();
            }
       

        }

        public void CountTodaysActiveTodos()
        {
            NumberOfTodaysActiveTodos = TodoItems
                .Where(TodoItemIsActive)
                .Where(TodoItemIsCreatedToday)
                .Count();
        }

        private bool TodoItemIsActive(TodoItemViewModel todoitem)
        {
            return !todoitem.IsDone;
        }

        private bool TodoItemIsCreatedToday(TodoItemViewModel todoitem)
        {
            return todoitem.TimeStamp.Date == DateTime.Now.Date;
        }


        private void AddNewTag()
        {
            if (!String.IsNullOrWhiteSpace(NewTag) & !NewTag.Equals("+") & !NewTodoTags.Contains(NewTag))
            {
                NewTodoTags.Add(NewTag);
                NewTag = "+";
            }

        }

        public async void WriteTodosAsync()
        {
            try
            {
                IsBusy = true;                
                await _todoItemService.WriteTodos(TodoItems.Select(vm => vm.TodoItem));
                await Waiting();
                IsBusy = false;
            }catch(Exception ex)
            {

            }

        }

        public async Task Waiting()
        {
            await Task.Delay(2000);
        }


    }
}
