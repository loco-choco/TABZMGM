using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TABZMoreGunsMod.WeaponHandlerEditingHandler;
using TABZMoreGunsMod.WeaponMakerHelper;
using UnityEngine;

namespace TABZMoreGunsMod.CustomWeaponsSettings
{
    public class FireWeaponGenerator
    {
        private FireWeaponSettings WeaponFireSettings;
        public string Name { get { return WeaponFireSettings.Name; } set { } }
        public FireWeaponGenerator(CustomFireWeaponSettings settings)
        {
            WeaponFireSettings = settings.ToFireWeaponSettings();
            Debug.Log(Name);
            WeaponHandlerEditingHelper.AddWeaponToList(settings.Name, settings.ToWeaponSoundWrapper(), MakeWeapon);
        }

       public Weapon MakeWeapon(Transform playerTransform)
        {
            try
            {
                return FireWeaponMakerHelper.MakeFireWeapon(playerTransform, WeaponFireSettings);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Source);
                Debug.Log(ex.StackTrace);
                Debug.Log(ex.InnerException.Message);

                return null;
            }
        }
    }

    public class MeleeWeaponGenerator
    {
        private MeleeWeaponSettings WeaponMeleeSettings;
        public string Name { get { return WeaponMeleeSettings.Name; } set { } }
        public MeleeWeaponGenerator(CustomMeleeSettings settings)
        {
            WeaponMeleeSettings = settings.ToMeleeWeaponSettings();
            WeaponHandlerEditingHelper.AddWeaponToList(settings.Name, settings.ToWeaponSoundWrapper(), MakeWeapon);
        }
        public Weapon MakeWeapon(Transform playerTransform)
        {
            try
            {
                return MeleeMakerHelper.MakeMeleeWeapon(playerTransform, WeaponMeleeSettings);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Source);
                Debug.Log(ex.StackTrace);
                Debug.Log(ex.InnerException.Message);

                return null;
            }
        }
    }
}
