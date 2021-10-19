using System;
using System.Windows;
using ToDo.Commands;
using ToDo.Services;
using ToDo.ViewModels;
using ToDo.Views;

namespace ToDo
{

    class TodoItem 
    {
        public string Name { get; set; }

         public bool IsDone { get; set; }

        public DateTime Datum { get; set; }
        
    }
}
