using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class ObjectPooler : MonoBehaviour
    {
        private GameObject currentItem;
        public void CreatePool(WeaponTypes weapon)
        {
            for (int i = 0; i < weapon.ammountToPool; i++)
            {
                currentItem = Instantiate(weapon.projectile);
                currentItem.SetActive(false);
            }
        }

    }
}