using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    [CreateAssetMenu(fileName ="WeaponType", menuName = "MetroidVania/Weapon", order = 1)]
    public class WeaponTypes : ScriptableObject
    {
        public GameObject projectile;
        public float projectileSpeed;
        public int ammountToPool;
    }
}