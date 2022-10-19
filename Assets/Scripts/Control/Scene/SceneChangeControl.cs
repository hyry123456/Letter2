using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    //�����л�
    public class SceneChangeControl : MonoBehaviour
    {
        private static SceneChangeControl instance;
        public static SceneChangeControl Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject gameObject = new GameObject("SceneChange");
                    gameObject.AddComponent<SceneChangeControl>();
                }
                return instance;
            }
        }

        private string targetScene;
        AsyncOperation asyncStatic;

        bool isSceneChange = false;
        /// <summary>
        /// �����ж����л��������µ����¼��ػ��Ǳ��������¼��أ�ֻ�г�������ʱ�Ż���øú���
        /// </summary>
        public bool IsSceneChange => isSceneChange;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ChangeScene(string targetSceneName)
        {
            isSceneChange = true;
            targetScene = targetSceneName;
            SceneManager.LoadScene("ChangeScene");
            asyncStatic = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);
            asyncStatic.allowSceneActivation = false;
            //SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
            //StartCoroutine(AsynLoadScene());
        }

        IEnumerator AsynLoadScene()
        {
            while(asyncStatic.progress < 0.9f)
            {
                Debug.Log(asyncStatic.progress);
                yield return null;
            }
            yield return null;
            SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
        }

        public float GetLoadProgress()
        {
            return asyncStatic.progress;
        }

        /// <summary>        /// ������������еĳ���������        /// </summary>
        public string GetRuntimeSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        /// <summary>        /// ���¼��ص�ǰ���еĳ���        /// </summary>
        public void ReloadActiveScene()
        {
            isSceneChange = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GameExit()
        {
            Application.Quit();
        }
    }
}