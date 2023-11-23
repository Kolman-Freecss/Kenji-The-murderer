#region

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class LifeBar : MonoBehaviour
{
    [SerializeField] Image lifeBarImage;
    [SerializeField] private float lifeAnimationDuration = 0.25f;

    public void SetNormalizedValue(float newValue)
    {
        lifeBarImage.DOFillAmount(newValue, lifeAnimationDuration);
    }
}