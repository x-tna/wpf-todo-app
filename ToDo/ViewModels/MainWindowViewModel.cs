using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ToDo.Commands;
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
        private const string NEW_TODO = "Neues Todo";

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

        private TodoItemViewModel _selectedTodoItem;
        public TodoItemViewModel SelectedTodoItem
        {
            get { return _selectedTodoItem; }
            set
            {
                _selectedTodoItem = value;
                DeleteTodoCommand?.RaisCanExecuteChanged();

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
        public ActionCommand DeleteTodoCommand { get; }
        public ActionCommand ShowAllCommand { get; }
        public ActionCommand ShowActiveCommand { get; }
        public ActionCommand ShowDoneCommand { get; }


        public MainWindowViewModel(
            ITodoItemService todoItemService,
            IDateTimeService dateTimeService)
        {
            _todoItemService = todoItemService;
            _dateTimeService = dateTimeService;

            AddTodoCommand = new ActionCommand(AddNewTodo, CanAddNewTodo);
            DeleteTodoCommand = new ActionCommand(DeleteSelectedTodo, CanDeleteTodo);
            ShowAllCommand = new ActionCommand(ShowAll, CanShowAll);
            ShowActiveCommand = new ActionCommand(ShowActive, CanShowActive);
            ShowDoneCommand = new ActionCommand(ShowDone, CanShowDone);

            TodoItems = new ObservableCollection<TodoItemViewModel>();
            var todoItemModels = _todoItemService.ReadTodos();

            foreach (var item in todoItemModels.OrderBy(item => item.IsDone).ThenBy(item => item.Timestamp))
            {
                TodoItems.Add(CreateTodoViewModel(item));
            }

            CountTodaysActiveTodos();

            NewTodoName = NEW_TODO;
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
            return (!String.IsNullOrWhiteSpace(NewTodoName)) & !String.Equals(NewTodoName, NEW_TODO);
        }
        private void AddNewTodo()
        {
            if (!String.IsNullOrWhiteSpace(NewTodoName))
            {

                var newItem = new TodoItem()
                {
                    Name = NewTodoName,
                    IsDone = false,
                    Timestamp = _dateTimeService.Now(),
                };

                TodoItems.Add(CreateTodoViewModel(newItem));

                _todoItemService.WriteTodos(TodoItems.Select(vm => vm.TodoItem));

                CountTodaysActiveTodos();

                NewTodoName = NEW_TODO;
            }
        }

        private bool CanDeleteTodo()
        {
            return SelectedTodoItem != null;
        }
        private void DeleteSelectedTodo()
        {
            if (SelectedTodoItem != null)
            {
                TodoItems.Remove(SelectedTodoItem);

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

        private bool TodoItemIsActive (TodoItemViewModel todoitem)
        {
            return !todoitem.IsDone;
        }

        private bool TodoItemIsCreatedToday (TodoItemViewModel todoitem)
        {
            return todoitem.TimeStamp.Date == DateTime.Now.Date;
        }


    }
}
