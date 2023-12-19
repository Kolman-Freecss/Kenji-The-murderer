#region

using Entity.Scripts;
using Systems.WeaponSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using AnimationEvent = Gameplay.Events.AnimationEvent;

#endregion

namespace Gameplay.GameplayObjects.Player.Script
{
    /// <summary>
    /// @author: Kolman Freecss
    /// </summary>
    public class PlayerMeleeAttackController : EntityMeleeAttack
    {
        [Header("Player Settings")] [SerializeField]
        private InputActionReference meleeAttackInput;

        [SerializeField] private MeleeWeapon meleeWeapon;
        [SerializeField] float cadence = 0.3f; // Slash/s | 1 slash - 3s

        private EntityAnimation entityAnimation;
        private EntityWeapons entityWeapons;
        private AnimationEvent animationEvent;
        private PlayerController playerController;

        private float nextSlashTime = 0f;

        private void Awake()
        {
            if (meleeWeapon == null)
            {
                Debug.LogWarning("Melee weapon is not set!");
            }

            entityAnimation = GetComponent<EntityAnimation>();
            entityWeapons = GetComponent<EntityWeapons>();
            animationEvent = GetComponentInChildren<AnimationEvent>();
            playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            meleeAttackInput.action.Enable();
            meleeAttackInput.action.performed += ctx => PerformInitAttack();
        }

        private void Start()
        {
            meleeWeapon.gameObject.SetActive(false);
            animationEvent.OnAnimationFinish += HandleAnimationFinished;
        }

        public void HandleAnimationFinished(int handleAnimationId)
        {
            if (entityWeapons.HasCurrentWeapon())
            {
                entityWeapons.GetCurrentWeapon().gameObject.SetActive(true);
            }

            playerController.meleeAttacking = false;
            meleeWeapon.gameObject.SetActive(false);
        }

        public void PerformInitAttack()
        {
            if (Time.time > nextSlashTime)
            {
                nextSlashTime = Time.time + 1f / cadence;
                if (entityAnimation != null)
                {
                    if (entityWeapons.HasCurrentWeapon())
                    {
                        entityWeapons.GetCurrentWeapon().gameObject.SetActive(false);
                    }

                    playerController.meleeAttacking = true;
                    meleeWeapon.gameObject.SetActive(true);
                    meleeWeapon.Hit();
                    entityAnimation.GetAnimator().SetTrigger("KatanaSlash");
                }
            }
        }

        public override void PerformAttack()
        {
            base.PerformAttack();
        }

        private void OnDisable()
        {
            meleeAttackInput.action.Disable();
            meleeAttackInput.action.performed -= ctx => PerformInitAttack();
        }
    }
}