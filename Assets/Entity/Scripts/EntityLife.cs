#region

using UnityEngine;
using UnityEngine.AI;

#endregion

public class EntityLife : MonoBehaviour
{
    [SerializeField] private float minDeathPushForce = 3000f;
    [SerializeField] private float maxDeathPushForce = 4500f;
    [SerializeField] float maxLife = 3f;

    [Header("Debug")] [SerializeField] private bool debugHit;
    [SerializeField] Transform debugOffender;

    HurtBox hurtBox;
    private EntityRagdollizer entityRagdollizer;
    private float currentLife;
    Animator animator;
    NavMeshAgent navMeshAgent;
    PlayerController playerController;
    CharacterController characterController;
    LifeBar lifeBar;
    private Enemy enemy;

    private void OnValidate()
    {
        if (debugHit)
        {
            debugHit = false;
            OnHitNotifiedWithOffender(debugOffender ? debugOffender : transform);
        }
    }

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
        lifeBar = GetComponentInChildren<LifeBar>();
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
            lifeBar.SetNormalizedValue(Mathf.Clamp01(currentLife / maxLife));
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