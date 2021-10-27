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
            var todoItemViewModel = CreateSut2();
            viewModel.SelectedTodoItem = todoItemViewModel;

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
            var todoItemViewModel = CreateSut2();
            todoItemViewModel.Name = "Neues Todo";

            viewModel.TodoItems.Add(todoItemViewModel);
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


        [TestMethod]
        public void CountTodaysActiveTodos_TodaysTodoIsActive_NumberOfTodaysActiveTodosIsOne()
        {
            // Arrange
            var viewModel = CreateSut();
            var todoItemViewModel = CreateSut2();
            todoItemViewModel.Name = "Neues Todo isActive";
            todoItemViewModel.TimeStamp = DateTime.Now;
            todoItemViewModel.IsDone = false;

            viewModel.TodoItems.Add(todoItemViewModel);
            // Act
            viewModel.CountTodaysActiveTodos();
            // Assert
            viewModel.NumberOfTodaysActiveTodos.ShouldBe(1);
        }

        [TestMethod]
        public void CountTodaysActiveTodos_TodaysTodoIsDone_NumberOfTodaysActiveTodosIsNull()
        {
            // Arrange
            var viewModel = CreateSut();
            var todoItemViewModel = CreateSut2();
            todoItemViewModel.Name = "Neues Todo isDone";
            todoItemViewModel.TimeStamp = DateTime.Now;
            todoItemViewModel.IsDone = true;

            viewModel.TodoItems.Add(todoItemViewModel);
            // Act
            viewModel.CountTodaysActiveTodos();
            // Assert
            viewModel.NumberOfTodaysActiveTodos.ShouldBe(0);
        }

        [TestMethod]
        public void CountTodaysActiveTodos_NoTodaysTodo_NumberOfTodaysActiveTodosIsNull()
        {
            // Arrange
            var viewModel = CreateSut();
            var todoItemViewModel = CreateSut2();
            todoItemViewModel.Name = "Todo von Gestern";
            todoItemViewModel.TimeStamp = DateTime.Today.AddDays(-1);
            todoItemViewModel.IsDone = false;

            viewModel.TodoItems.Add(todoItemViewModel);
            // Act
            viewModel.CountTodaysActiveTodos();
            // Assert
            viewModel.NumberOfTodaysActiveTodos.ShouldBe(0);
        }


        private MainWindowViewModel CreateSut(DateTime FakeNow = default)
        {
            var dateTimeService = new FakeTimeStampService();
            dateTimeService.FakeNow = FakeNow;

            return new MainWindowViewModel(new FakeTodoService(), dateTimeService);
        }


        private TodoItemViewModel CreateSut2(FakeTodoService fakeTodoService = null)
        {
            if (fakeTodoService == null)
            {
                fakeTodoService = new FakeTodoService();
            }
            var todoItem = new TodoItem();
            var allTodos = new ObservableCollection<TodoItemViewModel>();
            var mainWindowViewModel = new MainWindowViewModel(fakeTodoService, null);
            return new TodoItemViewModel(todoItem, fakeTodoService, allTodos, mainWindowViewModel);
        }
    }
}
