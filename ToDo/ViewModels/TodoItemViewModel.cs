using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                _todoitemService.WriteTodos(_allTodos);

                mainWindowViewModel.CountTodaysActiveTodos();
                
            }

        }

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

        }
    }
}
