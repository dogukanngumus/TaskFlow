using Moq;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Queries.GetTasks;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Application.Tests.QueryTests;

[TestClass]
public class GetTasksQueryHandlerTests
{
    private Mock<ITaskItemRepository> _mockRepository = null!;
    private GetTasksQueryHandler _handler = null!;
    private List<TaskItem> _taskList = null!;

    [TestInitialize]
    public void Setup()
    {
        var user = Guid.NewGuid();
        _taskList = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Desc 1", DateTime.Today.AddDays(1),user),
            TaskItem.Create("Task 2", "Desc 2", DateTime.Today.AddDays(1), user),
        };

        _mockRepository = new Mock<ITaskItemRepository>();
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_taskList.ToList());
        _handler = new GetTasksQueryHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_TasksExist_ReturnsTaskDtoList()
    {
        // Arrange
        var query = new GetTasksQuery();

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNotNull(result);
        
        var list = result.ToList();
        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("Task 1", list[0].Title);
        Assert.AreEqual("Task 2", list[1].Title);
    }

    [TestMethod]
    public async Task Handle_NoTasksExist_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<TaskItem>());
        var query = new GetTasksQuery();

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }
}
