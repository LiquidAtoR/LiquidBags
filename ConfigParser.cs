using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Styx;
using Styx.Common;
using System.Windows.Media;

using PluginLiquidBags3;

namespace PluginLiquidBags3
{
    class ConfigParser
    {
        public static LBItemCollection parseConfig()
        {
            LBItemCollection itemObjs = new LBItemCollection(); // init collection

            itemObjs = parseItemsXml("plugins\\LiquidBags\\use_items.xml", itemObjs); // parse the xml we ship
            itemObjs = parseItemsXml("plugins\\LiquidBags\\discard_items.xml", itemObjs); // parse the xml a user might add

            return itemObjs;
        }

        private static LBItemCollection parseItemsStructure(IEnumerable<XElement> items, LBItemCollection collection)
        {
            foreach (XElement item in items) // process all items
            {
                int id = Int32.Parse(item.Attribute("id").Value); // parse id attribute
                int stack = Int32.Parse(item.Attribute("stack").Value); // parse stack attribute
                bool sleep = Boolean.Parse(item.Attribute("sleep").Value); // parse sleep attribute
                string action = item.Attribute("action").Value; // parse action attribute
                LBItem tempItem = new LBItem(id, stack, sleep, action); // generate new LBItem object

                // check if element is an override, then throw it out
                if (collection.Contains(tempItem.ItemId))
                {
                    collection.Remove(tempItem.ItemId);
                }

                collection.Add(tempItem); // add object to collection
            }

            return collection;
        }

        private static LBItemCollection parseItemsXml(string pathToXml, LBItemCollection collection)
        {
            if (!File.Exists(pathToXml))
            {
                return collection;
            }
            XDocument xml = XDocument.Load(pathToXml); // load xml from path
            var items = xml.Descendants("item"); // put all <item> tags into a variable

            return parseItemsStructure(items, collection); // parse the xml
        }
    }
}
