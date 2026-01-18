using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using System;
using Vintagestory.GameContent;

namespace SeaweedFarming
{
    public class BlockCultivatedSeaweed : BlockWaterPlant
    {
        // Cache asset locations for performance
        private readonly AssetLocation kelpSectionCode = new AssetLocation("seaweedfarming:cultivatedseaweed-section");
        private readonly AssetLocation kelpTopCode = new AssetLocation("seaweedfarming:cultivatedseaweed-top");

        public override string RemapToLiquidsLayer => "saltwater-still-7";

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            BlockPos targetPos = blockSel.Position.AddCopy(blockSel.Face);

            // Check both Default and Fluid layers for saltwater
            Block? defaultBlock = world.BlockAccessor.GetBlock(targetPos, BlockLayersAccess.Default);
            Block? liquidBlock = world.BlockAccessor.GetBlock(targetPos, BlockLayersAccess.Fluid);
            
            if ((defaultBlock != null && defaultBlock.LiquidCode == "saltwater") || 
                (liquidBlock != null && liquidBlock.LiquidCode == "saltwater")) 
            {
                return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
            }

            // Failed to find saltwater
            failureCode = "placesaltwateronly";
            return false;
        }

        public override bool CanPlantStay(IBlockAccessor blockAccessor, BlockPos pos)
        {
            Block? blockBelow = blockAccessor.GetBlock(pos.DownCopy());
            
            if (blockBelow == null || blockBelow.Code == null) return false;

            // If we are the base block (cultivated-seaweed), we rely on default behavior
            if (Code.Path == "cultivated-seaweed")
            {
                return true; 
            }

            // For Section and Top blocks:
            if (blockBelow.Code.Equals(new AssetLocation("seaweedfarming:cultivated-seaweed")) || 
                blockBelow.Code.Equals(kelpSectionCode))
            {
                return true;
            }

            return false;
        }

        public bool TryGrow(IWorldAccessor world, BlockPos pos)
        {
            IBlockAccessor blockAccessor = world.BlockAccessor;

            // Scan upward to find where to grow next
            int currentHeight = 0;
            BlockPos checkPos = pos.UpCopy();

            while (currentHeight < SeaweedFarmingModSystem.Config.MaxHeight)
            {
                Block? solidBlock = blockAccessor.GetBlock(checkPos, BlockLayersAccess.Default);
                Block? fluidBlock = blockAccessor.GetBlock(checkPos, BlockLayersAccess.Fluid);

                if (solidBlock == null || fluidBlock == null) return false;

                // If there's already seaweed here, count it and continue upward
                if (solidBlock.Code.Equals(kelpSectionCode) || solidBlock.Code.Equals(kelpTopCode))
                {
                    currentHeight++;
                    checkPos.Up();
                    continue;
                }

                // Stop if blocked by a non-saltwater solid
                if (solidBlock.Id != 0 && solidBlock.LiquidCode != "saltwater") return false;

                // If we found saltwater, grow here
                if (solidBlock.LiquidCode == "saltwater" || fluidBlock.LiquidCode == "saltwater")
                {
                    GrowSeaweed(world, checkPos, blockAccessor);
                    return true;
                }
                
                // No saltwater found (air or fresh water) - stop growing
                return false;
            }
            
            // Reached max height
            return false;
        }

        private void GrowSeaweed(IWorldAccessor world, BlockPos targetPos, IBlockAccessor blockAccessor)
        {
            Block? topBlock = world.GetBlock(kelpTopCode);
            Block? sectionBlock = world.GetBlock(kelpSectionCode);
            
            if (topBlock == null || sectionBlock == null) return;

            // Convert the block below to a section if it was previously a top
            BlockPos belowPos = targetPos.DownCopy();
            Block? belowBlock = blockAccessor.GetBlock(belowPos);

            if (belowBlock != null && belowBlock.Code.Equals(kelpTopCode))
            {
                blockAccessor.SetBlock(sectionBlock.Id, belowPos);
            }

            // Place the new top piece at target position
            blockAccessor.SetBlock(topBlock.Id, targetPos);
            
            // Notify neighbors for physics updates
            blockAccessor.TriggerNeighbourBlockUpdate(targetPos);
        }
    }
}