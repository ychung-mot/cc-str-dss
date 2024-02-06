using AdvSol.Data;
using AdvSol.Data.Repositories;
using AdvSol.Services;
using AdvSol.Services.Dtos.Notification;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace AdvSol.Tests
{

    public class NotificationServiceShould
    {
        [Theory]
        [AutoDomainData]
        public async Task GetNotificationsSuccessfully(NotificationDto dto,
            [Frozen] Mock<INotificationRepository> mockNotificationRepo, 
            NotificationService sut)
        {
            // Arrange
            var notificationData = new NotificationDto[] { dto };

            mockNotificationRepo.Setup(x => x.GetNotificationsAsync()).ReturnsAsync(notificationData);

            // Act
            var result = await sut.GetNotificationsAsync();

            // Assert
            Assert.Equal(result.Length, notificationData.Length);
        }

        [Theory]
        [AutoDomainData]
        public async Task UpdateNotificationsCommitSuccessfully(int[] ids,
            [Frozen] Mock<INotificationRepository> mockNotificationRepo,
            [Frozen] Mock<IUnitOfWork> mockUnitOfWork,
            NotificationService sut)
        {
            // Arrange
            mockNotificationRepo.Setup(x => x.UpdateNotificationAsync(ids)).Returns(Task.CompletedTask);

            // Act
            await sut.UpdateNotificationAsync(ids);

            // Assert
            mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }
    }
}

 