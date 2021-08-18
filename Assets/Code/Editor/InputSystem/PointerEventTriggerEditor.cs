using System;
using UnityEditor;
using UnityEditor.EventSystems;
using UnityEngine;
using VIRA.InputSystem;

namespace VIRA.EditorUtils.InputSystem
{
    [CustomEditor(typeof(PointerEventTrigger), true)]
    public class PointerEventTriggerEditor : Editor
    {
        // Fields
        private SerializedProperty m_DelegatesProperty;
        private GUIContent m_IconToolbarMinus;
        private GUIContent m_EventIDName;
        private GUIContent[] m_EventTypes;
        private GUIContent m_AddButonContent;

        // Methods
        private void OnAddNewSelected(object index)
        {
            m_DelegatesProperty.arraySize++;
            m_DelegatesProperty.GetArrayElementAtIndex(m_DelegatesProperty.arraySize - 1).FindPropertyRelative("eventID").enumValueIndex = (int)index;
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnEnable()
        {
            this.m_DelegatesProperty = serializedObject.FindProperty("m_Delegates");
            this.m_AddButonContent = EditorGUIUtility.TrTextContent("Add New Event Type", null, (Texture)null);
            this.m_EventIDName = new GUIContent("");
            this.m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            this.m_IconToolbarMinus.tooltip = "Remove all events in this list.";
            string[] names = Enum.GetNames(typeof(PointerEventTriggerType));
            this.m_EventTypes = new GUIContent[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                this.m_EventTypes[i] = new GUIContent(names[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            int toBeRemovedEntry = -1;
            EditorGUILayout.Space();
            Vector2 vector = GUIStyle.none.CalcSize(this.m_IconToolbarMinus);
            int num2 = 0;
            while (true)
            {
                if (num2 >= this.m_DelegatesProperty.arraySize)
                {
                    if (toBeRemovedEntry > -1)
                    {
                        this.RemoveEntry(toBeRemovedEntry);
                    }
                    Rect rect = GUILayoutUtility.GetRect(this.m_AddButonContent, GUI.skin.button);
                    rect.x += (rect.width - 200f) / 2f;
                    rect.width = 200f;
                    if (GUI.Button(rect, this.m_AddButonContent))
                    {
                        this.ShowAddTriggermenu();
                    }
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
                SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(num2);
                SerializedProperty property2 = arrayElementAtIndex.FindPropertyRelative("eventID");
                SerializedProperty property3 = arrayElementAtIndex.FindPropertyRelative("callback");
                this.m_EventIDName.text = property2.enumDisplayNames[property2.enumValueIndex];
                EditorGUILayout.PropertyField(property3, this.m_EventIDName, Array.Empty<GUILayoutOption>());
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect position = new Rect((lastRect.xMax - vector.x) - 8f, lastRect.y + 1f, vector.x, vector.y);
                if (GUI.Button(position, this.m_IconToolbarMinus, GUIStyle.none))
                {
                    toBeRemovedEntry = num2;
                }
                EditorGUILayout.Space();
                num2++;
            }
        }

        private void RemoveEntry(int toBeRemovedEntry)
        {
            this.m_DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        private void ShowAddTriggermenu()
        {
            GenericMenu menu = new GenericMenu();
            int index = 0;
            while (true)
            {
                if (index >= this.m_EventTypes.Length)
                {
                    menu.ShowAsContext();
                    Event.current.Use();
                    return;
                }
                bool flag = true;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= this.m_DelegatesProperty.arraySize)
                    {
                        if (flag)
                        {
                            menu.AddItem(this.m_EventTypes[index], false, new GenericMenu.MenuFunction2(OnAddNewSelected), index);
                        }
                        else
                        {
                            menu.AddDisabledItem(this.m_EventTypes[index]);
                        }
                        index++;
                        break;
                    }
                    SerializedProperty property2 = this.m_DelegatesProperty.GetArrayElementAtIndex(num2).FindPropertyRelative("eventID");
                    if (property2.enumValueIndex == index)
                    {
                        flag = false;
                    }
                    num2++;
                }
            }
        }
    }
}