using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public class TodoItemService : ITodoItemService
    {
        string path = @"C:\01_DATA\Temp\ToDoListe.txt";

        public IEnumerable<TodoItem> ReadTodos()
        {
            if (!File.Exists(path))
                return Enumerable.Empty<TodoItem>();

            var jsonString = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<IEnumerable<TodoItem>>(jsonString);

        }

        public async Task WriteTodos(IEnumerable<TodoItem> todoItems)
        {
            if (!File.Exists(path))
            {
                using (var file = File.Create(path)) { };
            }

            var jsonString = JsonConvert.SerializeObject(todoItems, Formatting.Indented);


            using (var fileStream = File.CreateText(path))
            {
                await fileStream.WriteAsync(jsonString);
            }

            


        }


    }
}
