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
        public delegate void AnimationEventDelegate();

        public event AnimationEventDelegate OnAnimationFinish;

        /// <summary>
        /// Called from Animation Event.
        /// </summary>
        public void AnimationFinished()
        {
            OnAnimationFinish?.Invoke();
        }
    }
}