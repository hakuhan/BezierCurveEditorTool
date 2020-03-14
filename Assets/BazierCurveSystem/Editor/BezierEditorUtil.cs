using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BezierCurve
{
    public class BezierEditorUtil : Editor
    {
        public static List<UnityEngine.Object> GetObject(Type typeComponent, string meg = null)
        {
            List<UnityEngine.Object> lsResults = new List<UnityEngine.Object>();

            Event aEvent;
            aEvent = Event.current;

            GUI.contentColor = Color.white;

            var dragArea = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));

            GUIContent title = new GUIContent(meg);
            if (string.IsNullOrEmpty(meg))
            {
                title = new GUIContent("Drag Object here from Project view to get the object");
            }

            GUI.Box(dragArea, title);
            switch (aEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dragArea.Contains(aEvent.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (aEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i)
                        {
                            var temp = DragAndDrop.objectReferences[i];

                            if (temp == null
                                && temp.GetType() != typeof(GameObject)
                                && (temp as GameObject).GetComponent(typeComponent) == null)
                            {
                                return null;
                            }
                            lsResults.Add(temp);
                        }
                    }

                    Event.current.Use();
                    break;
                default:
                    break;
            }

            return lsResults;
        }

        /// <summary>
        /// 选择物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T PickObj<T>(T scrCurrent, string info) where T : Component
        {
            T effectGO = null;

            if (GUILayout.Button(info))
            {
                //create a window picker control ID
                var currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive);

                //use the ID you just created
                EditorGUIUtility.ShowObjectPicker<GameObject>(scrCurrent == null ? null : scrCurrent.gameObject, true, "", currentPickerWindow);
            }

            if (Event.current.commandName == "ObjectSelectorClosed")
            {
                var objSelected = EditorGUIUtility.GetObjectPickerObject();
                if (objSelected != null)
                {
                    effectGO = (objSelected as GameObject).GetComponent<T>();
                    // Debug.Log("object selected");
                }
            }

            return effectGO;
        }

    }
}