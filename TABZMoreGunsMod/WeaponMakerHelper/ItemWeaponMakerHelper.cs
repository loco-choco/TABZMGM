using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TABZMoreGunsMod.InventoryItemEditingHelper;

namespace TABZMoreGunsMod.WeaponMakerHelper
{
    public class ItemWeaponMakerHelper
    {

        /// <summary>
        /// Creates a InventoryWeaponItem from a Cube Primitive and from the settings. It doesn't add the item to RuntimeResources
        /// </summary>
        /// <param name="settings"></param>
        /// <returns> Returns the item from the specified settings</returns>
        public static InventoryItemWeapon MakeItemWeapon(ItemWeaponSettings settings)
        {
            GameObject NewItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
            NewItem.GetComponent<MeshFilter>().mesh = settings.ItemMesh;
            NewItem.GetComponent<MeshRenderer>().material = settings.ItemMaterial;
            NewItem.name = settings.DisplayName+"Item";
            NewItem.layer = LayerMask.NameToLayer("Item");
            NewItem.AddComponent<PhotonView>();
            InventoryItemWeapon itemComponent = NewItem.AddComponent<InventoryItemWeapon>();

            InventoryItemEditing.DisplayNameRef(itemComponent) = settings.DisplayName;
            InventoryItemEditing.FlavourTextRef(itemComponent) = settings.FavourText;
            InventoryItemEditing.ItemIconRef(itemComponent) = Sprite.Create(settings.ItemIcon, new Rect(0.0f, 0.0f, settings.ItemIcon.width, settings.ItemIcon.height), new Vector2(0.5f, 0.5f), 100.0f);

            InventoryItemEditing.ItemTypeRef(itemComponent) = InventoryService.ItemType.WEAPON;
            InventoryItemEditing.AmmoTypeRef(itemComponent) = settings.AmmoType;
            itemComponent.BulletsInMagazine = settings.BulletsInMagazine;

            return itemComponent;
        }
    }
    public struct ItemWeaponSettings
    {
        public string DisplayName;
        public string FavourText;
        public Texture2D ItemIcon;
        public InventoryService.AmmoType AmmoType;
        public Mesh ItemMesh;
        public Material ItemMaterial;
        public int BulletsInMagazine;
    }
}
