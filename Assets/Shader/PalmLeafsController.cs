using UnityEngine;

public class PalmLeafsController : MonoBehaviour
{
    [SerializeField] float windSpeed = 2f;
    [SerializeField] float WindStrength = 0.5f;
    [SerializeField] float windInfuluenceMask = 5f;


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

        propBlock.SetFloat("_InfluenceSection", windInfuluenceMask);
        propBlock.SetFloat("_WindStrength", WindStrength);
        propBlock.SetFloat("_WindSpeed", windSpeed);

        renderer.SetPropertyBlock(propBlock);

    }
}
