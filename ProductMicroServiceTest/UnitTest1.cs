using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductMicroService.Controllers;
using ProductMicroService.Models;
using ProductMicroService.Provider;
using ProductMicroService.Repository;
using System.Collections.Generic;

namespace ProductMicroServiceTest
{
    [TestFixture]
    public class Tests
    {
        private ProductController _controller;
        private Mock<IProvider> _config;
        private ProductProvider provider;
        private Mock<IProductRepository> _repo;
        private List<ProductDto> dto;



        [SetUp]
        public void Setup()
        {
            _config = new Mock<IProvider>();
            _controller = new ProductController(_config.Object);
            _repo = new Mock<IProductRepository>();
            provider = new ProductProvider(_repo.Object);
            dto = new List<ProductDto>()
            {
                new ProductDto(){Id=1,Price=1000,Name="Shoes",Description="Refresh your style with a new pair of men's shoes",Image_name="Shoes.png",Rating=4},
                new ProductDto(){Id=2,Price=5000,Name="Thermals",Description="Discover wide range of thermals for women and men",Image_name="Thermals.png",Rating=4},
                new ProductDto(){Id=3,Price=500,Name="Bats",Description="A cricket bat is simply equipment made from wooden pieces used by a sportsperson in the cricket sport to hit the ball",Image_name="Bats.png",Rating=5},

            };
        }

        [Test]
        public void GetProductDetailsById_Success()
        {
            _config.Setup(p => p.SearchProductById(1)).Returns(new ProductDto
            {
                Id = 1,
                Price = 1000,
                Name = "Shoes",
                Description = "..",
                Image_name = "Shoes.png",
                Rating = 4
            });
            var result = _controller.SearchProductById(1);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void GetProductDetailsByName_Success()
        {
            _config.Setup(p => p.SearchProductByName("Thermals")).Returns(new ProductDto
            {
                Id = 2,
                Price = 5000,
                Name = "Thermals",
                Description = "..",
                Image_name = "Thermals.png",
                Rating = 4
            });
            var result = _controller.SearchProductByName("Thermals");
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void GetProductDetailsById_Fail()
        {

            var result = _controller.SearchProductById(0) as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);
        }
        [Test]
        public void GetProductDetailsByName_Fail()
        {

            var result = _controller.SearchProductByName("Ball") as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);
        }
        [Test]
        public void AddRatingToProduct_Success()
        {
            ProductRating rating = new ProductRating { Id = 1, Rating = 3 };
            _config.Setup(p => p.AddProductRating(rating)).Returns(new RatingStatus { Message = "Rating added Successfully to the Product" });
            var result = _controller.AddProductRating(rating);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void AddRatingToProduct_Fail()
        {
            ProductRating rating = new ProductRating { Id = 0, Rating = 3 };
            var result = _controller.AddProductRating(rating);
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
        [Test]
        public void GetProductByIdRepo_Sucess()
        {
            _repo.Setup(x => x.SearchProductById()).Returns(dto);
            var response = provider.SearchProductById(1);
            Assert.IsNotNull(response);
        }
        [Test]
        public void GetProductByIdRepo_Fail()
        {
            _repo.Setup(x => x.SearchProductById()).Returns(dto);
            var response = provider.SearchProductById(99);
            Assert.IsNull(response);
        }
        [Test]
        public void GetProductByNameRepo_Sucess()
        {
            _repo.Setup(x => x.SearchProductByName()).Returns(dto);
            var response = provider.SearchProductByName("Shoes");
            Assert.IsNotNull(response);
        }
        [Test]
        public void GetProductByNameRepo_Fail()
        {
            _repo.Setup(x => x.SearchProductByName()).Returns(dto);
            var response = provider.SearchProductByName("dinesh");
            Assert.IsNull(response);
        }

    }
}