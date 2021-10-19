using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToDo.Services
{
    interface ITodoItemService
    {
        IEnumerable<TodoItem> ReadTodos();

        void WriteTodos(IEnumerable<TodoItem> todoItems);

    }
}
