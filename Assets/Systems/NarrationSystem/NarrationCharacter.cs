﻿#region

using UnityEngine;

#endregion

namespace Systems.NarrationSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Narration/Character")]
    public class NarrationCharacter : ScriptableObject
    {
        [SerializeField]
        private string m_CharacterName;

        public string CharacterName => m_CharacterName;
    }
}
