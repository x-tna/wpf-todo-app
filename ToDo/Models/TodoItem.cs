using System;
using System.Collections.Generic;

namespace ToDo
{

    public class TodoItem : IComparable<TodoItem>
    {
        public string Name { get; set; }

        public bool IsDone { get; set; }

        public DateTime Timestamp { get; set; }

        public int CompareTo(TodoItem other)
        {
            if (Name.Length > other.Name.Length)
            {
                return 1;
            }
            else if (Name.Length < other.Name.Length)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }

    }

}
