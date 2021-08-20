using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TABZMoreGunsMod.WeaponMakerHelper;
using UnityEngine;

namespace TABZMoreGunsMod.CustomWeaponsSettings
{
    public class WeaponGenerator
    {
        private static FireWeaponSettings[] FireWeaponSettings;
        private static MeleeWeaponSettings[] MeleeSettings;

        static public void GenarateFireWeaponSettings(CustomFireWeaponSettings[] settings)
        {
            FireWeaponSettings = new FireWeaponSettings[settings.Length];
            for (int i = 0; i < FireWeaponSettings.Length; i++)
                FireWeaponSettings[i] = settings[i].ToFireWeaponSettings();
        }
        static public void GenarateMeleeWeaponSettings(CustomMeleeSettings[] settings)
        {
            MeleeSettings = new MeleeWeaponSettings[settings.Length];
            for (int i = 0; i < FireWeaponSettings.Length; i++)
                MeleeSettings[i] = settings[i].ToMeleeWeaponSettings();
        }

        static public Weapon[] CreateWeaponsFromSettings(Transform playerTransform)
        {
            Weapon[] weapons = new Weapon[FireWeaponSettings.Length + MeleeSettings.Length];



            return weapons;
        }
    }
}