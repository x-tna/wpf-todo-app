using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo;
using ToDo.Services;
using ToDo.ViewModels;

namespace TodoApp.UnitTests.ViewModels
{
   class FakeTodoService : ITodoItemService
    {
        public IEnumerable<TodoItem> ReadTodos()
        {
           return new List<TodoItem>();

        }

        public bool WriteToDosWasCalled { get; private set; }

        public void WriteTodos(IEnumerable<TodoItem> todoItems)
        {
            WriteToDosWasCalled = true;
        }
    }

    internal class FakeTimeStampService : IDateTimeService
    {
        public DateTime FakeNow { get; set; }
        public DateTime Now()
        {
            return FakeNow;
        }
    }



    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void AddNewtodo_NewTodoNameIsEmpty_AddTodoButtonCannotBeExecuted()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.NewTodoName = "";
            // Act
            var canExecute = viewModel.AddTodoCommand.CanExecute(null);

            // Assert
            canExecute.ShouldBeFalse();
        }

        [TestMethod]
        public void AddNewtodo_NewTodoNameIsNotEmpty_AddTodoButtonCanBeExecuted()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.NewTodoName = "Some text...";
            // Act
            var canExecute = viewModel.AddTodoCommand.CanExecute(null);

            // Assert
            canExecute.ShouldBeTrue();
        }

        [TestMethod]
        public void AddNewTodo_NewTodoNameIsWhitespace_AddTodoButtonCannotBeExecuted()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.NewTodoName = " ";
            // Act
            var canExecute = viewModel.AddTodoCommand.CanExecute(null);

            // Assert
            canExecute.ShouldBeFalse();
        }

        [TestMethod]
        public void DeleteTodo_ItemIsNotSelected_DeleteButtonCannotBeExecuted()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.SelectedTodoItem = null;
            // Act
            var canExecute = viewModel.DeleteTodoCommand.CanExecute(null);

            // Assert
            canExecute.ShouldBeFalse();

        }

        [TestMethod]
        public void DeleteTodo_ItemIsSelected_DeleteButtonCanBeExecuted()
        {
            // Arrange
            
            var viewModel = CreateSut();
            var todoItem = new TodoItem();
            var allTodos = new ObservableCollection<TodoItemViewModel>();
            var todoItemModel = new TodoItemViewModel(todoItem, null, allTodos);
            viewModel.SelectedTodoItem = todoItemModel;

            // Act
            var canExecute = viewModel.DeleteTodoCommand.CanExecute(null);

            // Assert
            canExecute.ShouldBeTrue();

        }

        [TestMethod]
        public void ExecuteAddNewTodo_TodoNameNotEmpty_TodoItemIsAddedToList()
        {
            // Arrange
            var viewModel = CreateSut();
            viewModel.NewTodoName = "Staubsaugen";

            // Act
            viewModel.AddTodoCommand.Execute(null);

            // Assert
            viewModel.TodoItems.Single().Name.ShouldBe("Staubsaugen");

        }

        [TestMethod]
        public void ExecuteDeleteItem_ItemIsSelected_TodoItemIsDeleted()
        {
            // Arrange
            var viewModel = CreateSut();
            var todoItem = new TodoItem();
            var allTodos = new ObservableCollection<TodoItemViewModel>();
            viewModel.TodoItems.Add(new TodoItemViewModel(todoItem, null, allTodos) { Name = "Staubsaugen" });
            var selectedItem = viewModel.TodoItems[0];
            viewModel.SelectedTodoItem = selectedItem;

            // Act
            viewModel.DeleteTodoCommand.Execute(null);

            // Assert
            viewModel.TodoItems.ShouldBeEmpty();
        }


        [TestMethod]
        public void AddNewTodo_NewTodoHasCurrentTimeAsTimeStamp()
        {
            // Arrange
            var FakeTimestamp = DateTime.Now;
            var viewModel = CreateSut(FakeTimestamp);
            viewModel.NewTodoName = "Test Timestamp";

            // Act
            viewModel.AddTodoCommand.Execute(null);

            // Assert
            viewModel.TodoItems.Single().TimeStamp.Equals(FakeTimestamp);

        }

 

        private MainWindowViewModel CreateSut(DateTime FakeNow = default)
        {
            var dateTimeService = new FakeTimeStampService();
            dateTimeService.FakeNow = FakeNow;
            return new MainWindowViewModel(new FakeTodoService(), dateTimeService);
            
        }
    }
}
