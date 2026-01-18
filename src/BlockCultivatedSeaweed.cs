using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using System;

namespace SeaweedFarming
{
    public class BlockCultivatedSeaweed : Block
    {
        private int maxHeight = 10;         // Config: Max height
        
        // Cache asset locations for performance
        private readonly AssetLocation kelpSectionCode = new AssetLocation("game:seaweed-section");
        private readonly AssetLocation kelpTopCode = new AssetLocation("game:seaweed-top");

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            BlockPos targetPos = blockSel.Position.AddCopy(blockSel.Face);

            // Check both Default and Fluid layers for saltwater
            Block defaultBlock = world.BlockAccessor.GetBlock(targetPos, BlockLayersAccess.Default);
            Block liquidBlock = world.BlockAccessor.GetBlock(targetPos, BlockLayersAccess.Fluid);
            
            if (defaultBlock.LiquidCode == "saltwater" || liquidBlock.LiquidCode == "saltwater") 
            {
                return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
            }

            // Failed to find saltwater
            failureCode = "placesaltwateronly";
            return false;
        }

        public void TryGrow(IWorldAccessor world, BlockPos pos)
        {
            IBlockAccessor blockAccessor = world.BlockAccessor;

            // Scan upward to find where to grow next
            int currentHeight = 0;
            BlockPos checkPos = pos.UpCopy();

            while (currentHeight < maxHeight)
            {
                Block solidBlock = blockAccessor.GetBlock(checkPos, BlockLayersAccess.Default);
                Block fluidBlock = blockAccessor.GetBlock(checkPos, BlockLayersAccess.Fluid);

                // If there's already seaweed here, count it and continue upward
                if (solidBlock.Code.Equals(kelpSectionCode) || solidBlock.Code.Equals(kelpTopCode))
                {
                    currentHeight++;
                    checkPos.Up();
                    continue;
                }

                // Stop if blocked by a non-saltwater solid
                if (solidBlock.Id != 0 && solidBlock.LiquidCode != "saltwater") return;

                // If we found saltwater, grow here
                if (solidBlock.LiquidCode == "saltwater" || fluidBlock.LiquidCode == "saltwater")
                {
                    GrowSeaweed(world, checkPos, blockAccessor);
                    return;
                }
                
                // No saltwater found (air or fresh water) - stop growing
                return;
            }
        }

        private void GrowSeaweed(IWorldAccessor world, BlockPos targetPos, IBlockAccessor blockAccessor)
        {
            Block topBlock = world.GetBlock(kelpTopCode);
            Block sectionBlock = world.GetBlock(kelpSectionCode);
            
            if (topBlock == null || sectionBlock == null) return;

            // Convert the block below to a section if it was previously a top
            BlockPos belowPos = targetPos.DownCopy();
            Block belowBlock = blockAccessor.GetBlock(belowPos);

            if (belowBlock.Code.Path.Equals("seaweed-top"))
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