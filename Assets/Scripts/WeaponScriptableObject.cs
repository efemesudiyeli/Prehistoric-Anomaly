using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "WeaponScriptableObject", order = 0)]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponScriptableObjectData WeaponSettings;

    public bool IsWeaponEquipped()
    {
        if (WeaponSettings.WeaponType == WeaponScriptableObjectData.WeaponTypes.EMPTY)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Serializable Class
    [Serializable]
    public class WeaponScriptableObjectData
    {
        public enum WeaponTypes
        {
            EMPTY,
            BAT,
            BOW,
            AK47,
            RPG,
            LIGHTSABER,
            ATOMICBOMB
        }

        public string WeaponName;
        public WeaponTypes WeaponType;
        public float WeaponAttackDamage;
        public float WeaponAttackSpeed;
        public float WeaponKnockbackPower;
        public GameObject WeaponProjectile;
        public AudioClip WeaponAttackSound;

    }
}