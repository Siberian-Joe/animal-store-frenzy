using System;
using UnityEditor;
using UnityEngine;

namespace NewCore.Components
{
    [DisallowMultipleComponent]
    public class EntityIdentifier : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(Id))
                return;

            Id = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}