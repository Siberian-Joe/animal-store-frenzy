#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SelectImplementationAttribute))]
public class SelectImplementationDrawer : PropertyDrawer
{
    private readonly Dictionary<Type, List<Type>> _cachedTypes = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.LabelField(position, label.text, "Use SelectImplementation with SerializedReference.");

            return;
        }

        var baseType = GetBaseType(fieldInfo.FieldType);
        var implementationTypes = GetImplementationTypes(baseType);

        if (implementationTypes.Count == 0)
        {
            EditorGUI.LabelField(position, label.text, "No implementations available.");

            return;
        }

        var typeName = property.managedReferenceFullTypename.Split(' ').Last();
        var currentType = implementationTypes.FirstOrDefault(t => t.FullName == typeName);
        var typeLabel = currentType != null ? currentType.Name : "None";

        EditorGUI.BeginProperty(position, label, property);

        var foldoutRect = new Rect(position.x, position.y, position.width - 60, EditorGUIUtility.singleLineHeight);
        var buttonRect = new Rect(position.x + position.width - 60, position.y, 60, EditorGUIUtility.singleLineHeight);

        if (currentType != null)
        {
            property.isExpanded =
                EditorGUI.Foldout(foldoutRect, property.isExpanded, label.text + " (" + typeLabel + ")", true);
        }
        else
        {
            EditorGUI.LabelField(foldoutRect, label.text + " (" + typeLabel + ")");
            property.isExpanded = false;
        }

        if (GUI.Button(buttonRect, "Change"))
            ShowContextMenu(property, implementationTypes, currentType);

        if (property.isExpanded && property.managedReferenceValue != null)
        {
            var contentRect = new Rect(position.x,
                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, position.height);

            EditorGUI.indentLevel++;

            var prop = property.Copy();
            var endProperty = prop.GetEndProperty();

            prop.NextVisible(true);

            while (!SerializedProperty.EqualContents(prop, endProperty))
            {
                contentRect.height = EditorGUI.GetPropertyHeight(prop, true);
                EditorGUI.PropertyField(contentRect, prop, true);
                contentRect.y += contentRect.height + EditorGUIUtility.standardVerticalSpacing;
                prop.NextVisible(false);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded && property.managedReferenceValue != null)
        {
            var prop = property.Copy();
            var endProperty = prop.GetEndProperty();
            prop.NextVisible(true);
            while (!SerializedProperty.EqualContents(prop, endProperty))
            {
                height += EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing;
                prop.NextVisible(false);
            }
        }

        return height;
    }

    private Type GetBaseType(Type type)
    {
        return type.IsArray ? type.GetElementType() : type.GetGenericArguments().FirstOrDefault() ?? type;
    }

    private List<Type> GetImplementationTypes(Type baseType)
    {
        if (_cachedTypes.TryGetValue(baseType, out var types))
            return types;

        var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        _cachedTypes[baseType] = implementationTypes;

        return _cachedTypes[baseType];
    }

    private void ShowContextMenu(SerializedProperty property, List<Type> implementationTypes, Type currentType)
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("None"), currentType == null, () =>
        {
            property.serializedObject.Update();
            property.managedReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        });

        foreach (var type in implementationTypes)
        {
            menu.AddItem(new GUIContent(type.Name), type == currentType, () =>
            {
                var instance = Activator.CreateInstance(type);
                property.serializedObject.Update();
                property.managedReferenceValue = instance;
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }
}
#endif