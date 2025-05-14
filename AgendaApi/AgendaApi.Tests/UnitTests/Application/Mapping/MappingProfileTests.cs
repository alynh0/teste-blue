using AgendaApi.Application.Mapping;
using AutoMapper;

namespace AgendaApi.Tests.UnitTests.Application.Mapping
{
    public class MappingProfileTests
    {
        [Fact]
        public void MappingConfiguration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }

    }
}
