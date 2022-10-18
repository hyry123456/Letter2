using UnityEngine;
using Common;

namespace Audio
{
    public class AudioControl : MonoBehaviour
    {
        private static AudioControl instance;
        public static AudioControl Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject game = new GameObject("AudioControl");
                    game.AddComponent<AudioControl>();
                }
                return instance;
            }
        }

        AudioSource nowAudioSource;
        AudioSource preAudioSource;

        float changeTime = 3,       //背景音乐切换时需要的时间 
            volume = 1;             //音乐大小
        float nowRadio;
        AudioSource nowAudio;
        AudioSource preAudio;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            nowAudioSource = gameObject.AddComponent<AudioSource>();
            preAudioSource = gameObject.AddComponent<AudioSource>();
            nowAudioSource.loop = preAudioSource.loop = true;
        }

        public void ChangeBackgroundAduio(AudioClip audio)
        {
            if (nowAudio.clip == null)
            {
                preAudio.clip = null;
                preAudio.Stop();
            }
            else
            {
                preAudio.clip = nowAudio.clip;
                preAudio.volume = volume;
                preAudio.Play();
            }
            nowAudio.clip = audio;
            nowAudio.volume = 0;
            nowAudio.Play();
            SustainCoroutine.Instance.AddCoroutine(ChangeAduio);
        }

        /// <summary>     /// 动态更新背景音乐    /// </summary>
        bool ChangeAduio()
        {
            nowRadio += Time.deltaTime * (1.0f / changeTime);
            if(nowRadio >= 1.0f)
            {
                nowAudio.volume = volume;   //完全开启
                if (preAudio.clip != null)
                {
                    preAudio.Stop();            //停止播放
                    preAudio.volume = 0;        //关闭之前
                }
                return true;
            }
            nowAudio.volume = Mathf.Lerp(0, volume, nowRadio);
            if (preAudio.clip != null)
                preAudio.volume = Mathf.Lerp(volume, 0, nowRadio);
            return false;
        }


    }
}