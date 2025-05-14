using AgendaApi.Application.Mapping;
using AutoMapper;

namespace AgendaApi.Tests.TestUtils
{
    public static class AutoMapperHelper
    {
        private static IMapper _mapper;

        public static IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                _mapper = config.CreateMapper();
            }
            return _mapper;
        }
    }
}
