//using AutoFixture;
//using Castle.Core.Logging;
//using GameStore.DAL.Interfaces;
//using GameStore.Domain.Models;
//using GameStore.Service.Services;
//using Microsoft.Extensions.Logging;
//using MockQueryable.Moq;
//using Moq;

//namespace GameStore.Service.UnitTests.TestGameService
//{
//    public class TestGenreService
//    {
//        private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
//        private readonly Mock<ILogger<GenreService>> _loggerMock;
//        private readonly Fixture _fixture;
//        public TestGenreService()
//        {
//            _genreRepositoryMock = new Mock<IRepository<Genre>>();
//            _loggerMock = new Mock<ILogger<GenreService>>();
//            _fixture = new Fixture();
//        }
        
//        [Fact]
//        public async Task GetGenres_ReturnListOfGenre()
//        {
//            //Arrange
//            var fixtureGenres = _fixture
//                .Build<Genre>()
//                .Without(g => g.GameGenres)
//                .CreateMany(3)
//                .AsQueryable()
//                .BuildMock();
            
//            _genreRepositoryMock.Setup(rep => rep.GetAll()).Returns(fixtureGenres);
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.GetGenresAsync();

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//            Assert.NotEmpty(result.Data);
//        }

//        [Fact]
//        public async Task GetGenres_ThrowException_ReturnEmptyListOfGenre()
//        {
//            //Arrange
//            _genreRepositoryMock.Setup(rep => rep.GetAll()).Throws(new Exception());
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.GetGenresAsync();

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.GetAll(), Times.Once);
//            Assert.Empty(result.Data);
//        }
        
//        [Fact]
//        public async Task Create_OnSuccess_ReturnTrue()
//        {
//            //Arrange
//            var genre = _fixture.Create<Genre>();
//            _genreRepositoryMock.Setup(rep => rep.CreateAsync(genre));
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.CreateGenreAsync(genre);

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.CreateAsync(genre), Times.Once);
//            Assert.True(result.Data);
//        }
        
//        [Fact]
//        public async Task Create_ThrowException_ReturnFalse()
//        {
//            //Arrange
//            var genre = _fixture.Create<Genre>();
//            _genreRepositoryMock.Setup(rep => rep.CreateAsync(genre)).Throws(new Exception());
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.CreateGenreAsync(genre);

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.CreateAsync(genre), Times.Once);
//            Assert.False(result.Data);
//        }
        
//        [Fact]
//        public async Task Update_ThrowException_ReturnFalse()
//        {
//            //Arrange
//            var genre = _fixture.Create<Genre>();
//            _genreRepositoryMock.Setup(rep => rep.UpdateAsync(genre)).Throws(new Exception());
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.UpdateGenreAsync(genre);

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.UpdateAsync(genre), Times.Once);
//            Assert.False(result.Data);
//        }
        
//        [Fact]
//        public async Task Update_OnSuccess_ReturnTure()
//        {
//            //Arrange
//            var genre = _fixture.Create<Genre>();
//            _genreRepositoryMock.Setup(rep => rep.UpdateAsync(genre));
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.UpdateGenreAsync(genre);

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.UpdateAsync(genre), Times.Once);
//            Assert.True(result.Data);
//        }
        
//        [Fact]
//        public async Task Delete_OnSuccess_ReturnTure()
//        {
//            //Arrange
//            var genre = _fixture.Create<Genre>();
//            _genreRepositoryMock.Setup(rep => rep.DeleteAsync(genre));
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.DeleteGenreAsync(It.IsAny<int>());

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.UpdateAsync(genre), Times.Once);
//            Assert.True(result.Data);
//        }
        
//        [Fact]
//        public async Task Delete_ThrowException_ReturnFalse()
//        {
//            //Arrange
//            var genre = _fixture.Create<Genre>();
//            _genreRepositoryMock.Setup(rep => rep.DeleteAsync(genre)).Throws(new Exception());
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.DeleteGenreAsync(It.IsAny<int>());

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.DeleteAsync(genre), Times.Once);
//            Assert.False(result.Data);
//        }
        
//        [Fact]
//        public async Task GetById_OnSuccess_ReturnGenre()
//        {
//            //Arrange
//            const int id = 1;
//            var genre = _fixture.Build<Genre>().Do(g => g.Id = id).Create();
            
//            _genreRepositoryMock.Setup(rep => rep.GetByIdAsync(id)).ReturnsAsync(genre);
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.GetGenreByIdAsync(id);

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.GetByIdAsync(id), Times.Once);
//            Assert.Equal(result.Data, genre);
//        }
        
//        [Fact]
//        public async Task GetById_ThrowException_ReturnNull()
//        {
//            //Arrange
//            const int id = 1;
//            var genre = _fixture.Build<Genre>().Do(g => g.Id = id).Create();
            
//            _genreRepositoryMock.Setup(rep => rep.GetByIdAsync(id)).Throws(new Exception());
//            var genreService = new GenreService(_loggerMock.Object, _genreRepositoryMock.Object);

//            //Act
//            var result = await genreService.GetGenreByIdAsync(id);

//            //Assert
//            _genreRepositoryMock.Verify(rep => rep.GetByIdAsync(id), Times.Once);
//            Assert.Equal(result.Data, genre);
//        }
//    }
//}
