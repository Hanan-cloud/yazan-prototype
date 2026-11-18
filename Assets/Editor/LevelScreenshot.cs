using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelScreenshot : EditorWindow
{
    private Camera targetCamera;
    private int width = 3840;
    private int height = 2160;

    [MenuItem("Tools/Capture Full Level")]
    public static void ShowWindow()
    {
        GetWindow<LevelScreenshot>("Level Capture");
    }

    private void OnGUI()
    {
        GUILayout.Label("Full Level Screenshot", EditorStyles.boldLabel);
        targetCamera = (Camera)EditorGUILayout.ObjectField("Level Camera", targetCamera, typeof(Camera), true);
        width = EditorGUILayout.IntField("Image Width (px)", width);
        height = EditorGUILayout.IntField("Image Height (px)", height);

        if (GUILayout.Button("Save PNG"))
        {
            if (targetCamera == null)
            {
                Debug.LogError("Please assign a Camera first!");
                return;
            }
            Capture();
        }
    }

    private void Capture()
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        targetCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

        targetCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string path = EditorUtility.SaveFilePanel("Save Level Image", "", "FullLevel.png", "png");

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, bytes);
            Debug.Log("Saved level screenshot to: " + path);
        }
    }
}
