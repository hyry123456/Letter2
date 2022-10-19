using UnityEngine;

/// <summary>/// 场景改变时对后处理的设置/// </summary>
public class ChangeSceneLoad : MonoBehaviour
{
    [SerializeField]
    DefferedRender.PostFXSetting fXSetting;
    /// <summary> /// 在Awake中设置，保证立刻执行 /// </summary>
    private void Awake()
    {
        fXSetting.SetColorFilter(Color.black);
        fXSetting.SetMaxDepthFog(0.1f);
    }
}
