using UnityEngine;

/// <summary>/// �Ӵ���ͻ������ķ���/// </summary>
public class OnTigerDie : MonoBehaviour
{
    /// <summary>   
    /// ����������ʱִ�еķ���������ִ�н��������������������̽����ý���
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        //ֻ��������
        if (other.tag == "Player")
        {
            Control.SceneChangeControl.Instance.ReloadActiveScene();
        }
    }

}
