﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo;
using ToDo.Models;
using ToDo.Services;
using ToDo.ViewModels;

namespace TodoApp.UnitTests.ViewModels
{

    [TestClass]
    public class TodoItemViewModelTests
    {
        [TestMethod]
        public void Name_NameIsTheSameAsInModel()
        {
            // Arrange
            var viewModel = CreateSut();

            // Act
            viewModel.Name = "Staubsaugen";

            // Assert
            viewModel.TodoItem.Name.ShouldBe("Staubsaugen");
        }

        [TestMethod]
        public void Timestamp_TimestampIsTheSameAsInModel()
        {
            // Arrange
            var viewModel = CreateSut();
            var FakeTimestamp = DateTime.Now;

            // Act
            viewModel.TimeStamp = FakeTimestamp;

            // Assert
            viewModel.TodoItem.Timestamp.ShouldBe(FakeTimestamp);
        }

        [TestMethod]
        public void IsDone_IsDoneIsTheSameAsInModel()
        {
            // Arrange
            var viewModel = CreateSut();

            // Act
            viewModel.IsDone = true;

            // Assert
            viewModel.TodoItem.IsDone.ShouldBeTrue();
        }

        [TestMethod]
        public void SetIsDone_IsDoneIsTrue_WriteTodoIsExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            // Act
            viewModel.IsDone = true;
            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void SetIsDone_IsDoneIsFalse_WriteTodoIsExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            // Act
            viewModel.IsDone = false;
            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void Description_DescriptionIsTheSameAsInModel()
        {
            // Arrange
            var viewModel = CreateSut();
            // Act
            viewModel.Description = "Test Description";
            // Assert
            viewModel.TodoItem.Description.ShouldBe("Test Description");

        }


        [TestMethod]
        public void AddNewTag_TagIsNotWhitespace_WriteTodoIsExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            // Act
            viewModel.NewTag = "Neuer Tag";
            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeTrue();
        }

        [TestMethod]
        public void AddNewTag_TagIsWhitespace_WriteTodoIsNotExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            // Act
            viewModel.NewTag = " ";

            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void AddNewTag_TagIsNull_WriteTodoIsNotExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            // Act
            viewModel.NewTag = "";
            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeFalse();
        }

        [TestMethod]
        public void AddNewTag_TagIsPlusSign_WriteTodoIsNotExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            // Act
            viewModel.NewTag = "+";
            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeFalse();

        }

        [TestMethod]
        public void DeleteTag_TagIsNotNull_WriteTodoIsExecuted()
        {
            // Arrange
            var fakeTodoService = new FakeTodoService();
            var viewModel = CreateSut(fakeTodoService);
            var param = "Tag";
            // Act
            viewModel.DeleteTagCommand.Execute(param);
            // Assert
            fakeTodoService.WriteToDosWasCalled.ShouldBeTrue();
        }

        private TodoItemViewModel CreateSut(FakeTodoService fakeTodoService = null)
        {
            if(fakeTodoService == null)
            {
                fakeTodoService = new FakeTodoService();
            }
            var todoItem = new TodoItem();
            todoItem.Tags = new List<string>();

            var allTodos = new ObservableCollection<TodoItemViewModel>();
            var mainWindowViewModel = new MainWindowViewModel(fakeTodoService, null);
            return new TodoItemViewModel(todoItem, fakeTodoService, allTodos, mainWindowViewModel);
        }
    }
}
