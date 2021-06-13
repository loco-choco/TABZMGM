using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace TABZMoreGunsMod.InventoryItemEditingHelper
{
    public class InventoryItemEditing
    {
        static public ref string DisplayNameRef(InventoryItem item)
        {
            return ref AccessTools.FieldRefAccess<InventoryItem,string>(item, "m_displayName");
        }
        static public ref string FlavourTextRef(InventoryItem item)
        {
            return ref AccessTools.FieldRefAccess<InventoryItem, string>(item, "m_flavourText");
        }

        static public ref InventoryService.ItemType ItemTypeRef(InventoryItem item)
        {
            return ref AccessTools.FieldRefAccess<InventoryItem, InventoryService.ItemType>(item, "m_type");
        }
        static public ref Sprite ItemIconRef(InventoryItem item)
        {
            return ref AccessTools.FieldRefAccess<InventoryItem, Sprite>(item, "m_itemIcon");
        }
    }
}
