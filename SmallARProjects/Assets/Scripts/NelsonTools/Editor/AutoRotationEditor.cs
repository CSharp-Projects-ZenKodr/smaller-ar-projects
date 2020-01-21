using UnityEngine;
using UnityEditor;
using System;

namespace NelsonTools {
    [CustomEditor(typeof(AutoRotation))]
    public class AutoRotationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var autoRotation = target as AutoRotation;

            autoRotation.sepateSpeeds = EditorGUILayout.Toggle("Hide Fields", autoRotation.sepateSpeeds);

            using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(autoRotation.sepateSpeeds)))
            {
                if (group.visible == false)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PrefixLabel("Color");
                    autoRotation.xSpeed = EditorGUILayout.FloatField(autoRotation.xSpeed);
                }
            }
        }
    }
}