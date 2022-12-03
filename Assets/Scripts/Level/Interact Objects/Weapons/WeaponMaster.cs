using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMaster : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    private bool init = false;
    private int _ammoMagazine;
    

    public void Init()
    {
        if (init) return;
        _ammoMagazine = weaponData.MountMaxAmmoInMagazine;
        init = true;
    }

    public void Reload(int ammoInStash)
    {
        if (ammoInStash >= weaponData.MountMaxAmmoInMagazine)
        {
            _ammoMagazine = weaponData.MountMaxAmmoInMagazine;
            return;
        }

        _ammoMagazine = ammoInStash;
    }

    public WeaponData WeaponData => weaponData;
    public int AmmoMagazine => _ammoMagazine;
}
