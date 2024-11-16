using AutoMapper;
using System; 

namespace MachineVision.Shared.Services
{
    public interface IAppMapper
    {
        IMapper Current { get; }

        T Map<T>(object source);
    }

    public class AppMapper : IAppMapper
    {
        public AppMapper()
        {
            var config = new MapperConfiguration(config =>
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                config.AddMaps(assemblys);
            });
            Current = config.CreateMapper();
        }

        public IMapper Current { get; }

        public T Map<T>(object source) => Current.Map<T>(source);
    }
}
