#region

using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

#endregion

public class EntityLife : MonoBehaviour
{
    [SerializeField] private float minDeathPushForce = 3000f;
    [SerializeField] private float maxDeathPushForce = 4500f;
    [SerializeField] float maxLife = 3f;
    [SerializeField] Image lifeBar;

    HurtBox hurtBox;
    private EntityRagdollizer entityRagdollizer;
    private float currentLife;
    Animator animator;
    NavMeshAgent navMeshAgent;
    PlayerController playerController;
    CharacterController characterController;
    private Enemy enemy;

    private void Awake()
    {
        currentLife = maxLife;
        hurtBox = GetComponent<HurtBox>();
        entityRagdollizer = GetComponentInChildren<EntityRagdollizer>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        hurtBox.onHitNotifiedWithOffender.AddListener(OnHitNotifiedWithOffender);
    }

    private void OnDisable()
    {
        hurtBox.onHitNotifiedWithOffender.RemoveListener(OnHitNotifiedWithOffender);
    }

    private void OnHitNotifiedWithOffender(Transform offender)
    {
        if (currentLife > 0f)
        {
            currentLife -= 1f;
            lifeBar.DOFillAmount(currentLife / maxLife, 0.25f);
            if (currentLife <= 0f)
            {
                animator.enabled = false;

                if (navMeshAgent)
                {
                    navMeshAgent.enabled = false;
                }

                if (playerController)
                {
                    playerController.enabled = false;
                }

                if (characterController)
                {
                    characterController.enabled = false;
                }

                if (enemy)
                {
                    enemy.enabled = false;
                }

                entityRagdollizer.Ragdollize();
                entityRagdollizer.Push((transform.position - offender.position), minDeathPushForce, maxDeathPushForce);
            }
        }
    }
}