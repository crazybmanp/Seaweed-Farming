using System;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace SeaweedFarming
{
    public class BlockEntityCultivatedSeaweed : BlockEntity
    {
        private double nextGrowthTotalHours;
        
        private const double BaseGrowthHours = 36.0;  // ~1.5 days
        private const double VariancePercent = 0.25;  // ±25%
        private const int TickIntervalMs = 180000;     // 3 minutes

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            // Only run on server
            if (api.Side == EnumAppSide.Server)
            {
                // If this is a new block entity (not loaded from save), schedule the first growth
                if (nextGrowthTotalHours <= 0)
                {
                    ScheduleNextGrowth();
                }

                // Register tick listener - checks every 3 minutes
                RegisterGameTickListener(OnServerTick, TickIntervalMs);
            }
        }

        private void OnServerTick(float dt)
        {
            // Check if it's time to grow
            if (Api.World.Calendar.TotalHours >= nextGrowthTotalHours)
            {
                // Get the block and call its growth logic
                BlockCultivatedSeaweed block = Block as BlockCultivatedSeaweed;
                if (block != null)
                {
                    block.TryGrow(Api.World, Pos);
                    // Schedule the next growth
                    ScheduleNextGrowth();
                    MarkDirty();
                }
            }
        }

        private void ScheduleNextGrowth()
        {
            // Calculate random variance (±25% of base growth time)
            double variance = BaseGrowthHours * VariancePercent;
            double randomOffset = (Api.World.Rand.NextDouble() * 2 - 1) * variance;
            double growthInterval = BaseGrowthHours + randomOffset;

            // Set absolute timestamp for when this seaweed should next grow
            nextGrowthTotalHours = Api.World.Calendar.TotalHours + growthInterval;
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);
            
            if (nextGrowthTotalHours > 0)
            {
                double hoursUntilGrowth = nextGrowthTotalHours - Api.World.Calendar.TotalHours;
                
                if (hoursUntilGrowth > 0)
                {
                    double daysUntilGrowth = hoursUntilGrowth / 24.0;
                    dsc.AppendLine($"Next growth: {daysUntilGrowth:F1} days");
                }
                else
                {
                    dsc.AppendLine("Ready to grow");
                }
            }
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetDouble("nextGrowthTotalHours", nextGrowthTotalHours);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            base.FromTreeAttributes(tree, worldAccessForResolve);
            nextGrowthTotalHours = tree.GetDouble("nextGrowthTotalHours");
        }
    }
}
