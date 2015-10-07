using System.Collections.ObjectModel;

namespace PluginLiquidBags3
{
    /**
     * Generic class to hold data about Items processed by TidyBags
     */
    public class LBItem
    {
        protected int itemId;
        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; } 
        }

        protected int stackSize;
        public int StackSize
        {
            get { return this.stackSize; }
            set { this.stackSize = value; } 
        }

        protected bool needsSleep;
        public bool NeedsSleep
        {
            get { return this.needsSleep; }
            set { this.needsSleep = value; } 
        }

        protected string action;
        public string Action
        {
            get { return this.action; }
            set { this.action = value; }
        }

        public LBItem(int itemId, int stackSize, bool needsSleep, string action)
        {
            this.itemId = itemId;
            this.stackSize = stackSize;
            this.needsSleep = needsSleep;
            this.action = action;
        }
    }

    /**
     * Generic Collection to access stored LBItems parsed from xml file
     */
    public class LBItemCollection : KeyedCollection<int, LBItem>
    {
        public LBItemCollection() : base() {}
    
        protected override int GetKeyForItem(LBItem item)
        {
            return item.ItemId;
        }
    }
}
