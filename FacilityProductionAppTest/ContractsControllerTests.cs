using FacilityProductionManagement.Controllers;
using FacilityProductionManagement.Data;
using FacilityProductionManagement.DTOs;
using FacilityProductionManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FacilityProductionAppTest
{
    public class ContractsControllerTests
    {
        private DbContextOptions<MyDbContext> CreateInMemoryDbOptions(string databaseName) =>
        new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        private async Task<MyDbContext> SeedDataAsync(DbContextOptions<MyDbContext> options, Action<MyDbContext> seedAction)
        {
            var context = new MyDbContext(options);
            seedAction?.Invoke(context);
            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task CreateContract_ReturnsBadRequest_WhenAreaNotEnough()
        {
            // Arrange
            var options = CreateInMemoryDbOptions("TestDatabase_AreaNotEnough");

            await using var context = await SeedDataAsync(options, ctx =>
            {
                ctx.Facilities.Add(new Facility { Code = "FA01", Name = "Facility A", StandardArea = 500 });
                ctx.EquipmentTypes.Add(new EquipmentType { Code = "EQ01", Name = "Equipment A", Area = 50 });
                ctx.EquipmentContracts.Add(new EquipmentContract { FacilityCode = "FA01", EquipmentTypeCode = "EQ01", Quantity = 5 });
            });

            var controller = new ContractController(context);

            var contractDto = new ContractDTO
            {
                FacilityCode = "FA01",
                EquipmentTypeCode = "EQ01",
                Quantity = 6 
            };

            // Act
            var result = await controller.CreateContract(contractDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Insufficient area in the facility. Available area: 250, required: 300.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateContract_ReturnsBadRequest_WhenContractDtoIsNull()
        {
            // Arrange
            var options = CreateInMemoryDbOptions("TestDatabase_NullInput");

            await using var context = new MyDbContext(options);
            var controller = new ContractController(context);

            // Act
            var result = await controller.CreateContract(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid data. Ensure all fields are filled with valid values.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateContract_ReturnsNotFound_WhenFacilityDoesNotExist()
        {
            // Arrange
            var options = CreateInMemoryDbOptions("TestDatabase_MissingFacility");

            await using var context = await SeedDataAsync(options, ctx =>
            {
                ctx.EquipmentTypes.Add(new EquipmentType { Code = "EQ01", Name = "Equipment A", Area = 50 });
            });

            var controller = new ContractController(context);

            var contractDto = new ContractDTO
            {
                FacilityCode = "FA99",
                EquipmentTypeCode = "EQ01",
                Quantity = 5
            };

            // Act
            var result = await controller.CreateContract(contractDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Facility with code 'FA99' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateContract_ReturnsNotFound_WhenEquipmentTypeDoesNotExist()
        {
            // Arrange
            var options = CreateInMemoryDbOptions("TestDatabase_MissingEquipment");

            await using var context = await SeedDataAsync(options, ctx =>
            {
                ctx.Facilities.Add(new Facility { Code = "FA01", Name = "Facility A", StandardArea = 500 });
            });

            var controller = new ContractController(context);

            var contractDto = new ContractDTO
            {
                FacilityCode = "FA01",
                EquipmentTypeCode = "EQ99",
                Quantity = 5
            };

            // Act
            var result = await controller.CreateContract(contractDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Equipment type with code 'EQ99' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateContract_ReturnsBadRequest_WhenAddingContractExceedsRemainingArea()
        {
            // Arrange
            var options = CreateInMemoryDbOptions("TestDatabase_ExceedsArea");

            await using var context = await SeedDataAsync(options, ctx =>
            {
                ctx.Facilities.Add(new Facility { Code = "FA01", Name = "Facility A", StandardArea = 500 });
                ctx.EquipmentTypes.Add(new EquipmentType { Code = "EQ01", Name = "Equipment A", Area = 50 });
                ctx.EquipmentContracts.Add(new EquipmentContract { FacilityCode = "FA01", EquipmentTypeCode = "EQ01", Quantity = 8 });
            });

            var controller = new ContractController(context);

            var contractDto = new ContractDTO
            {
                FacilityCode = "FA01",
                EquipmentTypeCode = "EQ01",
                Quantity = 3 
            };

            // Act
            var result = await controller.CreateContract(contractDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Insufficient area in the facility. Available area: 100, required: 150.", badRequestResult.Value);
        }
    }
}
