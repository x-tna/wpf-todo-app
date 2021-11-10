using System;
using System.Collections.Generic;

namespace ToDo
{

    public class TodoItem : IComparable<TodoItem>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime Timestamp { get; set; }

        public string Tag { get; set; }

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
