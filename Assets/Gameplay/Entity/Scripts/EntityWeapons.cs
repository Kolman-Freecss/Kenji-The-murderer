#region

using Gameplay.GameplayObjects.UI;
using UnityEngine;

#endregion

public class EntityWeapons : MonoBehaviour
{
    [SerializeField] private Transform weaponsParent;
    [SerializeField] private Transform weaponUIParent;

    Weapon[] weapons;
    WeaponUI[] weaponsUI = new WeaponUI[4];
    private int currentWeapon = -1;
    private bool haveWeapon = false;
    private AudioSource weaponsAudioSource;


    private void Awake()
    {
        weapons = weaponsParent.GetComponentsInChildren<Weapon>();
        weaponsUI = weaponUIParent.GetComponentsInChildren<WeaponUI>();
        currentWeapon = weapons.Length > 0 ? 0 : -1;
        weaponsAudioSource = weaponsParent.GetComponent<AudioSource>();

        SetCurrentWeapon(currentWeapon);
    }

    internal void SelectNextWeapon()
    {
        int nextWeapon = currentWeapon + 1;
        if (nextWeapon >= weapons.Length)
        {
            nextWeapon = -1;
        }

        SetCurrentWeapon(nextWeapon);
    }

    internal void SelectPreviousWeapon()
    {
        int prevWeapon = currentWeapon - 1;
        if (prevWeapon < -1)
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

        int prevWeapon = currentWeapon == -1 ? 0 : currentWeapon;
        currentWeapon = selectedWeapon;
        haveWeapon = currentWeapon != -1;
        if (haveWeapon)
        {
            weaponsUI[prevWeapon].SetWeaponSprite(false);
            weaponsUI[currentWeapon].SetWeaponSprite(true);
        }
        else
        {
            weaponsUI[prevWeapon].SetWeaponSprite(false);
        }

        weaponsAudioSource.Play();
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