namespace PluginLiquidBags4
{
    using Styx;
    using Styx.Common;
    using Styx.Plugins;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;

    using System;
    using System.Drawing;
    using System.Diagnostics;
    using System.Windows.Media;

    public class LiquidBags4 : HBPlugin
    {
        public override string Name { get { return "LiquidBags"; } }
        public override string Author { get { return "LiquidAtoR"; } }
        public override Version Version { get { return new Version(4,0,0,0); } }

        private static Stopwatch sw = new Stopwatch();
        protected LBItemCollection _items;

        public override void Initialize()
        {
            this._items = ConfigParser.parseConfig(); // parse the config xml
        }

        public override void Pulse()
        {
            if (!sw.IsRunning)
            {
                sw.Start();
            }
            else if (sw.Elapsed.TotalMilliseconds < 5000) // throttle to run max every 5s
            {
                // time hasn't passed yet, so return
                return;
            }

            // return early if we cant' or shouldn;t use an item
            if (StyxWoW.Me.IsActuallyInCombat
                || StyxWoW.Me.Mounted
                || StyxWoW.Me.IsDead
                || StyxWoW.Me.IsGhost
                //|| Styx.CommonBot.LootTargeting.LootMobs // currently broken in the api, used to delay tidying up until everything is looted
                ) {
                return;
            }

            foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>()) // iterate over every item
            {
                if (item != null && item.BagSlot != -1) // check if item exists and is in bag
                {
                    if (this._items.Contains(Convert.ToInt32(item.Entry))) // look for item in collection
                    {
                        LBItem _item = _items[Convert.ToInt32(item.Entry)]; // get LBItem Obj for found Item
                        if (item.StackCount >= _item.StackSize) // check against supplied stack count
                        {
                            this.processItem(item, _item); // process the item
                        }
                    }
                }
            }

            StyxWoW.SleepForLagDuration();
            sw.Reset();
            sw.Start();
        }

        private void processItem(WoWItem wowItem, LBItem tbItem)
        {
            // LBItem holds the action to use, switch to the right method
            switch (lbItem.Action)
            {
                case "use":
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Using {2} we have {3}", this.Name, this.Version, wowItem.Name, wowItem.StackCount);
                    ItemInteraction.UseItem(wowItem, tbItem.NeedsSleep);
                    break;
                case "drop":
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Dropping {2}", this.Name, this.Version, wowItem.Name);
                    ItemInteraction.DropItem(wowItem);
                    break;
                case "keep":
                    // dummy action, does nothing as nothing is exactly what is meant
                    break;
                default:
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Trying to process \"{2}\" but Action \"{3}\" is unknown", this.Name, this.Version, wowItem.Name, tbItem.Action);
                    break;
            }
        }
    }
}
