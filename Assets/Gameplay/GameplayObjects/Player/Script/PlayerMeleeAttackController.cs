#region

using Entity.Scripts;
using Systems.WeaponSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using AnimationEvent = Gameplay.Events.AnimationEvent;

#endregion

namespace Gameplay.GameplayObjects.Player.Script
{
    public class PlayerMeleeAttackController : EntityMeleeAttack
    {
        [SerializeField] private InputActionReference meleeAttackInput;

        [SerializeField] private MeleeWeapon meleeWeapon;

        private EntityAnimation entityAnimation;
        private EntityWeapons entityWeapons;
        private AnimationEvent animationEvent;

        private void Awake()
        {
            if (meleeWeapon == null)
            {
                Debug.LogWarning("Melee weapon is not set!");
            }

            entityAnimation = GetComponent<EntityAnimation>();
            entityWeapons = GetComponent<EntityWeapons>();
            animationEvent = GetComponentInChildren<AnimationEvent>();
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

        public void HandleAnimationFinished()
        {
            if (entityWeapons.HasCurrentWeapon())
            {
                entityWeapons.GetCurrentWeapon().gameObject.SetActive(true);
            }

            meleeWeapon.gameObject.SetActive(false);
        }

        public void PerformInitAttack()
        {
            if (entityAnimation != null)
            {
                if (entityWeapons.HasCurrentWeapon())
                {
                    entityWeapons.GetCurrentWeapon().gameObject.SetActive(false);
                }

                meleeWeapon.gameObject.SetActive(true);
                entityAnimation.GetAnimator().SetTrigger("KatanaSlash");
            }
        }

        public override void PerformAttack()
        {
            meleeWeapon.Hit();
            base.PerformAttack();
        }

        private void OnDisable()
        {
            meleeAttackInput.action.Disable();
            meleeAttackInput.action.performed -= ctx => PerformInitAttack();
        }
    }
}