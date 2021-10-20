using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDo.Commands;
using ToDo.Services;

namespace ToDo.ViewModels
{
    class MainWindowViewModel
    {

        private readonly ITodoItemService _todoItemService;
        private readonly IDateTimeService _dateTimeService;

        private string _newTodoName;
        public string NewTodoName
        {
            get { return _newTodoName; }
            set { _newTodoName = value; AddTodoCommand?.RaisCanExecuteChanged(); }
        }
        public ObservableCollection<TodoItemViewModel> TodoItems { get; set; }

        private TodoItemViewModel _selectedTodoItem;
        public TodoItemViewModel SelectedTodoItem
        {
            get { return _selectedTodoItem; }
            set { _selectedTodoItem = value; DeleteTodoCommand?.RaisCanExecuteChanged(); }
        }


        public ActionCommand AddTodoCommand { get; set; }
        public ActionCommand DeleteTodoCommand { get; set; }

        public MainWindowViewModel(
            ITodoItemService todoItemService,
            IDateTimeService dateTimeService)
        {
            _todoItemService = todoItemService;
            _dateTimeService = dateTimeService;

            TodoItems = new ObservableCollection<TodoItemViewModel>();

            var todoItemModels = _todoItemService.ReadTodos();

            foreach (var item in todoItemModels)
            {
                TodoItems.Add(CreateTodoViewModel(item));
            }

            AddTodoCommand = new ActionCommand(AddNewTodo, CanAddNewTodo);
            DeleteTodoCommand = new ActionCommand(DeleteSelectedTodo, CanDeleteTodo);

        }

        private TodoItemViewModel CreateTodoViewModel (TodoItem todoItem)
        {
            return new TodoItemViewModel(todoItem, _todoItemService, TodoItems);
        }

        private bool CanAddNewTodo()
        {
            return !String.IsNullOrWhiteSpace(NewTodoName);
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

            }
        }

        private bool CanDeleteTodo()
        {
            if (SelectedTodoItem != null)
                return true;
            return false;
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
