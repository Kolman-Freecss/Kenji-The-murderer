#region

using System;
using Entity.Scripts;
using Gameplay.GameplayObjects.Interactables._derivatives;
using UnityEngine;
using AnimationEvent = Gameplay.Events.AnimationEvent;

#endregion

namespace Gameplay.GameplayObjects.Player.Script
{
    /// <summary>
    /// Player stats class.
    /// @author: Kolman-Freecss
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private AudioClip m_OnMeatingClip;

        EntityLife m_EntityLife;
        EntityAnimation m_EntityAnimation;
        AudioSource m_AudioSource;
        AnimationEvent animationEvent;

        private MeatInteractable currentMeatInteraction;

        private void Awake()
        {
            m_EntityLife = GetComponent<EntityLife>();
            m_EntityAnimation = GetComponent<EntityAnimation>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            animationEvent = GetComponentInChildren<AnimationEvent>();
            animationEvent.OnAnimationFinish += HandleAnimationFinished;
        }

        private void HandleAnimationFinished(int handleAnimationId)
        {
            if (handleAnimationId == (int)PlayerAnimationIds.Eat)
            {
                m_EntityLife.Heal(currentMeatInteraction.meatRecovery);
                Destroy(currentMeatInteraction.gameObject);
            }
        }

        public void OnMeatCollected(object obj)
        {
            try
            {
                MeatInteractable meat = obj as MeatInteractable;
                currentMeatInteraction = meat;
                if (m_EntityAnimation != null)
                {
                    m_EntityAnimation.GetAnimator().SetTrigger("ParasyteEat");
                }

                if (m_AudioSource != null && m_OnMeatingClip != null)
                {
                    m_AudioSource.PlayOneShot(m_OnMeatingClip);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}