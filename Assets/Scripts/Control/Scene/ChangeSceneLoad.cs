using UnityEngine;

/// <summary>/// �����ı�ʱ�Ժ��������/// </summary>
public class ChangeSceneLoad : MonoBehaviour
{
    [SerializeField]
    DefferedRender.PostFXSetting fXSetting;
    /// <summary> /// ��Awake�����ã���֤����ִ�� /// </summary>
    private void Awake()
    {
        fXSetting.SetColorFilter(Color.black);
        fXSetting.SetMaxDepthFog(0.1f);
    }
}
