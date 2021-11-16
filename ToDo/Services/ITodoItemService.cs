using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public interface ITodoItemService
    {
        IEnumerable<TodoItem> ReadTodos();

        void WriteTodos(IEnumerable<TodoItem> todoItems);

    }
}
