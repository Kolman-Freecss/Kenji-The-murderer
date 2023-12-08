#region

using UnityEngine;

#endregion

public class EntityWeapons : MonoBehaviour
{
    [SerializeField] private Transform weaponsParent;

    Weapon[] weapons;
    private int currentWeapon = -1;

    private void Awake()
    {
        weapons = weaponsParent.GetComponentsInChildren<Weapon>();
        currentWeapon = weapons.Length > 0 ? 0 : -1;

        SetCurrentWeapon(currentWeapon);
    }

    internal void SelectNextWeapon()
    {
        int nextWeapon = ++currentWeapon;
        if (nextWeapon >= weapons.Length)
        {
            nextWeapon = 0;
        }

        SetCurrentWeapon(nextWeapon);
    }

    internal void SelectPreviousWeapon()
    {
        int prevWeapon = --currentWeapon;
        if (prevWeapon < weapons.Length)
        {
            prevWeapon = weapons.Length - 1;
        }

        SetCurrentWeapon(prevWeapon);
    }

    public void SetCurrentWeapon(int selectedWeapon)
    {
        for (int j = 0; j < weapons.Length; j++)
        {
            weapons[j].gameObject.SetActive(j == selectedWeapon);
        }

        currentWeapon = selectedWeapon;
    }

    public void Shot()
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].Shot();
        }
    }

    public void StartShooting()
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].StartShooting();
        }
    }

    public void StopShooting()
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].StopShooting();
        }
    }

    public bool HasCurrentWeapon()
    {
        return currentWeapon != -1;
    }

    public Weapon GetCurrentWeapon()
    {
        return weapons[currentWeapon];
    }
}