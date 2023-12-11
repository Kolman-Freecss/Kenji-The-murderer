#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Gameplay.GameplayObjects.UI
{
    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] private Image weaponSprite;
        [SerializeField] private TextMeshProUGUI index;

        public void SetWeaponSprite(bool isCurrentWeapon)
        {
            if (isCurrentWeapon)
            {
                weaponSprite.color = Color.red;
                index.color = Color.red;
            }
            else
            {
                weaponSprite.color = Color.black;
                index.color = Color.black;
            }
        }
    }
}