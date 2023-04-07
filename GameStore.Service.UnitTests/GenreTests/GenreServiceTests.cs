// using AutoFixture;
// using Castle.Core.Logging;
// using GameStore.DAL.Interfaces;
// using GameStore.Domain.Enums;
// using GameStore.Domain.Models;
// using GameStore.Domain.Response;
// using GameStore.Domain.ViewModels.Genre;
// using GameStore.Service.Interfaces;
// using GameStore.Service.Services;
// using Microsoft.Extensions.Logging;
// using MockQueryable.Moq;
// using Moq;
//
// namespace GameStore.Service.UnitTests.GenreTests
// {
//     public class GenreServiceTests
//     {
//         private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
//         private readonly Mock<ILogger<GenreService>> _loggerMock;
//         private readonly Fixture _fixture;
//         public GenreServiceTests()
//         {
//             _genreRepositoryMock = new Mock<IRepository<Genre>>();
//             _loggerMock = new Mock<ILogger<GenreService>>();
//             _fixture = new Fixture();
//         }
//
//         [Fact]
//         public async Task GetGenresAsync_ReturnsGenreList()
//         {
//             //Arrange
//             var fixtureGenres = _fixture
//                 .Build<Genre>()
//                 .Without(g => g.GameGenres)
//                 .CreateMany(3)
//                 .AsQueryable()
//                 .BuildMock();
//
//             _genreRepositoryMock.Setup(rep => rep.GetAll()).Returns(fixtureGenres);
//             var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//             //Act
//             var result = await genreService.GetGenresAsync();
//
//             //Assert
//             _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//             Assert.NotEmpty(result.Data);
//         }
//
//         [Fact]
//         public async Task GetGenresAsync_ThrowsException_ReturnsResponseWithStatusServerError()
//         {
//             //Arrange
//             _genreRepositoryMock.Setup(x => x.GetAll()).Throws<Exception>();
//             var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//             //Act
//             var result = await genreService.GetGenresAsync();
//
//             //Assert
//             _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//             Assert.Equal((int)HttpStatusCode.ServerError, (int)result.Status);
//         }
//
//         [Fact]
//         public async Task CreateAsync_ReturnsResponseWithStatusCreated()
//         {
//             //Arrange
//             var response = new Response<bool>() { };
//             var genreView = _fixture.Create<GenreViewModel>();
//             var genre = new Genre() { Name = genreView.Name };
//
//             _genreRepositoryMock.Setup(rep => rep.CreateAsync(genre));
//
//             //var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//             var genreServiceMock = new Mock<GenreService>();
//             
//             genreServiceMock.Setup(x => x.CheckExistAsync(genreView, null)).ReturnsAsync(response);
//
//             //Act
//             var result = await genreServiceMock.Object.CreateGenreAsync(genreView);
//
//             //Assert
//             //_genreRepositoryMock.Verify(rep => rep.CreateAsync(genre), Times.Once);
//             Assert.Equal((int)HttpStatusCode.Created, (int)result.Status);
//         }
//
//         [Fact]
//         public async Task CreateAsync_TrowsException_ReturnsResponseWithStatusServerErro()
//         {
//             //Arrange
//             var genreView = _fixture.Create<GenreViewModel>();
//             var genre = new Genre() { Name = genreView.Name };
//             
//             _genreRepositoryMock.Setup(rep => rep.CreateAsync(genre)).Throws<Exception>();
//             var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//             //Act
//             var result = await genreService.CreateGenreAsync(genreView);
//
//             //Assert
//             _genreRepositoryMock.Verify(rep => rep.CreateAsync(genre), Times.Once);
//             Assert.Equal((int)HttpStatusCode.ServerError, (int)result.Status);
//         }
//         //[Fact]
//         //public async Task Update_ThrowException_ReturnFalse()
//         //{
//         //    //Arrange
//         //    var genre = _fixture.Create<Genre>();
//         //    _genreRepositoryMock.Setup(rep => rep.UpdateAsync(genre)).Throws(new Exception());
//         //    var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//         //    //Act
//         //    var result = await genreService.UpdateGenreAsync(genre);
//
//         //    //Assert
//         //    _genreRepositoryMock.Verify(rep => rep.UpdateAsync(genre), Times.Once);
//         //    Assert.False(result.Data);
//         //}
//
//         //[Fact]
//         //public async Task Update_OnSuccess_ReturnTure()
//         //{
//         //    //Arrange
//         //    var genre = _fixture.Create<Genre>();
//         //    _genreRepositoryMock.Setup(rep => rep.UpdateAsync(genre));
//         //    var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//         //    //Act
//         //    var result = await genreService.UpdateGenreAsync(genre);
//
//         //    //Assert
//         //    _genreRepositoryMock.Verify(rep => rep.UpdateAsync(genre), Times.Once);
//         //    Assert.True(result.Data);
//         //}
//
//         //[Fact]
//         //public async Task Delete_OnSuccess_ReturnTure()
//         //{
//         //    //Arrange
//         //    var genre = _fixture.Create<Genre>();
//         //    _genreRepositoryMock.Setup(rep => rep.DeleteAsync(genre));
//         //    var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//         //    //Act
//         //    var result = await genreService.DeleteGenreAsync(It.IsAny<int>());
//
//         //    //Assert
//         //    _genreRepositoryMock.Verify(rep => rep.UpdateAsync(genre), Times.Once);
//         //    Assert.True(result.Data);
//         //}
//
//         //[Fact]
//         //public async Task Delete_ThrowException_ReturnFalse()
//         //{
//         //    //Arrange
//         //    var genre = _fixture.Create<Genre>();
//         //    _genreRepositoryMock.Setup(rep => rep.DeleteAsync(genre)).Throws(new Exception());
//         //    var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//         //    //Act
//         //    var result = await genreService.DeleteGenreAsync(It.IsAny<int>());
//
//         //    //Assert
//         //    _genreRepositoryMock.Verify(rep => rep.DeleteAsync(genre), Times.Once);
//         //    Assert.False(result.Data);
//         //}
//
//         //[Fact]
//         //public async Task GetById_OnSuccess_ReturnsResnposeWithGenre()
//         //{
//         //    //Arrange
//         //    const int id = 1;
//         //    var genre = _fixture.Build<Genre>()
//         //                        .Do(g => g.Id = id)
//         //                        .Create();
//
//         //    var genres = _fixture.Create<List<Genre>>();
//         //    genres.Add(genre);
//
//         //    _genreRepositoryMock.Setup(rep => rep.GetAll()).Returns(genres.AsQueryable);
//         //    var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//         //    //Act
//         //    var result = await genreService.GetGenreByIdAsync(id);
//
//         //    //Assert
//         //    _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//         //    Assert.NotNull(result.Data);
//         //}
//
//         [Fact]
//         public async Task GetGenreByIdAsync_ThrowException_ReturnsResponseWithServerError()
//         {
//             //Arrange
//             const int id = 1;
//
//             _genreRepositoryMock.Setup(rep => rep.GetAll()).Throws<Exception>();
//             var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//             //Act
//             var result = await genreService.GetGenreByIdAsync(id);
//
//             //Assert
//             _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//             Assert.Null(result.Data);
//             Assert.Equal((int)HttpStatusCode.ServerError, (int)result.Status);
//         }
//
//         [Fact]
//         public async Task GetById_GenreIsNull_ReturnsResponseWithNotFound()
//         {
//             //Arrange
//             const int id = -1;
//
//             var genres = _fixture
//                 .Build<Genre>()
//                 .Without(g => g.GameGenres)
//                 .CreateMany(3)
//                 .ToList()
//                 .BuildMock();// buildmock чтобы работал asQuaeryable иначе ошибка вылетает
//
//             _genreRepositoryMock.Setup(rep => rep.GetAll()).Returns(genres.AsQueryable);
//             var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);
//
//             //Act
//             var result = await genreService.GetGenreByIdAsync(id);
//
//             //Assert
//             _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//             Assert.Null(result.Data);
//             Assert.Equal((int)HttpStatusCode.NotFound, (int)result.Status);
//         }
//     }
// }
