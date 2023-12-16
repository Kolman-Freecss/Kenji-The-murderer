#region

using UnityEngine;

#endregion

namespace Systems.WeaponSystem.Scripts
{
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("Weapon Settings")] [SerializeField]
        private AudioSource shootAudioSource;

        [SerializeField] private AudioClip shootAudioClip;

        private void Awake()
        {
            shootAudioSource.clip = shootAudioClip;
        }

        private void SetSoundVolume()
        {
            shootAudioSource.volume = SoundManager.Instance.EffectsAudioVolume;
        }

        private void Start()
        {
            SetSoundVolume();
        }

        private void PlayHitSound()
        {
            shootAudioSource.Play();
        }

        public void Hit()
        {
            PlayHitSound();
        }
    }
}