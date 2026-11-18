using UnityEngine;
using UnityEditor;
using System.IO;

public class SkyBoundsScreenshot : EditorWindow
{
    private SpriteRenderer skySprite;
    private Camera sourceCamera;
    private int pixelsPerUnit = 100;

    [MenuItem("Tools/Capture Level By Sky Bounds")]
    public static void ShowWindow()
    {
        GetWindow<SkyBoundsScreenshot>("Sky Level Capture");
    }

    private void OnGUI()
    {
        GUILayout.Label("Capture Level By Sky Bounds", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        skySprite = (SpriteRenderer)EditorGUILayout.ObjectField("Sky Sprite Renderer", skySprite, typeof(SpriteRenderer), true);
        sourceCamera = (Camera)EditorGUILayout.ObjectField("Source Camera (Optional)", sourceCamera, typeof(Camera), true);
        pixelsPerUnit = EditorGUILayout.IntSlider("Pixels Per Unit", pixelsPerUnit, 10, 300);

        if (skySprite != null)
        {
            Bounds b = skySprite.bounds;
            int previewW = Mathf.RoundToInt(b.size.x * pixelsPerUnit);
            int previewH = Mathf.RoundToInt(b.size.y * pixelsPerUnit);
            EditorGUILayout.HelpBox($"Output Resolution: {previewW} x {previewH} px", MessageType.Info);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Capture & Save PNG", GUILayout.Height(35)))
        {
            if (skySprite == null)
            {
                EditorUtility.DisplayDialog("Warning", "Please assign the Sky SpriteRenderer first.", "OK");
                return;
            }
            CaptureFromSkyBounds();
        }
    }

    private void CaptureFromSkyBounds()
    {
        Bounds bounds = skySprite.bounds;

        int width = Mathf.Clamp(Mathf.RoundToInt(bounds.size.x * pixelsPerUnit), 64, 16384);
        int height = Mathf.Clamp(Mathf.RoundToInt(bounds.size.y * pixelsPerUnit), 64, 16384);

        GameObject tempCamObj = new GameObject("TempSkyCaptureCamera");
        Camera cam = tempCamObj.AddComponent<Camera>();

        if (sourceCamera != null)
        {
            cam.CopyFrom(sourceCamera);
        }
        else if (Camera.main != null)
        {
            cam.CopyFrom(Camera.main);
        }

        cam.orthographic = true;
        cam.orthographicSize = bounds.extents.y;
        cam.aspect = bounds.size.x / bounds.size.y;
        cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
        cam.transform.rotation = Quaternion.identity;

        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        DestroyImmediate(tempCamObj);

        byte[] bytes = screenShot.EncodeToPNG();
        DestroyImmediate(screenShot);

        string path = EditorUtility.SaveFilePanel("Save Level Image", "", "LevelMap.png", "png");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, bytes);
            Debug.Log($"Saved level screenshot to: {path}");
        }
    }
}
