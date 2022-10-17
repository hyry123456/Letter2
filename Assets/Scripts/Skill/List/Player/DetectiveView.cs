using UnityEngine;

namespace Skill
{
    /// <summary>
    /// ��̽��Ұ���ܣ�����������֮��
    /// </summary>
    public class DetectiveView : SkillBase
    {
        DefferedRender.PostFXSetting fXSetting;
        public DetectiveView()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 0;
            skillName = "��̽��Ұ";
            skillType = SkillType.Auxiliary;
        }
        //һ��ʼӦ������ǿ��Ч������ӥ�ۣ����Ĭ�ϸ�ֵΪfalse
        bool isUpFog = false;
        
        float nowRadio, //��ǰ����ֵ
            changeTime = 2;    //�仯��Ҫ��ʱ��

        public override void OnSkillRelease(SkillManage mana)
        {
            if(fXSetting == null)
                fXSetting = Resources.Load<DefferedRender.PostFXSetting>("Render/PostFX/PostFX");
            isUpFog = !isUpFog;
            nowRadio = 0;
            if (isUpFog)
            {
                fXSetting.BeginDetective();
                Common.SustainCoroutine.Instance.AddCoroutine(UpFog);
            }
            else
            {
                Common.SustainCoroutine.Instance.AddCoroutine(DecreaseFog);
            }
        }

        //��ǿ��Ч������ӥ��
        bool UpFog()
        {
            //����л�Ϊ������Ч���˳�����Ϊ
            if (!isUpFog)
                return true;
            nowRadio += Time.deltaTime * (1.0f / changeTime);
            if(nowRadio >= 1.0f)
            {
                nowRadio = 1.0f;
                fXSetting.SetMaxDepthFog(0);
                return true;
            }
            fXSetting.SetMaxDepthFog(Mathf.Lerp(0.1f, 0, nowRadio));
            return false;
        }

        //������Ч
        bool DecreaseFog()
        {
            //����л�Ϊ������Ч���˳�����Ϊ
            if(isUpFog) return true;
            nowRadio += Time.deltaTime * (1.0f / changeTime);
            if (nowRadio >= 1.0f)
            {
                nowRadio = 1.0f;
                fXSetting.SetMaxDepthFog(0.1f);
                fXSetting.CloseDetective();
                return true;
            }
            fXSetting.SetMaxDepthFog(Mathf.Lerp(0f, 0.1f, nowRadio));
            return false;
        }
    }
}