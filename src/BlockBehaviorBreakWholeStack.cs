using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace SeaweedFarming
{
    public class BlockBehaviorBreakWholeStack : BlockBehavior
    {
        private AssetLocation? matchCode;

        public BlockBehaviorBreakWholeStack(Block block) : base(block)
        {
        }

        public override void Initialize(JsonObject properties)
        {
            base.Initialize(properties);
            string? code = properties["matchCode"].AsString();
            if (code != null)
            {
                matchCode = new AssetLocation(code);
            }
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
        {
            // Only handle server-side logic
            if (world.Side == EnumAppSide.Server)
            {
                BlockPos lowestPos = pos.Copy();
                
                // Scan downwards until we hit the bottom of the map or a non-matching block
                while (lowestPos.Y > 0)
                {
                    BlockPos belowPos = lowestPos.DownCopy();
                    Block belowBlock = world.BlockAccessor.GetBlock(belowPos);
                    
                    if (IsPartOfStack(belowBlock))
                    {
                        lowestPos.Down();
                    }
                    else
                    {
                        // Found the bottom
                        break;
                    }
                }

                if (lowestPos != pos)
                {
                    // Break the lowest block to collapse the stack
                    world.BlockAccessor.BreakBlock(lowestPos, byPlayer);
                    
                    // Prevent this block from breaking normally, letting the stack collapse handle it
                    handling = EnumHandling.PreventDefault;
                }
            }
            
            base.OnBlockBroken(world, pos, byPlayer, ref handling);
        }

        private bool IsPartOfStack(Block? block)
        {
            if (block == null || block.Code == null) return false;
            
            if (matchCode != null)
            {
                return Vintagestory.API.Util.WildcardUtil.Match(matchCode, block.Code);
            }
            
            return false;
        }
    }
}
