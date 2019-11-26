using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using MOD.TechnologyService.Repositories;
using MOD.TechnologyService.Models;
using MOD.TechnologyService.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MOD.Test
{
    public class TechnologyControllerTest
    {
        private readonly Mock<ITechRepo> _repo;
        private readonly TechnologyController _controller;

        public TechnologyControllerTest()
        {
            _repo = new Mock<ITechRepo>();
            _controller = new TechnologyController(_repo.Object);
        }

        [Fact]
        public void Get()
        {
            _repo.Setup(m => m.GetAllTechnologies()).Returns(GetSkills());
            var result = _controller.Get() as List<Technology>;
            Assert.Equal(3, result.Count);

        }
        [Fact]
        public void GetById()
        {
            _repo.Setup(m => m.GetTechnologyById(1)).Returns(GetSkills()[0]);
            var result = _controller.Get(1) as Technology;
            Assert.NotNull(result);
            Assert.Equal(1, result.TechID);

        }

        [Fact]
        public void Post()
        {
            var item = GetSkills()[0];
            var result = _controller.Post(item) as OkResult;
            Assert.NotNull(result);

        }

        [Fact]

        public void Put()
        {
            var item = GetSkills()[0];
            var result = _controller.Put(item) as OkResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete()
        {
            _repo.Setup(m => m.deleteTechnology(It.IsAny<int>()));
            var result = _controller.Delete(5) as OkResult;
            //var result = _controller.Delete(0) as BadRequestResult;
            Assert.NotNull(result);
        }

        private List<Technology> GetSkills()
        {
            return new List<Technology>()
            {
                new Technology(){TechID=1,TechnologyName="DotnetCore"},
                new Technology(){TechID=2,TechnologyName="Java"},
                new Technology(){TechID=3,TechnologyName="Mean Stack"}
            };
        }

    }
}
