// using System.Net;

// using DataMedic.Application.Sensors.Models;
// using DataMedic.Infrastructure.Portainer;

// using Moq;
// using Moq.Protected;

// using Newtonsoft.Json;

// using Xunit;

// using Assert = Xunit.Assert;

// namespace DataMedic.Infrastructure.Tests.Portainer
// {
//     public class PortainerServiceTests
//     {
//         private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
//         private readonly PortainerService _portainerService;

//         public PortainerServiceTests()
//         {
//             _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
//             var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
//             _portainerService = new PortainerService();
//         }

//         [Fact]
//         public async Task GetDockerContainerListPagedAsync_WithValidInputs_ReturnsListOfContainerInfoFromPortainer()
//         {
//             // Arrange
//             int hostId = 1;

//             // Act
//             List<ContainerInfoFromPortainer> result =
//                 await _portainerService.GetDockerContainerListPagedAsync(hostId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.IsType<List<ContainerInfoFromPortainer>>(result);
//         }

//         [Fact]
//         public async Task
//             GetDockerContainerListPagedAsync_WithInvalidInputs_ReturnsEmptyListOfContainerInfoFromPortainer()
//         {
//             // Arrange
//             int hostId = -1;

//             // Act & Assert
//             await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
//                 await _portainerService.GetDockerContainerListPagedAsync(hostId)
//             );
//         }

//         [Fact]
//         public async Task GetDockerHostList_WithValidInputs_ReturnsListOfDockerHostFromPortainer()
//         {
//             // Arrange

//             // Act
//             List<DockerHostFromPortainer> result = await _portainerService.GetDockerHostList();

//             // Assert
//             Assert.NotNull(result);
//             Assert.IsType<List<DockerHostFromPortainer>>(result);
//         }

//         [Fact]
//         public async Task GetContainerInfoByContainerId_WithValidInputs_ReturnsDockerContainerDetail()
//         {
//             // Arrange
//             int portainerHostId = 1;
//             string containerId = "dc1aa044739c97ad9c5f77391b87edbeb5e491c1e1e35048e4790192b69ecce2";

//             // Act
//             DockerContainerDetail result =
//                 await _portainerService.GetContainerInfoByContainerId(portainerHostId, containerId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.IsType<DockerContainerDetail>(result);
//         }

//         [Fact]
//         public async Task GetContainerInfoByContainerId_WithInvalidInputs_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             int portainerHostId = -1;
//             string containerId = "";

//             // Act & Assert
//             await Assert.ThrowsAsync<InvalidOperationException>(() =>
//                 _portainerService.GetContainerInfoByContainerId(portainerHostId, containerId));
//         }
//     }
// }