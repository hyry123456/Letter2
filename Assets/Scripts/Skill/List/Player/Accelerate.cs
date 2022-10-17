using UnityEngine;

namespace Skill
{
    /// <summary>  /// ���ټ��ܣ�����ʵ����������Ч��  /// </summary>
    public class Accelerate : SkillBase
    {
        public Accelerate()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 5;
            skillName = "����";
            skillType = SkillType.Dodge;
        }

        const float sustainTime = 1;
        float nowTime = 0, minForece = 10, maxForece = 5;
        Rigidbody rb;
        Camera camera;
        /// <summary>    /// �ͷż��ټ���    /// </summary>
        public override void OnSkillRelease(SkillManage mana)
        {
            if (rb == null)
                rb = mana.GetComponent<Rigidbody>();
            camera = Camera.main;
            if (camera == null || rb == null) return;
            nowTime = 0;
            //���������ٵķ�����ջ�����м���
            Common.SustainCoroutine.Instance.AddCoroutine(SustainAccelate);
        }

        /// <summary>   /// �������ٵķ���������ʱ����fov   /// </summary>
        bool SustainAccelate()
        {
            nowTime += Time.deltaTime;
            if(nowTime < sustainTime)
            {
                float radio = 1.0f - Mathf.Abs(nowTime / sustainTime - 0.5f) / 0.5f;
                float trueForece = Mathf.Lerp(minForece, maxForece, radio);
                rb.AddForce(camera.transform.forward * trueForece, ForceMode.Force);
                camera.fieldOfView = Mathf.Lerp(60, 75, radio);
                return false;
            }
            return true;
        }
    }
}