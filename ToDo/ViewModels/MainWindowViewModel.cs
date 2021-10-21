using System;
using System.Collections.ObjectModel;
using System.Linq;
using ToDo.Commands;
using ToDo.Services;

namespace ToDo.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {

        private readonly ITodoItemService _todoItemService;
        private readonly IDateTimeService _dateTimeService;
        private const string NEW_TODO = "Neues Todo";

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

        public ObservableCollection<TodoItemViewModel> TodoItems { get; set; }
        public ActionCommand AddTodoCommand { get; set; }
        public ActionCommand DeleteTodoCommand { get; set; }

        public MainWindowViewModel(
            ITodoItemService todoItemService,
            IDateTimeService dateTimeService)
        {
            _todoItemService = todoItemService;
            _dateTimeService = dateTimeService;
            AddTodoCommand = new ActionCommand(AddNewTodo, CanAddNewTodo);
            DeleteTodoCommand = new ActionCommand(DeleteSelectedTodo, CanDeleteTodo);
            TodoItems = new ObservableCollection<TodoItemViewModel>();
            var todoItemModels = _todoItemService.ReadTodos();
            NewTodoName = NEW_TODO;

            foreach (var item in todoItemModels)
            {
                TodoItems.Add(CreateTodoViewModel(item));
            }

        }

        private TodoItemViewModel CreateTodoViewModel(TodoItem todoItem)
        {
            return new TodoItemViewModel(todoItem, _todoItemService, TodoItems);
        }

        private bool CanAddNewTodo()
        {
            return (!String.IsNullOrWhiteSpace(NewTodoName)) &! String.Equals(NewTodoName, NEW_TODO);
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

            }
        }

    }
}
