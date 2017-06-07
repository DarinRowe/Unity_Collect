using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace RCGameFrame
{
    public static class RcSoundManager
    {
        public static AudioClip[] m_AudioClip;

        private static AudioSource m_Music;

        private static bool m_InitAssetBundle = false;

        private static Dictionary<int, Vector3> m_SoundInfo = new Dictionary<int, Vector3>();

        public static void Init()
        {
            m_AudioClip = new AudioClip[SoundDefines.m_SoundPath.Length];
            for (int i = 0; i < SoundDefines.m_SoundPath.Length; i++){
                m_AudioClip[i] = Resources.Load(SoundDefines.m_SoundPath[i],typeof(AudioClip)) as AudioClip;
            }
            m_Music = null;
        }
        public static void InitForAssetBundle(string bundleName,int Version)
        {
            if (!m_InitAssetBundle)
            {
                m_InitAssetBundle = true;
                //AssetBundle bundle = AssetBundleManger.GetAssetBundle(bundleName, Version);
                for(int i=0; i < SoundDefines.m_SoundPath.Length; i++)
                {
                    if (SoundDefines.m_SoundBundleData[i])
                    {
                        string name = SoundDefines.m_SoundPath[i];
                        //去掉路径前缀，只有文件名
                        name = name.Replace("Sounds/", string.Empty);
                        //m_AudioClip[i] = bundle.Load(name, typeof(AudioClip)) as AudioClip;
                    }
                }
            }
        }
        public static AudioSource GetMusic()
        {
            return m_Music;
        }
        public static void InitMusic()
        {
           
        }
        public static void SetMuteMusic(bool isMuteMusic)
        {
            m_Music.mute = isMuteMusic;
        }
        public static bool PlayMusic(int sndID,bool loop=true)
        {
            if (sndID < 0 || sndID >= m_AudioClip.Length)
                return false;
            //if (SoundDefines.MusicSwith==false)
                //return false;
            if (m_Music.isPlaying)
                m_Music.Stop();
            m_Music.clip = m_AudioClip[sndID];
            m_Music.loop = loop;
            m_Music.Play();
            RefreshMusicVolume();
            return true;
        }
        public static void StopMusic()
        {
            m_Music.Stop();
        }
        public static void RefreshMusicVolume(float change = 1f)
        {
            //SetMusicVolume(SoundDefines.MusicVolume);
        }
        public static void SetMusicVolume(float vol)
        {
            m_Music.volume = vol;
        }
        public static bool PlaySFX(int SFXID,GameObject SFXObj, bool loop = true)
        {
            //if (SoundDefines.SFXSwith == false)
                //return false;
            if (SFXID < 0 || SFXID >= m_AudioClip.Length)
                return false;
            if (!m_SoundInfo.ContainsKey(SFXID))
                m_SoundInfo.Add(SFXID, SFXObj.transform.position);
            if (m_Music != null)
                RefreshMusicVolume(0.1f);
            return true;
        }
        public static void ResetSound()
        {
            m_SoundInfo.Clear();
        }
        public static void Update(float delta)
        {
            foreach(KeyValuePair<int,Vector3>pair in m_SoundInfo)
            {
                if (m_AudioClip[pair.Key] != null)
                {
                    AudioSource.PlayClipAtPoint(m_AudioClip[pair.Key], pair.Value);
                }
                else
                {
                    Debugger.LogError("Not found Clip");
                }
            }
            ResetSound();
        }
    }
}
