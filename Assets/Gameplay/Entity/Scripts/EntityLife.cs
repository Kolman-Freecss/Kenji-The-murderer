#region

using System;
using Gameplay.Config.Scripts;
using Gameplay.GameplayObjects.Interactables;
using Gameplay.GameplayObjects.Interactables._derivatives;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

#endregion

public class EntityLife : MonoBehaviour
{
    [SerializeField] private float minDeathPushForce = 3000f;
    [SerializeField] private float maxDeathPushForce = 4500f;
    [SerializeField] float maxLife = 3f;
    [SerializeField] float timeToDestroyAfterDeath = 2f;

    [Header("Debug")] [SerializeField] private bool debugHit;
    [SerializeField] Transform debugOffender;
    [SerializeField] float debugDamage = 1f;

    HurtBox hurtBox;
    private EntityRagdollizer entityRagdollizer;
    private float currentLife;
    Animator animator;
    NavMeshAgent navMeshAgent;
    PlayerController playerController;
    CharacterController characterController;
    LifeBar lifeBar;
    private Enemy enemy;

    public UnityEvent onDeath = new UnityEvent();

    private void OnValidate()
    {
        if (debugHit)
        {
            debugHit = false;
            OnHitNotifiedWithOffender(debugDamage, debugOffender ? debugOffender : transform);
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

    private void OnHitNotifiedWithOffender(float damage, Transform offender)
    {
        if (currentLife > 0f)
        {
            currentLife -= damage;
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
                    onDeath.Invoke();
                }

                if (characterController)
                {
                    characterController.enabled = false;
                    onDeath.Invoke();
                }

                if (enemy)
                {
                    enemy.enabled = false;
                    enemy.GetComponent<CapsuleCollider>().enabled = false;
                    MeatInteractable meat = null;
                    try
                    {
                        meat = Instantiate(
                                ((Interactable<MeatInteractable>)RoundManager.Instance.GetDynamicAsset(RoundManager
                                    .DynamicAssets.Meat)).gameObject, transform.position, Quaternion.identity)
                            .GetComponent<MeatInteractable>();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("No meat interactable found in the scene");
                    }

                    if (meat != null)
                    {
                        meat.meatRecovery = GetMeatRecoveryByEnemy(enemy.GetEnemyType());
                    }
                }

                entityRagdollizer.Ragdollize();
                entityRagdollizer.Push((transform.position - offender.position), minDeathPushForce, maxDeathPushForce);

                Destroy(gameObject, timeToDestroyAfterDeath);
            }
        }
    }

    public void Heal(float amount)
    {
        currentLife += amount;
        lifeBar.SetNormalizedValue(Mathf.Clamp01(currentLife / maxLife));
    }

    private float GetMeatRecoveryByEnemy(Enemy.EnemyType enemyType)
    {
        switch (enemyType)
        {
            case Enemy.EnemyType.Ninja:
                return 5f;
            case Enemy.EnemyType.Archer:
                return 2f;
            case Enemy.EnemyType.Giant:
                return 10f;
            default:
                return 0f;
        }
    }
}