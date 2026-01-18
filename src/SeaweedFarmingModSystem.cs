using Vintagestory.API.Common;

namespace SeaweedFarming
{
    public class SeaweedFarmingModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockClass("CultivatedSeaweed", typeof(BlockCultivatedSeaweed));
            api.RegisterBlockEntityClass("CultivatedSeaweedEntity", typeof(BlockEntityCultivatedSeaweed));
            api.RegisterBlockBehaviorClass("BreakWholeStack", typeof(BlockBehaviorBreakWholeStack));
        }
    }
}