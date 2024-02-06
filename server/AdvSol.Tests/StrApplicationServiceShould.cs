using AdvSol.Data.Entities;
using AdvSol.Data;
using AdvSol.Data.Repositories;
using AdvSol.Services;
using AdvSol.Services.Dtos.StrApplication;
using AdvSol.Utils;
using AutoFixture.Xunit2;
using Moq;
using Xunit;
using AdvSol.Services.Dtos.CommonCode;

namespace AdvSol.Tests
{
    public class StrApplicationServiceShould
    {
        [Theory]
        [AutoDomainData]
        public async Task GetStrApplicationsSuccessfully(
            StrApplicationDto dto,
            [Frozen] Mock<IStrApplicationRepository> mockStrApplicationRepo,
            StrApplicationService sut)
        {
            // Arrange
            var strApplicationsData = new StrApplicationDto[] { dto };
            mockStrApplicationRepo.Setup(x => x.GetStrApplicationsAsync()).ReturnsAsync(strApplicationsData);

            // Act
            var result = await sut.GetStrApplicationsAsync();

            // Assert
            Assert.Equal(result.Length, strApplicationsData.Length);
        }

        [Theory]
        [AutoDomainData]
        public async Task GetStrApplicationSuccessfully(
            int id,
            StrApplicationDto dto,
            [Frozen] Mock<IStrApplicationRepository> mockStrApplicationRepo,
            StrApplicationService sut)
        {
            // Arrange
            dto.Id = id;
            var strApplicationData = dto;
            mockStrApplicationRepo.Setup(x => x.GetStrApplicationAsync(id)).ReturnsAsync(strApplicationData);

            // Act
            var result = await sut.GetStrApplicationAsync(id);

            // Assert
            Assert.Equal(dto.Id, result.Id);
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateStrApplicationSuccessfully(
            StrApplicationDto dto,
            StrApplication entity,
            CommonCodeDto code,
            SystemUser user,
            [Frozen] Mock<ICommonCodeRepository> mockCommonCodeRepo,
            [Frozen] Mock<ISystemUserRepository> mockSystemUserRepo,
            [Frozen] Mock<INotificationRepository> mockNotificationRepo,
            [Frozen] Mock<IStrApplicationRepository> mockStrApplicationRepo,
            [Frozen] Mock<IUnitOfWork> mockUnitOfWork,
            StrApplicationService sut)
        {
            // Arrange
            code.CodeSet = "COMPLIANCE_STATUS";
            code.CodeValue = code.CodeName = "Pending";             
            var pendingStatus = code;

            mockCommonCodeRepo.Setup(x => x.GetCode(CodeSet.ComplianceStatus, "Pending")).ReturnsAsync(pendingStatus);

            mockUnitOfWork.Setup(x => x.Commit()).Verifiable();

            var adminUsersData = new SystemUser[] { user };
            mockSystemUserRepo.Setup(x => x.GetAdminUsers()).ReturnsAsync(adminUsersData);
            mockNotificationRepo.Setup(x => x.CreateNotificationAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
            mockStrApplicationRepo.Setup(x => x.CreateStrApplicationAsync(dto)).ReturnsAsync(entity);

            mockUnitOfWork.Setup(x => x.Commit()).Verifiable();

            // Act
            var result = await sut.CreateStrApplicationAsync(dto);

            // Assert
            mockUnitOfWork.Verify(x => x.Commit(), Times.Exactly(2)); 
        }

        [Theory]
        [AutoDomainData]
        public async Task GetStrApplicationsByApplicantSuccessfully(
            int applicantId,
            StrApplicationDto dto,
            [Frozen] Mock<IStrApplicationRepository> mockStrApplicationRepo,
            StrApplicationService sut)
        {
            // Arrange
            var strApplicationsData = new StrApplicationDto[] { dto };
            mockStrApplicationRepo.Setup(x => x.GetStrApplicationsByApplicantAsync(applicantId)).ReturnsAsync(strApplicationsData);

            // Act
            var result = await sut.GetStrApplicationsByApplicantAsync(applicantId);

            // Assert
            Assert.Equal(result.Length, strApplicationsData.Length);
        }
    }
}
