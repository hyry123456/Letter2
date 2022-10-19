using UnityEngine;

public class SetPostBegin : MonoBehaviour
{
    [SerializeField]
    DefferedRender.PostFXSetting fXSetting;
    void Start()
    {
        fXSetting.SetMaxDepthFog(0.1f);
        fXSetting.SetColorFilter(Color.white);
    }

}
