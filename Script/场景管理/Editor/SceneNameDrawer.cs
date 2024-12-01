#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            // ��ȡ Build Settings �е����г���
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            string[] sceneNames = new string[scenes.Length];

            for (int i = 0; i < scenes.Length; i++)
            {
                // ֻ��ȡ����������
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            }

            // ��ʾ�����˵���ѡ�񳡾�
            int index = Mathf.Max(0, System.Array.IndexOf(sceneNames, property.stringValue));
            index = EditorGUI.Popup(position, label.text, index, sceneNames);

            // ��������ֵ
            property.stringValue = sceneNames[index];
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use SceneName with string.");
        }
    }
}
#endif
