using UnityEngine;

namespace Common
{
    /// <summary>    
    /// �����࣬��������ȫ���ļ��ط�����Ϊ�˱�֤���أ�ÿһ���ೡ������һ����
    /// �����ֻ�ǵ����࣬�����Ƿ��ظ������ǿ�������ʵ�ֵ�
    /// </summary>
    public class GameLoad : MonoBehaviour
    {
        private static GameLoad instance;
        public static GameLoad Instance => instance;
        private string sceneName;
        public string SceneName => sceneName;
        [SerializeField]
        AudioClip sceneBackgroundAudio;

        private void Awake()
        {
            instance = this;
            sceneName = Control.SceneChangeControl.Instance.GetRuntimeSceneName();

            SustainCoroutine sustain = SustainCoroutine.Instance; //����Э��
            Task.AsynTaskControl.Instance.ReLoadTask();
            Application.targetFrameRate = -1;
            SceneObjectMap objectMap = SceneObjectMap.Instance;
            if (sceneBackgroundAudio == null)
                return;
            if(sceneBackgroundAudio != null &&
                !Audio.AudioControl.Instance.CheckAudioIsPlaying(sceneBackgroundAudio))
            {
                Audio.AudioControl.Instance.ChangeBackgroundAduio(sceneBackgroundAudio);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}