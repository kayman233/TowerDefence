﻿using UnityEngine;

namespace Turret.Weapon.Field
{
    [CreateAssetMenu(menuName = "Assets/Turret Field Weapon Asset", fileName = "Turret Field Weapon Asset")]

    public class TurretFieldWeaponAsset: TurretWeaponAssetBase
    {
        public float MaxDistance;

        public GameObject FieldPrefab;

        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretFieldWeapon(this, view);
        }
    }
}