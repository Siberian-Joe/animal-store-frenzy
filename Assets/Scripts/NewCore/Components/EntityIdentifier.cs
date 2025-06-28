using System;
using UnityEditor;
using UnityEngine;

namespace NewCore.Components
{
    [DisallowMultipleComponent]
    public class EntityIdentifier : MonoBehaviour
    {
        [field: SerializeField] public string ID { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(ID))
                return;

            ID = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}