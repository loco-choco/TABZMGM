using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace TABZMoreGunsMod
{
    public class WeaponHandlerEditingHelper
    {
        static private WeaponHandler WeaponHandler;
        static private List<WeaponHandler.WeaponWrapper> weapons;
        static public void AddWeaponWraperToList(WeaponHandler.WeaponWrapper wrapper)
        {
            if (WeaponHandler == null)
            {
                WeaponHandler = GameObject.FindObjectOfType<WeaponHandler>();
                weapons = WeaponHandler.m_weaponList.ToList();
            }
            wrapper.m_weapon.transform.parent = WeaponHandler.transform;
            weapons.Add(wrapper);
            WeaponHandler.m_weaponList = weapons.ToArray();
        }
    }
}
