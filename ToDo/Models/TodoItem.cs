using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ToDo.Models
{

    public class TodoItem : IComparable<TodoItem>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime Timestamp { get; set; }

        public List<string> Tags { get; set; }
        public string NewTag { get; set; }

        public int CompareTo(TodoItem other)
        {
            if (Timestamp < other.Timestamp)
            {
                return 1;
            }
            else if (Timestamp > other.Timestamp)
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
