#region

using UnityEngine;

#endregion

namespace Gameplay.Events
{
    /// <summary>
    /// Animation event forwarder.
    /// @author: Kolman-Freecss
    /// </summary>
    public class AnimationEvent : MonoBehaviour
    {
        public delegate void AnimationEventDelegate<T>(T handleAnimationId);

        public event AnimationEventDelegate<int> OnAnimationFinish;

        /// <summary>
        /// Called from Animation Event.
        /// </summary>
        public void AnimationFinished(int handleAnimationId)
        {
            OnAnimationFinish?.Invoke(handleAnimationId);
        }
    }
}