using Vintagestory.API.Common;

namespace SeaweedFarming
{
    public class SeaweedFarmingModSystem : ModSystem
    {
        public static SeaweedConfig Config { get; private set; } = new SeaweedConfig();

        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockClass("CultivatedSeaweed", typeof(BlockCultivatedSeaweed));
            api.RegisterBlockEntityClass("CultivatedSeaweedEntity", typeof(BlockEntityCultivatedSeaweed));
            api.RegisterBlockBehaviorClass("BreakWholeStack", typeof(BlockBehaviorBreakWholeStack));

            try
            {
                var loadedConfig = api.LoadModConfig<SeaweedConfig>("SeaweedFarming.json");
                if (loadedConfig != null)
                {
                    Config = loadedConfig;
                    api.Logger.Notification("Mod Config successfully loaded.");
                }
                else
                {
                    api.Logger.Notification("No Mod Config specified. Falling back to default settings");
                    Config = new SeaweedConfig();
                }
            }
            catch
            {
                Config = new SeaweedConfig();
                api.Logger.Error("Failed to load custom mod configuration. Falling back to default settings!");
            }
            finally
            {
                api.StoreModConfig(Config, "SeaweedFarming.json");
            }
        }
    }
}