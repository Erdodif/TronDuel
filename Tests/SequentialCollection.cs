using TronLightCycle.GameObjects;

using Xunit;

namespace TronDuel.Model.Tests
{
    //Intended to ensure there's no conflict with the Global Player Collection
    [CollectionDefinition("SequentialCollection")]
    public class SequentialCollection : ICollectionFixture<SharedContext>
    {
    }
    public class SharedContext
    {
        static SharedContext()
        {
            Player.ClearPlayerList();
        }
    }
}
