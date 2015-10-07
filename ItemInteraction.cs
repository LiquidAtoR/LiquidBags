using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;


namespace PluginLiquidBags3
{
    class ItemInteraction
    {
        public static void UseItem(WoWItem item, bool extraSleep = false)
        {
            if (extraSleep)
            {
                // some (soulbound) items require an additional sleep to prevent a loot bug
                StyxWoW.SleepForLagDuration();
            }
            Lua.DoString("UseItemByName(\"" + item.Name + "\")"); // use item via LUA

            StyxWoW.SleepForLagDuration();
        }

        public static void DropItem(WoWItem item)
        {
            // Deleting an item is a two step process in wow, I => pick it up  II => delete item on cursor
            item.PickUp(); // use HB api to do the picky upping
            Lua.DoString("DeleteCursorItem()");

            StyxWoW.SleepForLagDuration();
        }
    }
}
