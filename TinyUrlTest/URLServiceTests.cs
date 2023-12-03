using NUnit.Framework;
using Moq;
using TinyUrlApp.Services;
using TinyUrlApp.DataAccess;
using TinyUrlApp.Models;
using System.Threading.Tasks;
using System;

namespace URLServiceTests
{
    public class URLServiceTests
    {
        private Mock<IRepository> mockRepository;
        private URLService urlService;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IRepository>();
            urlService = new URLService(mockRepository.Object);

            // Setup mock behavior for repository methods
            mockRepository.Setup(repo => repo.AddUrlAsync(It.IsAny<UrlEntry>()))
                          .ReturnsAsync(true);
            mockRepository.Setup(repo => repo.GetUrlAsync(It.IsAny<string>()))
                          .ReturnsAsync(new UrlEntry { LongUrl = "https://www.example.com" });
            mockRepository.Setup(repo => repo.DeleteUrlAsync(It.IsAny<string>()))
                          .ReturnsAsync(true);
        }

        [Test]
        public async Task CreateShortURLAsync_ValidUrl_ReturnsShortUrl()
        {
            var shortUrl = await urlService.CreateShortURLAsync("https://www.example.com");
            Assert.IsNotNull(shortUrl);
        }


        [Test]
        public void CreateShortURLAsync_WithInvalidUrl_ThrowsArgumentException()
        {
            var longUrl = "invalid-url";
            Assert.ThrowsAsync<ArgumentException>(async () => await urlService.CreateShortURLAsync(longUrl));
        }

        [Test]
        public async Task GetLongURLAsync_ValidShortUrl_ReturnsLongUrl()
        {
            var longUrl = await urlService.GetLongURLAsync("short123");
            Assert.AreEqual("https://www.example.com", longUrl);
        }

        [Test]
        public async Task GetLongURLAsync_WithNonExistingShortUrl_ReturnsNull()
        {
            var shortUrl = "nonExistingShortUrl";
            mockRepository.Setup(repo => repo.GetUrlAsync(shortUrl))
                          .ReturnsAsync((UrlEntry)null);

            var longUrl = await urlService.GetLongURLAsync(shortUrl);
            Assert.IsNull(longUrl);
        }

        [Test]
        public async Task DeleteShortURLAsync_ValidShortUrl_ReturnsTrue()
        {
            var result = await urlService.DeleteShortURLAsync("short123");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteShortURLAsync_WithNonExistingShortUrl_ReturnsFalse()
        {
            var shortUrl = "nonExistingShortUrl";
            mockRepository.Setup(repo => repo.DeleteUrlAsync(shortUrl))
                          .ReturnsAsync(false);

            var result = await urlService.DeleteShortURLAsync(shortUrl);
            Assert.IsFalse(result);
        }
    }
}



