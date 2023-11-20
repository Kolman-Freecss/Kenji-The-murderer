#region

using UnityEngine;

#endregion

namespace Entity.Scripts
{
    public interface IEntityAnimable
    {
        public Vector3 GetLastVelocity();

        public float GetVerticalVelocity();

        public float GetJumpSpeed();

        public bool IsGrounded();
    }
}