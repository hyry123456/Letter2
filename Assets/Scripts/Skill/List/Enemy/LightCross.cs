using UnityEngine;

namespace Skill
{
    /// <summary> /// ������Boss����ͨ����֮һ����Զ�̹��� /// </summary>
    public class LightCross : SkillBase
    {
        public LightCross()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 5f;
            releaseTime = sustainTime + waitTime;
            skillName = "Skill Name";
            skillType = SkillType.LongDisAttack;

            drawData = new DefferedRender.ParticleDrawData
            {
                beginSpeed = Vector3.up,
                speedMode = DefferedRender.SpeedMode.JustBeginSpeed,
                useGravity = false,
                followSpeed = false,
                radius = 3,
                radian = 6.28f,
                lifeTime = 1f,
                showTime = 1f,
                frequency = 1,
                octave = 4,
                intensity = 1,
                sizeRange = new Vector2(0.2f, 0.4f),
                colorIndex = DefferedRender.ColorIndexMode.HighlightToAlpha,
                sizeIndex = DefferedRender.SizeCurveMode.Small_Hight_Small,
                textureIndex = 0,
                groupCount = 5,
            };
        }

        Vector3 start, end;
        GameObject origin;
        /// <summary>    /// ��������   /// </summary>
        float attackDistance = 50;
        /// <summary>   /// �ƶ����յ���Ҫ��ʱ��  /// </summary>
        float sustainTime = 1, waitTime = 3;
        /// <summary>  /// ��ǰ�ۼƵ��ƶ�����    /// </summary>
        float nowRadio = -1;
        /// <summary>/// ʹ�ü���ʱ��������Ϸ����  /// </summary>
        Sphere_Pooling useObj;
        Vector3 beginScale;
        /// <summary>   /// ��һ�����ݣ��ж��Ƿ��Ѿ������  /// </summary>
        bool isAttack = false;

        DefferedRender.ParticleDrawData drawData;


        public override void OnSkillRelease(SkillManage mana)
        {
            if (origin == null)
                origin = Resources.Load<GameObject>("Prefab/Sphere_Pooling");

            Transform playerTrans = Control.PlayerControl.Instance?.transform;
            if (playerTrans == null || nowRadio > 0) return;
            Vector3 offset = (playerTrans.position - mana.transform.position).normalized;
            start = mana.transform.position + offset * 3;
            //�ƶ�����������
            end = mana.transform.position + offset * attackDistance;

            nowRadio = 0; isAttack = false;
            Common.SustainCoroutine.Instance.AddCoroutine(SustainMove);
        }

        private bool SustainMove()
        {
            nowRadio += Time.deltaTime;
            if (nowRadio > waitTime + sustainTime)      //��������ʱ��֮��
            {
                nowRadio = -1;
                return true;
            }
            else if(nowRadio < waitTime)
            {
                drawData.beginPos = start;
                DefferedRender.ParticleNoiseFactory.Instance.DrawShape(drawData);
                return false;
            }

            drawData.beginPos = Vector3.Lerp(start, end, (nowRadio - waitTime) / sustainTime);
            DefferedRender.ParticleNoiseFactory.Instance.DrawShape(drawData);
            Debug.DrawLine(start, drawData.beginPos);

            if (!isAttack)      //�������˺�
            {
                RaycastHit raycastHit;
                float distance = (drawData.beginPos - start).magnitude;
                if ( Physics.SphereCast(start, 3, drawData.beginPos - start, out raycastHit, distance))
                {
                    isAttack = true;
                    Info.CharacterInfo character = raycastHit.collider.gameObject.GetComponent<Info.CharacterInfo>();
                    if (character == null) return false;
                    character.modifyHp(-10);
                    Rigidbody rigidbody = character.GetComponent<Rigidbody>();
                    rigidbody.velocity += (drawData.beginPos - start).normalized * 30;
                }
            }
            return false;
        }
    }
}