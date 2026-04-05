using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Game.World.Core
{
    [DisallowMultipleComponent]
    public sealed class EntityIdentifier : MonoBehaviour
    {
        [SerializeField] private string _id;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public bool HasId => string.IsNullOrWhiteSpace(_id) == false;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            if (HasId)
                return;

            if (ShouldSkipAutoGeneration())
                return;

            GenerateNewIdInternal();
        }

        [ContextMenu("Generate New Id")]
        private void GenerateNewIdFromContextMenu()
        {
            GenerateNewIdInternal();
        }

        private bool ShouldSkipAutoGeneration()
        {
            if (EditorUtility.IsPersistent(this))
                return true;

            if (PrefabStageUtility.GetPrefabStage(gameObject))
                return true;

            return gameObject.scene.IsValid() == false || string.IsNullOrWhiteSpace(gameObject.scene.path);
        }

        private void GenerateNewIdInternal()
        {
            _id = Guid.NewGuid().ToString("N");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}