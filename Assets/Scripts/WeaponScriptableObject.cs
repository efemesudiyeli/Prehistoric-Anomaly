using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "WeaponScriptableObject", order = 0)]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponScriptableObjectData WeaponSettings;

    // // Attack logics based of weapon type
    // public void Attack()
    // {
    //     switch (WeaponSettings.WeaponType)
    //     {
    //         case WeaponScriptableObjectData.WeaponTypes.BAT:
    //             BatAttack();
    //             break;
    //         case WeaponScriptableObjectData.WeaponTypes.AXE:
    //             AxeAttack();
    //             break;
    //     }
    // }

    // // Weapon Logics
    // private void BatAttack()
    // {
    //     Debug.Log("Bat Attack!");
    // }

    // private void AxeAttack()
    // {
    //     Debug.Log("Axe Attack!");
    // }

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
            AXE,
        }

        public string WeaponName;
        public WeaponTypes WeaponType;
        public float WeaponAttackDamage;
        public float WeaponAttackSpeed;
        public AnimatorController AnimatorController;

    }
}