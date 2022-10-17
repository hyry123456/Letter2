using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Motor
{
    public class EnemyMotor : MonoBehaviour
    {

        /// <summary>    /// ��ǰ�ٶ�, �����ٶ�, ����������ٶ�    /// </summary>
        Vector3 velocity, desiredVelocity, connectionVelocity;
        /// <summary>    /// ���ٶ�    /// </summary>
        public float groundAcceleration = 10f, airAcceleration = 5;

        Rigidbody body;
        GameObject connectObj, preConnectObj;

        /// <summary>    /// �Ƿ��ڵ�����    /// </summary>
        private bool onGround = false;

        /// <summary>        /// ���������б�нǣ��Լ�¥����б�н�        /// </summary>
        [Range(0, 90)]
        public float maxGroundAngle = 25f, maxStairAngle = 25;
        private float minGroundDot = 0, minStairsDot = 0;

        /// <summary>
        /// �Ӵ���ķ��ߣ����������ƽ�����ߣ�����ȷ���ƶ���ķ���
        /// </summary>
        Vector3 contactNormal;

        /// <summary>    /// �ж�ʱ��������ص��ٶȣ�����ٶȴ��ڸ�ֵ������������    /// </summary>
        [SerializeField, Range(0, 100f)]
        float maxSnapSpeed = 100f;
        /// <summary>    /// ���صļ�����    /// </summary>
        [SerializeField, Range(0, 10f)]
        float probeDistance = 3f;
        /// <summary>     /// ���ؼ�����    /// </summary>
        int stepSinceLastGround;

        /// <summary>    /// ���ؼ��Ĳ㣬�Լ�¥�ݼ���    /// </summary>
        [SerializeField]
        LayerMask probeMask, stairsMask = -1;

        Info.CharacterInfo characterInfo;
        Vector3 connectionWorldPostion;


        /// <summary>
        /// ��������ƫ����ֵ��x��ǰ�����ţ�y������ƫ��,z����ӵ����Ĵ�С
        /// </summary>
        [SerializeField]
        Vector3 climbData = Vector3.one;

        void Awake()
        {
            velocity = Vector3.zero;
            body = GetComponent<Rigidbody>();
            minGroundDot = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
            minStairsDot = Mathf.Cos(maxStairAngle * Mathf.Deg2Rad);
            characterInfo = GetComponent<Info.CharacterInfo>();
            if (characterInfo == null) Debug.LogError("��ɫ��ϢΪ��");
        }

        public void Move(float x, float y)
        {
            //ʵ���Ͼ��Ǽ��ٶ��Ƕ�ֵ����ɫ����ֵ��Ŀ���ٶȣ���֡���ٶȱ仯ΪĿ���ٶ�
            Vector2 playInput = new Vector2(x, y);
            playInput = Vector2.ClampMagnitude(playInput, 1);
            desiredVelocity = Vector3.forward * playInput.x + Vector3.right * playInput.y;

            desiredVelocity = desiredVelocity * characterInfo.runSpeed;
        }

        private void FixedUpdate()
        {
            //�������ݣ���������һ������֡�����ݽ��и���֮���
            UpdateState();

            //ȷ���ڿ��л����ڵ���
            AdjustVelocity();
            Rotate();

            velocity = Vector3.ClampMagnitude(velocity, 40);

            body.velocity = velocity;
            ClearState();
        }

        void UpdateState()
        {
            velocity = body.velocity;
            //�����ڵ���ʱִ���������淽��
            if (onGround/*�ڵ���*/ || SnapToGround()/*�����������棬Ҳ���Ǹո�δ������Ծ�����Ƿ��˳�ȥ*/)
            {

                contactNormal.Normalize();
                LoadTargetY(desiredVelocity);
            }
            else
                contactNormal = Vector3.up;

            if (connectObj && connectObj.tag == "CheckMove")
            {
                UpdateConnectionState();
            }
        }

        void UpdateConnectionState()
        {
            //ֻ��������ͬ�����б�Ҫ����
            if (connectObj == preConnectObj)
            {
                Vector3 connectionMovment =
                    connectObj.transform.position - connectionWorldPostion;
                connectionVelocity = connectionMovment / Time.deltaTime;
            }
            connectionWorldPostion = connectObj.transform.position;
        }

        private void OnCollisionExit(Collision collision)
        {
            EvaluateCollision(collision);
        }


        private void OnCollisionStay(Collision collision)
        {
            EvaluateCollision(collision);
        }

        void EvaluateCollision(Collision collision)
        {
            float minDot = GetMinDot(collision.gameObject.layer);
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
                float upDot = Vector3.Dot(Vector3.up, normal);
                if (upDot >= minDot)
                {
                    onGround = true;
                    //��֤����ж���Ӵ���ʱ�ܹ���ȷ�Ļ�ȡ����
                    contactNormal += normal;
                    connectObj = collision.gameObject;
                }
                //�������ƶ����ƣ����Ǳ��⳹�׵Ĵ�ֱ��
                else if (upDot > -0.01f)
                {
                    connectObj = collision.gameObject;
                }

            }
        }


        /// <summary>
        /// �����ƶ�����������֤�ƶ��ķ���������ƽ���
        /// </summary>
        void AdjustVelocity()
        {
            //��Ϊ�ٶ��õ�Ҳ���������꣬����ƶ�ʱͶӰҲ���������������꣬����right����x�ᣬfoward����Y��
            //��1��0��0ͶӰ���Ӵ�ƽ���ϣ�
            Vector3 xAixs = ProjectDirectionOnPlane(Vector3.right, contactNormal);
            //��0��0��1ͶӰ���Ӵ�ƽ����
            Vector3 zAxis = ProjectDirectionOnPlane(Vector3.forward, contactNormal);

            Vector3 relativeVelocity = velocity - connectionVelocity;
            //ȷ��ʵ���������ƽ���ϵ�X�ƶ�ֵ
            float currentX = Vector3.Dot(relativeVelocity, xAixs);
            //ȷ��ʵ���������ƽ���ϵ�Z�ƶ�ֵ
            float currentZ = Vector3.Dot(relativeVelocity, zAxis);

            float acceleration = onGround ? groundAcceleration : airAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;

            //ȷ�����������õ����ƶ�ֵ
            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

            //�ƶ�Ҫ�������ƽ��ķ������ƶ�����˸���ʵ��ֵ������ֵ�Ĳ�ȷ��Ҫ���ӵ��ٶȴ�С��
            //Ȼ�����ͶӰ���������Xֵ�Լ�Zֵȷ�������ƶ�ֵ
            velocity += xAixs * (newX - currentX) + zAxis * (newZ - currentZ);
        }

        /// <summary>
        /// ������ݣ���һЩ���ݹ�Ϊ��ʼ��
        /// </summary>
        void ClearState()
        {
            onGround = false;
            contactNormal = connectionVelocity = Vector3.zero;
            preConnectObj = connectObj;
            connectObj = null;
        }

        /// <summary>
        /// �������������õķ����������ƶ�ʱ��ɳ�ȥ��Ч��
        /// </summary>
        /// <returns>�������һЩ������ʹ�ã�����з���ֵ</returns>
        bool SnapToGround()
        {
            //������Ϊֻ����һ��
            if (stepSinceLastGround > 1)
            {
                return false;
            }
            float speed = velocity.magnitude;
            //��������ٶȣ�����������
            if (speed > maxSnapSpeed)
                return false;

            RaycastHit hit;
            if (!Physics.Raycast(body.position, -Vector3.up, out hit, probeDistance, probeMask))
                return false;

            float upDot = Vector3.Dot(Vector3.up, hit.normal);
            //������е��治����Ϊ����վ�����棬�Ͳ���������
            if (upDot < GetMinDot(hit.collider.gameObject.layer))
                return false;

            contactNormal = hit.normal;

            //ȷ���ٶ��ڷ����ϵĴ�С
            float dot = Vector3.Dot(velocity, hit.normal);
            //��ֻ֤���ٶȳ���ʱ�Ż�����ѹ��������������ٶ�
            if (dot > 0)
            {
                //�����ٶȵĴ�С��ƽ����ѹ
                velocity = (velocity - hit.normal * dot).normalized * speed;
            }
            connectObj = hit.collider.gameObject;
            return true;
        }

        float GetMinDot(int layer)
        {
            //�ж���¥�ݻ�����������
            return (stairsMask & (1 << layer)) == 0 ?
                minGroundDot : minStairsDot;
        }


        /// <summary>    /// ȷ���÷���ͶӰ����ƽ���ϵķ���ֵ�����й���׼����    /// </summary>
        Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }

        float targetRotateY;
        /// <summary>       /// ��תģ��        /// </summary>
        void Rotate()
        {
            Vector3 angle = transform.eulerAngles;
            //�ƶ��Ƕ�
            angle.y = Mathf.MoveTowardsAngle(angle.y, targetRotateY, characterInfo.rotateSpeed);

            transform.eulerAngles = angle;
        }

        /// <summary>
        /// ������תĿ��Yֵ��ֻ���ڵ���ʱ���ߵ�ǿʱ�Ż���ת
        /// </summary>
        /// <param name="desire">�����ƶ������緽��</param>
        void LoadTargetY(Vector3 desire)
        {
            Vector2 vector2 = new Vector2(desire.x, desire.z);
            //̫С�Ͳ�������������ת��������
            if (Mathf.Abs(vector2.y) < 0.0001) return;
            targetRotateY = GetAngle(vector2.normalized);
        }

        /// <summary>
        /// �����ƶ��Ĳ��ֵ�ж���ת�Ƕȣ�ע�⴫��ֵҪ��׼����
        /// ����Ϊ��̬��Ϊ�����������Ҫ�õ��������ݣ����ֻ�ÿ���һ��������͹���
        /// </summary>
        static float GetAngle(Vector2 direction)
        {
            //ͨ�������Һ����������ת������ƶ���������Ҫ��yֵ�Ƕ�
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            //�ж����ıߣ�Ҳ����˳ʱ�뻹����ʱ��
            return direction.x < 0f ? 360f - angle : angle;
        }



    }
}