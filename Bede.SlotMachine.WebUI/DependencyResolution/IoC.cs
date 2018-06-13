using Bede.SlotMachine.Engine;
using StructureMap;
namespace Bede.SlotMachine.WebUI.DependencyResolution
{
    public static class IoC
    {
        public static StructureMap.IContainer Initialize()
        {
            return new Container(_ =>
            {
                _.For<ISlotEngine>().Use<SlotEngine>();
            });
        }
    }
}