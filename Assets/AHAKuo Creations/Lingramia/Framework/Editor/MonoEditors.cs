using UnityEngine;
using UnityEditor;
using System;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework.Editors
{
    [CustomEditor(typeof(LingramiaConfigAsset)), CanEditMultipleObjects]
    public class SignaliaSettings : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            //GUILayout.Label(GraphicLoader.SignaliaSettings, GUILayout.Height(150));

            EditorGUILayout.HelpBox("This is the Signalia's Lingramia Config Asset. Click below to edit.", MessageType.Info);

            GUILayout.Space(10);

            if (GUILayout.Button("Edit Settings", GUILayout.Height(30)))
            {
                FrameworkSettings.ShowWindow();
            }
        }
    }
}
