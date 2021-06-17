using System;
using System.Collections.Generic;
using UnityEngine;
namespace TABZMoreGunsMod.WeaponHandlerEditingHandler
{
    public class WeaponHandlerEditingHelper
    {
        static public List<WeaponWeapperResourcesNames> weaponsToAdd = new List<WeaponWeapperResourcesNames>();
        static public void AddWeaponToList(string WeaponName, WeaponSpawner WeaponSpawningMethod)
        {
            var itemExampleHolder = new GameObject(WeaponName + "ItemHolder").AddComponent<InventoryItemWeapon>();
            itemExampleHolder.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(itemExampleHolder);
            InventoryItemEditingHelper.InventoryItemEditing.DisplayNameRef(itemExampleHolder) = WeaponName;
            weaponsToAdd.Add(new WeaponWeapperResourcesNames
            {
                Item = itemExampleHolder,
                WeaponSpawningMethod = WeaponSpawningMethod
            });
        }

        public struct WeaponWeapperResourcesNames
        {
            public InventoryItemWeapon Item;
            public WeaponSpawner WeaponSpawningMethod;
        }
        public delegate Weapon WeaponSpawner(Transform playerTransform);
    }
}
