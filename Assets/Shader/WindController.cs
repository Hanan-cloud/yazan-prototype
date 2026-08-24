using UnityEngine;

public class WindController : MonoBehaviour
{
    public float windInfuluenceMask = 1.0f;
    public float windStrength = 1.0f;
    public float windSpeed = 1.0f;


    void Start()
    {
        SetShaderParameter();
    }


    [ContextMenu("Set Shader")]
    void SetShaderParameter()
    {
        Renderer renderer = GetComponent<Renderer>();
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(propBlock);

        propBlock.SetFloat("_Wind_influence_mask", windInfuluenceMask);
        propBlock.SetFloat("_Wind_Strength", windStrength);
        propBlock.SetFloat("_WindSpeed", windSpeed);

        renderer.SetPropertyBlock(propBlock);
    }

}


