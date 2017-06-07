using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace RCGameFrame
{
    public class SoundManager : DDOLSingleton<SoundManager>
    {
        ////public static float lowPitchRange = 0.95f;             //The lowest a sound effect will be randomly pitched.                     随机最慢播放速度上限
        ////public static float highPitchRange = 1.05f;
        //public float sfxVolume;                         //The sfx volume                                                          音效音量
        //public float musicVolume;                       //The music volume                                                        音乐音量
        private AudioListener listener;					//Add audioListener                                                       Audio监听器
                                                                                        //Music Clips 
        private List<AudioSource>[] SFXPlayer = new List<AudioSource>[SFX.COUNT];	        //SFX Player                          音效播放器
        private List<AudioSource>[] MUSICPlayer = new List<AudioSource>[MUSIC.COUNT];       //MUSIC Player                        音效播放器


        public static AudioClip[] m_AudioClipList;    //Clip合集       
        private static AudioSource m_SinglePlay;      //单个播放器
        private static AudioSource[] m_ClipListPlay;  //多个播放器
        private AudioClip[] RSFXClips = new AudioClip[SoundDefines.SFXCount];                                                                                          //SFX Clips 
        private AudioClip[] RMusicClips = new AudioClip[SoundDefines.MusicCount];
        private static Dictionary<String , int> m_SoundInfo = new Dictionary<String, int>();   


        void Awake()
        {
            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);
            listener = gameObject.AddComponent<AudioListener>();

            //实例化m_AudioClipList和播放器AudioSource,加载clip到AudioClip中
            m_AudioClipList = new AudioClip[SoundDefines.m_SoundPath.Length];
            m_ClipListPlay = new AudioSource[SoundDefines.m_SoundPath.Length];
            for (int i = 0; i < SoundDefines.m_SoundPath.Length; i++)
            {
                m_AudioClipList[i] = Resources.Load(SoundDefines.m_SoundPath[i], typeof(AudioClip)) as AudioClip;
                if (m_AudioClipList[i])
                {
                    m_ClipListPlay[i].clip = m_AudioClipList[i];
                }
                else
                {
                    Debug.LogError("Can't find Clip GameObject m_AudioClipList");
                }    
                if (m_SoundInfo.ContainsKey(GetClipsName(SoundDefines.m_SoundPath[i])))
                {
                    m_SoundInfo.Add(GetClipsName(SoundDefines.m_SoundPath[i]), i);
                }             
            }
            m_SinglePlay = null;
        }

        //获取指定的GameObject
        //public GameObject GetUIObject(EnumSFXType _SFXType)
        //{
        //    GameObject _getObj = null;
        //    if (!audioClips.TryGetValue(_SFXType, out _getObj))
        //    {
        //        Debugger.Log("GetUIObject Failue!_uiType:" + _SFXType.ToString());
        //    }
        //    return _getObj;
        //}

        //获取文件名作为Key
        public string GetClipsName(string path)
        {
            string name = path;
            //去掉路径前缀，只有文件名
            name = name.Replace("Sounds/", string.Empty);
            return name;
        }

        public  bool PlayMusic(int sndID, bool loop = true)
        {
            if (sndID < 0 || sndID >= m_AudioClipList.Length)
                return false;
            //if (SoundDefines.MusicSwith == false)
                //return false;
            if (m_SinglePlay.isPlaying)
                m_SinglePlay.Stop();
            m_SinglePlay.clip = m_AudioClipList[sndID];
            m_SinglePlay.loop = loop;
            m_SinglePlay.Play();
            //RefreshMusicVolume();
            return true;
        }
        public void RegisterAllAudio()
        {
            PreloadSFXClip("SFX/Laser1", 0);
            //PreloadMusicClip("Music/Carve Him Up", 1);
        }

        #region SFX  音效
        //预加载音效
        public void PreloadSFXClip(string path, int id)
        {
            ResManager.Instance.LoadCoroutineInstance(path, (_obj) =>
            {
                if (_obj)
                {
                    RSFXClips[id] = _obj as AudioClip;
                    SFXPlayer[id][0].clip = RSFXClips[id];
                }
            });
        }

        //播放音效列表
        public void PlaySFXList(int id, bool isloop = false)
        {
            var players = SFXPlayer[id];
            //if (SFXState == EnumSFXState.SFX_ON)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (!players[i].isPlaying)
                    {
                        players[i].clip = RSFXClips[id];
                        players[i].loop = isloop;
                        players[i].Play();
                    }

                }
            }

            // 控制10个
            if (players.Count == 10)
            {
                PlaySFXSingle(id);
            }

            var newSource = gameObject.AddComponent<AudioSource>();
            players.Add(newSource);
            newSource.clip = RSFXClips[id];
            newSource.Play();
        }

        // Used to play single sound clips.播放一首音效
        public void PlaySFXSingle(int id)
        {
            //if (RSFXState == SETTINGS.SFX_ON)
            {
                SFXPlayer[id][0].Play();
            }
        }
        //public void LoadSoundSync(string path, int id)
        //{
        //    var obj = Resources.Load(path);
        //    RSFXClips[id] = obj as AudioClip;
        //    SFXPlayer[id][0].clip = RSFXClips[id];
        //}

        ////Stop SFX. 停止所有音效
        //public void StopSFX(int id)
        //{
        //    var players = SFXPlayer[id];
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].Stop();
        //    }
        //}
        ////Mute SFX. 音效静音
        //public void MuteSFX(int id)
        //{
        //    RSFXState = SETTINGS.SFX_OFF;
        //    var players = SFXPlayer[id];
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].mute = true;
        //    }
        //}
        ////UnMute SFX. 开启音效
        //public void UnMuteSFX(int id)
        //{
        //    RMusicState = SETTINGS.SFX_ON;
        //    var players = SFXPlayer[id];
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].mute = false;
        //    }
        //}
        #endregion

        //#region MUSIC 音乐
        ////预加载音乐
        //public void PreloadMusicClip(string path, int id)
        //{
        //    ResManager.Instance.LoadCoroutineInstance(path, (_obj) =>
        //    {
        //        if (_obj)
        //        {

        //            RMusicClips[id] = _obj as AudioClip;
        //            MUSICPlayer[id][0].clip = RMusicClips[id];
        //        }
        //    });
        //}
        ////播放音乐列表
        //public void PlayMusicList(int id, bool loop = false)
        //{
        //    if (RMusicState == SETTINGS.MUSIC_OFF)
        //    {
        //        return;
        //    }

        //    var players = MUSICPlayer[id];

        //    int count = players.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        if (players[i].isPlaying)
        //        {

        //        }
        //        else
        //        {
        //            players[i].clip = RMusicClips[id];
        //            players[i].loop = loop;
        //            players[i].Play();
        //            return;
        //        }
        //    }

        //    // 控制10个
        //    if (count == 10)
        //    {
        //        PlayMusicSingle(id);
        //        return;
        //    }

        //    var newSource = gameObject.AddComponent<AudioSource>();
        //    players.Add(newSource);
        //    newSource.clip = RMusicClips[id];
        //    newSource.Play();
        //}

        //// Used to play single sound clips.播放一首音乐
        //public void PlayMusicSingle(int id)
        //{
        //    MUSICPlayer[id][0].Play();
        //}

        ////Stop Music. 停止所有音乐
        //public void StopMusic(int id)
        //{
        //    var players = MUSICPlayer[id];
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].Stop();
        //    }
        //}
        ////Mute Music. 音乐静音
        //public void MuteMusic(int id)
        //{
        //    RMusicState = SETTINGS.MUSIC_OFF;
        //    var players = MUSICPlayer[id];
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].mute = true;
        //    }
        //}
        ////UnMute Music. 开启音乐
        //public void UnMuteMusic(int id)
        //{
        //    RMusicState = SETTINGS.MUSIC_ON;
        //    var players = MUSICPlayer[id];
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].mute = false;
        //    }
        //}
        //#endregion


        //#region ALL AUDIO

        //public void SetAllVolume(float setVolume)
        //{

        //}
        ////开启所有音效与音乐
        //public void UnMuteAllAudio()
        //{
        //    listener.enabled = true;
        //    if (RMusicState == SETTINGS.MUSIC_OFF)
        //    {
        //        UnMuteMusic(2);
        //    }
        //    if (RSFXState == SETTINGS.SFX_OFF)
        //    {
        //        UnMuteSFX(0);
        //    }
        //}

        ////关闭所有音效与音乐
        //public void MuteAllAudio()
        //{

        //    if (RMusicState == SETTINGS.MUSIC_ON)
        //    {
        //        MuteMusic(3);
        //    }
        //    if (RSFXState == SETTINGS.SFX_ON)
        //    {
        //        MuteSFX(2);
        //    }
        //    if (RMusicState == SETTINGS.MUSIC_OFF && RSFXState == SETTINGS.SFX_OFF)
        //    {
        //        listener.enabled = false;
        //    }

        //}

        //// Stops all sound, music and sfx
        //public void Stop(int id)
        //{

        //}
        //// Pauses all music and SFX.
        //public void Pause()
        //{

        //}
        //#endregion




        //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.音效播放随机音乐片段和随机音乐速度
        //public void RandomizeSfx(params AudioClip[] clips)
        //{            
        //    int randomIndex = Random.Range(0, clips.Length);                      
        //    float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        //    sfxPlayer.pitch = randomPitch;
        //    sfxPlayer.clip = clips[randomIndex];
        //    sfxPlayer.Play();
        //}

    }
}