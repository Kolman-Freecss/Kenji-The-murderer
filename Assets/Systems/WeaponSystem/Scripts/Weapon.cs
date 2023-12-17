#region

using UnityEngine;

#endregion

public class Weapon : MonoBehaviour
{
    public enum ShotMode
    {
        ShotByShot,
        Continuous
    };

    [Header("Weapon Settings")] [SerializeField]
    private AudioSource shootAudioSource;

    [SerializeField] private AudioClip shootAudioClip;
    public MoveToAimingPlanePosition moveToAimingPlanePosition;

    public ShotMode shotMode;

    Barrel[] barrels;

    private void Awake()
    {
        barrels = GetComponentsInChildren<Barrel>();
        shootAudioSource.clip = shootAudioClip;
    }

    private void SetSoundVolume()
    {
        shootAudioSource.volume = SoundManager.Instance.EffectsAudioVolume / 100f;
    }

    private void Start()
    {
        SetSoundVolume();
    }

    public void PlayShotSound()
    {
        SetSoundVolume();
        shootAudioSource.PlayOneShot(shootAudioClip);
    }

    public void Shot()
    {
        foreach (Barrel barrel in barrels)
        {
            barrel.Shot();
        }
    }

    public void StartShooting()
    {
        foreach (Barrel barrel in barrels)
        {
            barrel.StartShooting();
        }
    }

    public void StopShooting()
    {
        foreach (Barrel barrel in barrels)
        {
            barrel.StopShooting();
        }
    }
}