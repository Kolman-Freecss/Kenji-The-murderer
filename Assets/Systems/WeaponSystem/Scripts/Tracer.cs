#region

using DG.Tweening;
using UnityEngine;

#endregion

public class Tracer : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.2f;
    private LineRenderer lineRenderer;
    private Tween tween;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        DOTween.To(() => lineRenderer.widthMultiplier, x => lineRenderer.widthMultiplier = x, 0f, lifeTime);
        Destroy(gameObject, lifeTime);
    }

    public void Init(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3[] positions = { startPosition, endPosition };
        lineRenderer.SetPositions(positions);
    }
}