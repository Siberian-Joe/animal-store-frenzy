#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AI.Editor
{
    [CustomPropertyDrawer(typeof(StatCollection))]
    public class StatCollectionDrawer : PropertyDrawer
    {
        private ReorderableList _reorderableList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (_reorderableList == null)
                InitializeReorderableList(property);

            _reorderableList?.DoList(position);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _reorderableList?.GetHeight() ?? EditorGUIUtility.singleLineHeight;
        }

        private void InitializeReorderableList(SerializedProperty property)
        {
            var statsProperty = property.FindPropertyRelative("_stats");

            _reorderableList = new ReorderableList(property.serializedObject, statsProperty, true, true, true, false)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, property.displayName),
                drawElementCallback = (rect, index, _, _) =>
                {
                    if (index >= statsProperty.arraySize)
                        return;

                    var element = statsProperty.GetArrayElementAtIndex(index);

                    if (element == null)
                        return;

                    var labelName = GetElementLabel(element);
                    var fieldRect = new Rect(rect.x, rect.y, rect.width - 25, EditorGUIUtility.singleLineHeight);
                    var removeButtonRect =
                        new Rect(rect.x + rect.width - 20, rect.y, 20, EditorGUIUtility.singleLineHeight);

                    EditorGUI.PropertyField(fieldRect, element, new GUIContent(labelName), true);

                    if (GUI.Button(removeButtonRect, "\u2715"))
                        RemoveElementAt(statsProperty, index);
                },
                elementHeightCallback = index =>
                {
                    if (index >= statsProperty.arraySize)
                        return EditorGUIUtility.singleLineHeight + 4;

                    var element = statsProperty.GetArrayElementAtIndex(index);

                    return element == null
                        ? EditorGUIUtility.singleLineHeight + 4
                        : EditorGUI.GetPropertyHeight(element) + 4;
                },
                onAddCallback = _ => ShowAddMenu(statsProperty)
            };
        }

        private void RemoveElementAt(SerializedProperty statsProperty, int index)
        {
            if (index < statsProperty.arraySize)
            {
                statsProperty.DeleteArrayElementAtIndex(index);

                if (index < statsProperty.arraySize &&
                    statsProperty.GetArrayElementAtIndex(index).managedReferenceValue == null)
                    statsProperty.DeleteArrayElementAtIndex(index);

                statsProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        private void ShowAddMenu(SerializedProperty statsProperty)
        {
            var menu = new GenericMenu();

            foreach (var type in GetAvailableStatTypes(statsProperty))
                menu.AddItem(new GUIContent(type.Name), false, () => AddStatOfType(statsProperty, type));

            menu.ShowAsContext();
        }

        private IEnumerable<Type> GetAvailableStatTypes(SerializedProperty statsProperty)
        {
            var existingTypes = new HashSet<Type>();

            for (var i = 0; i < statsProperty.arraySize; i++)
            {
                var element = statsProperty.GetArrayElementAtIndex(i);
                var type = element.managedReferenceValue?.GetType();

                if (type != null)
                    existingTypes.Add(type);
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ICharacterStat).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract &&
                               !existingTypes.Contains(type));
        }

        private void AddStatOfType(SerializedProperty statsProperty, Type type)
        {
            statsProperty.arraySize++;

            var element = statsProperty.GetArrayElementAtIndex(statsProperty.arraySize - 1);

            element.managedReferenceValue = Activator.CreateInstance(type);
            statsProperty.serializedObject.ApplyModifiedProperties();
        }

        private string GetElementLabel(SerializedProperty element)
        {
            var elementType = element.managedReferenceFullTypename;

            return string.IsNullOrEmpty(elementType) ? "Null" : elementType.Split(' ').Last().Split('.').Last();
        }
    }
}

#endif