/**
 * @file    BGMManager.cs
 * @brief   BGMの管理クラス
 * @date    2021/08/30
 */
using MKTSingleton;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    [System.Serializable]
    public class PreludeBGM {
        [Header("前奏")]
        public string m_PreludeName = "";
        [Header("ループ")]
        public string m_LoopName    = "";
    }
    public class BGMManager : SingletonMonoBehaviour<BGMManager> {
        [Header("前奏が存在するBGMの要素配列")]
        [SerializeField] List<PreludeBGM> m_PreludeBGM       = new List<PreludeBGM>() {
                                                                   new PreludeBGM(){
                                                                       m_PreludeName = "Misterioso",
                                                                       m_LoopName    = "Misterioso_Loop"
                                                                   },
                                                                   new PreludeBGM(){
                                                                       m_PreludeName = "Strepitoso",
                                                                       m_LoopName    = "Strepitoso_Loop"
                                                                   },
                                                                   new PreludeBGM(){
                                                                       m_PreludeName = "a rigore, a piacere",
                                                                       m_LoopName    = "a rigore, a piacere_Loop"
                                                                   },
                                                               };
        [Header("事前確認用前奏")]
        [SerializeField] AudioClip        m_SetPreludeClip   = null;
        [Header("事前格納用")]
        [SerializeField] AudioClip        m_SetLoopClip      = null;

        [Header("前回再生した音源名")]
        [SerializeField] string           m_OldClipName      = "";
        [Header("AudioSourceコンポーネント")]
        [SerializeField] AudioSource      m_AudioSource      = null;
        [Header("設定画面フラグ")]
        [SerializeField] bool             m_ConfigSceneFlag  = false;
        public           bool             setConfigSceneFlag { set { m_ConfigSceneFlag = value; } }
        [Header("BGM切り替えフェード中フラグ")]
        [SerializeField] bool             m_BGMChangeingFlag = false;
        [Header("次に再生する音源名")]
        [SerializeField] string           m_NextClipName     = "";
        [Header("フェードスピード")]
        [SerializeField] float            m_Speed            = 1.25f;
        [Header("フェード中に使う音量変数")]
        [SerializeField] float             m_FadeBGMVol       = 0.0f;
        [Header("最大ボリューム格納用変数")]
        [SerializeField] float            m_MaxVol           = 0.0f;
        [Header("フェードフラグ")]
        [SerializeField] bool             m_FadeFlag         = false;

        void Start() {
            DontDestroyOnLoad(gameObject);
            InitAudioSource();
        }
        void Update() {
            InitAudioSource();
            //ループ音源でないとき
            if (!m_AudioSource.loop) {
                //曲が切り替わるタイミングが来た場合
                if (!m_AudioSource.isPlaying) {
                    //該当するオーディオクリップのループ音源を設定
                    if (m_AudioSource.clip == m_SetPreludeClip) {
                        m_AudioSource.clip = m_SetLoopClip;
                        //そしてループ再生モードに変更する。
                        m_AudioSource.loop = true;
                        m_AudioSource.Play();
                    }
                }
            }
            BGMFade();
        }
        void LateUpdate() {
            if (m_ConfigSceneFlag == false) {
                InitAudioSource();
                //フェード命令が行われていない時
                if (m_BGMChangeingFlag == false) {
                    m_AudioSource.volume = ConfigBinarySaveManager.Instance?.getBGMVolume ?? 0.0f;
                //フェード命令が行われている時
                } else {
                    m_AudioSource.volume = m_FadeBGMVol;
                }
            }
        }
        /// <summary>
        /// BGM用のAudioSource作成処理関数
        /// </summary>
        private void InitAudioSource() {
            if (m_AudioSource != null) {
                return;
            }
            //AudioSourceを取得
            m_AudioSource = this.GetComponent<AudioSource>();
            if (m_AudioSource == null) {
                if (m_AudioSource == null) {
                    m_AudioSource = gameObject.AddComponent<AudioSource>();
                }
                if (m_AudioSource == null) {
                    return;
                }
            }
            //BGMのvolumeを設定する
            ConfigBinarySaveManager.Instance.Load();
            m_AudioSource.volume = ConfigBinarySaveManager.Instance?.getBGMVolume ?? 0.0f;
        }
        /// <summary>
        /// BGMのフェード更新処理関数
        /// </summary>
        private void BGMFade() {
            if (m_BGMChangeingFlag == false) {
                return;
            }
            InitAudioSource();
            //もし、フェードフラグがオンならば
            if (m_FadeFlag) {
                //BGMを下げる処理から入る。
                m_FadeBGMVol -= (((m_Speed * m_MaxVol) * Time.deltaTime));
                if (m_AudioSource.clip == null) {
                    m_FadeBGMVol = 0.0f;
                }
                //BGMの音量が0となった時
                if (m_FadeBGMVol <= 0.0f) {
                    //BGMを切り替えてこの処理を終える。
                    BGMChange(m_NextClipName);
                    m_FadeFlag = false;
                }
                //フェードフラグがオフならば、
            } else {
                //BGMを上げる処理。
                m_FadeBGMVol += (((m_Speed * m_MaxVol) * Time.deltaTime));
                if (m_AudioSource.clip == null) {
                    m_FadeBGMVol = m_MaxVol;
                }
                //BGMの音量が最大となった時
                if (m_FadeBGMVol >= m_MaxVol) {
                    //BGM音量に補正を加えて、この処理を終える。
                    m_FadeBGMVol = m_MaxVol;
                    m_BGMChangeingFlag = false;
                }
            }
        }
        /// <summary>
        /// 前奏があるBGMを再生したときに設定を最適化する関数
        /// </summary>
        public void KentiAudio() {
            InitAudioSource();
            for (int i = 0; i < m_PreludeBGM.Count; i++) {
                string asclipname = "";
                if (m_AudioSource.clip != null) {
                    asclipname = m_AudioSource.clip.name;
                }

                //もし、該当する前奏があるオーディオクリップを再生したとき
                if (asclipname == m_PreludeBGM[i].m_PreludeName && m_AudioSource.loop) {
                    /*
                     * AssetBundleを使用する際はこのコメントアウトの処理を使用する。
                    m_SetPreludeClip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/BGM/" + m_PreludeBGM[i].m_PreludeName + ".mp3");
                    if (m_SetPreludeClip == null) {
                        m_SetPreludeClip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/BGM/" + m_PreludeBGM[i].m_PreludeName + ".wav");
                    }
                    m_SetLoopClip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/BGM/" + m_PreludeBGM[i].m_LoopName + ".mp3");
                    if (m_SetLoopClip == null) {
                        m_SetLoopClip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/BGM/" + m_PreludeBGM[i].m_LoopName + ".wav");
                    }*/
                    //Resource読込を行う
                    m_SetPreludeClip = Resources.Load<AudioClip>("BGM/" + m_PreludeBGM[i].m_PreludeName);
                    m_SetLoopClip    = Resources.Load<AudioClip>("BGM/" + m_PreludeBGM[i].m_LoopName);

                    //一度だけ再生モードに変更する。
                    m_AudioSource.loop = false;
                    m_AudioSource.Play();

                    //一旦ここでメモリ開放
                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();
                }
            }
        }
        /// <summary>
        /// BGMの即時切り替え関数
        /// <param name="nextclipname">  次に再生する音源名</param>
        /// </summary>
        public void BGMChange(string nextclipname = "") {
            InitAudioSource();
            //もし、前回の番号と同じならば
            if (m_OldClipName == nextclipname) {
                //何もしない
                return;
            //前回と違うBGMが読まれた時は切り替える
            } else {
                /*
                 * AssetBundleを使用する際はこのコメントアウトの処理を使用する。
                m_AudioSource.clip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/BGM/" + nextclipname + ".mp3");
                if (m_AudioSource.clip == null) {
                    m_AudioSource.clip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/BGM/" + nextclipname + ".wav");
                }*/
                //Resource読込を行う
                m_AudioSource.clip = Resources.Load<AudioClip>("BGM/" + nextclipname);

                m_AudioSource.loop    = true;
                m_AudioSource.Play();
                m_OldClipName         = nextclipname;

                KentiAudio();
            }
        }
        /// <summary>
        /// フェードをかけたBGM変更関数
        /// <param name="nextclipname">  次に再生する音源名</param>
        /// <param name="Speed">         フェードをかける際のスピード</param>
        /// </summary>
        public void FadeBGMChange(string nextclipname = "", float Speed = 1.25f) {
            //もし、前回の番号と同じならば
            if (m_OldClipName == nextclipname) {
                //何もしない
            //前回と違うBGMが読まれた時は切り替える
            } else {
                //渡された番号を保持。
                m_NextClipName     = nextclipname;
                //フェードスピードを同期
                m_Speed            = Speed;
                //同期前に必ずロード！
                ConfigBinarySaveManager.Instance.Load();
                //最大音声を同期
                m_MaxVol           = ConfigBinarySaveManager.Instance?.getBGMVolume ?? 0.0f;
                //フェード中にも使う音量変数に現在の音量を保存
                m_FadeBGMVol       = ConfigBinarySaveManager.Instance?.getBGMVolume ?? 0.0f;
                //使用するフラグをオンへ
                m_FadeFlag         = true;
                //フラグをオンにしてこの処理だけとする
                m_BGMChangeingFlag = true;
            }
        }
        /// <summary>
        /// コンフィグ設定中のBGM音量変更関数
        /// <param name="bgmvol">  設定するBGM音量</param>
        /// </summary>
        public void SetConfigBGM(float bgmvol) {
            InitAudioSource();
            //設定画面の時は設定情報を参照する。
            m_AudioSource.volume = bgmvol;
        }
    }
}