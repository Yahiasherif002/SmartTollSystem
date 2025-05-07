using Moq;
using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Application.Services;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Interfaces;

public class InvoiceServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly InvoiceService _invoiceService;

    public InvoiceServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _invoiceService = new InvoiceService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateInvoiceAsync_ValidDto_ReturnsInvoiceDto()
    {
        // Arrange
        var inputDto = new InvoiceDto
        {
            VehicleId = Guid.NewGuid(),
            PlateNumber = "ABC123",
            Amount = 100,
            Location = "Mansoura",
            CreatedAt = DateTime.UtcNow,
            IsPaid = false
        };

        var invoice = new Invoice
        {
            InvoiceId = Guid.NewGuid(),
            VehicleId = inputDto.VehicleId,
            PlateNumber = inputDto.PlateNumber,
            Amount = inputDto.Amount,
            Location = inputDto.Location,
            CreatedAt = inputDto.CreatedAt,
            IsPaid = inputDto.IsPaid,
            TollHistoryId = null
        };

       
        _mockUnitOfWork.Setup(u => u.InvoiceRepository.AddAsync(It.IsAny<Invoice>()))
               .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveAsync())
                      .ReturnsAsync(1); 

        // Act
        var result = await _invoiceService.CreateInvoiceAsync(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inputDto.VehicleId, result.VehicleId);
        Assert.Equal(inputDto.PlateNumber, result.PlateNumber);
        Assert.Equal(inputDto.Amount, result.Amount);
        Assert.Equal(inputDto.Location, result.Location);
        Assert.Equal(inputDto.IsPaid, result.IsPaid);
    }
}
