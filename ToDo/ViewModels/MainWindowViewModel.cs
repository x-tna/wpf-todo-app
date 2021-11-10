using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDo.Commands;
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
        private readonly ITagService _tagService;

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

        private string _newTodoTag;

        public string NewTodoTag
        {
            get { return _newTodoTag; }
            set 
            {
                _newTodoTag = value;
                RaisePropertyChanged(nameof(NewTodoDescription));
            }
        }

        private ObservableCollection<string> _todoTags;

        public ObservableCollection<string> TodoTags
        {
            get { return _todoTags; }
            set
            { 
                _todoTags = value;
                RaisePropertyChanged(nameof(TodoTags));
            }
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


        public MainWindowViewModel(
            ITodoItemService todoItemService,
            IDateTimeService dateTimeService,
            ITagService tagService)
        {
            _todoItemService = todoItemService;
            _dateTimeService = dateTimeService;
            _tagService = tagService;
         

            AddTodoCommand = new ActionCommand(AddNewTodo, CanAddNewTodo);
            ShowAllCommand = new ActionCommand(ShowAll, CanShowAll);
            ShowActiveCommand = new ActionCommand(ShowActive, CanShowActive);
            ShowDoneCommand = new ActionCommand(ShowDone, CanShowDone);

            DeleteTodoCommand = new ParameterCommand<TodoItemViewModel>(DeleteSelectedTodo, CanDeleteTodo);

            TodoItems = new ObservableCollection<TodoItemViewModel>();
            var todoItemModels = _todoItemService.ReadTodos();

            foreach (var item in todoItemModels.OrderBy(item => item.IsDone).ThenBy(item => item.Timestamp))
            {
                TodoItems.Add(CreateTodoViewModel(item));
            }

            CountTodaysActiveTodos();

            TodoTags = new ObservableCollection<string>();
            var taglist = _tagService.ReadTags();
            foreach(var tag in taglist)
            {
                TodoTags.Add(tag);
            }

            NewTodoName = Resources.MainWindowVMNewTodo;
            NewTodoDescription = Resources.MainWindowVMNewDescription;
        }


        private void ShowFilteredItems()
        {
            TodoItems = new ObservableCollection<TodoItemViewModel>(_todoItemService
                .ReadTodos()
                .Where(item => item.Tag == TodoTagFilter)
                .Select(CreateTodoViewModel));
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
            return new TodoItemViewModel(todoItem, _todoItemService, TodoItems, this, _tagService);
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
                    Tag = NewTodoTag,
                };

                TodoItems.Add(CreateTodoViewModel(newItem));

                _todoItemService.WriteTodos(TodoItems.Select(vm => vm.TodoItem));

                CountTodaysActiveTodos();

                if(!TodoTags.Contains(NewTodoTag))
                {
                    TodoTags.Add(NewTodoTag);
                    _tagService.WriteTags(TodoTags);
                }
                

                NewTodoName = Resources.MainWindowVMNewTodo;
                NewTodoDescription = Resources.MainWindowVMNewDescription;
                // NewTodoDescription = string.Empty;
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
                _todoItemService.WriteTodos(TodoItems.Select(vm => vm.TodoItem));
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


    }
}
