/**
 * @file    SEManager.cs
 * @brief   SEの管理クラス
 * @date    2021/09/06
 */
using MKTSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class SEManager : SingletonMonoBehaviour<SEManager> {
        [Header("スタックするSEリスト ※設定不要")]
        [SerializeField] List<AudioSource> m_StackSEList     = new List<AudioSource>();
        [Header("スタックするループSEリスト ※設定不要")]
        [SerializeField] List<AudioSource> m_StackLoopSEList = new List<AudioSource>();
        [Header("設定画面時に使用するSEの音量")]
        [SerializeField] float             m_ConfigSEVol     = 0.0f;
        public           float             setConfigSEVol { set { m_ConfigSEVol = value; } }
        [Header("設定画面時にオンにするフラグ ※設定不要")]
        [SerializeField] bool              m_ConfigFlag      = false;
        public           bool              setConfigFlag  { set { m_ConfigFlag  = value; } }
        //設定画面時にオンになり続けるフラグ
        private          bool              m_ConfigNawFlag   = false;
        
        void Start() {
            DontDestroyOnLoad(gameObject);
        }
        void FixedUpdate() {
            //設定画面時は、毎フレーム音量を確認する。
            if (m_ConfigFlag == true) {
                m_ConfigNawFlag = true;
                for (int i = 0; i < m_StackSEList.Count; i++) {
                    m_StackSEList[i].volume     = m_ConfigSEVol;
                }
                for (int i = 0; i < m_StackLoopSEList.Count; i++) {
                    m_StackLoopSEList[i].volume = m_ConfigSEVol;
                }
            //設定画面でない時
            } else {
                //設定画面時にオンになり続けるフラグがオンならば、一度のみ再設定処理。
                if (m_ConfigNawFlag == true) {
                    for (int i = 0; i < m_StackSEList.Count; i++) {
                        m_StackSEList[i].volume     = ConfigBinarySaveManager.Instance?.getSEVolume ?? 0.0f; ;
                    }
                    for (int i = 0; i < m_StackLoopSEList.Count; i++) {
                        m_StackLoopSEList[i].volume = ConfigBinarySaveManager.Instance?.getSEVolume ?? 0.0f; ;
                    }
                    m_ConfigNawFlag = false;
                }
            }
        }
        /// <summary>
        /// SEの終了判定用コルーチン
        /// <param name="sename">   再生する音源名</param>
        /// <param name="callback"> 再生終了後の処理</param>
        /// </summary>
        private IEnumerator SEStopChecking(AudioSource se,System.Action callback = null) {
            while (se.isPlaying) {
                yield return new WaitForFixedUpdate();
            }
            se.Stop();
            se.clip = null;

            callback?.Invoke();
            yield break;
        }
        /// <summary>
        /// SEの再生関数
        /// <param name="sename">   再生する音源名</param>
        /// <param name="callback"> 再生終了後の処理</param>
        /// </summary>
        public void SEPlay(string sename, System.Action callback = null) {
            AudioSource se = null;

            //スタックしているSEListに、使用していないAudioSourceがあればそれを使用
            for (int i = 0; i < m_StackSEList.Count; i++) {
                if (m_StackSEList[i] != null && m_StackSEList[i].clip == null) {
                    se = m_StackSEList[i];
                    break;
                }
            }
            //この段階でseのAudioSourceが無ければ生成する。
            if (se == null) {
                se = new GameObject("AudioSource_SE")?.AddComponent<AudioSource>() ?? null;
                if (se == null) {
                    return;
                }
                //オブジェクトの位置と親子関係を設定
                se.transform.position = transform.position;
                se.transform.SetParent(transform);
                //リストに追加。
                m_StackSEList.Add(se);
            }

            //音量を設定
            if (m_ConfigFlag == true) {
                se.volume = m_ConfigSEVol;
            } else {
                se.volume = ConfigBinarySaveManager.Instance?.getSEVolume ?? 0.0f;
            }
            //SEの再生処理
            /*
             * AssetBundleを使用する際はこのコメントアウトの処理を使用する。
            m_AudioSource.clip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/SE/" + nextclipname + ".mp3");
            if (m_AudioSource.clip == null) {
               m_AudioSource.clip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/SE/" + nextclipname + ".wav");
            }*/
            //Resource読込を行う
            se.clip = Resources.Load<AudioClip>("SE/" + sename);
            //clipが正常に読み込めれば再生
            if (se.clip != null) { 
                se.Play();
            }

            //コルーチンを回して、終了判定を取る。
            StartCoroutine(SEStopChecking(se, callback));
        }
        /// <summary>
        /// ループするSEの再生関数
        /// <param name="sename">  再生する音源名</param>
        /// </summary>
        public void LoopSEPlay(string sename) {
            AudioSource loopse = null;

            //ループSEは、同じ音を重複させない。
            for (int i = 0; i < m_StackLoopSEList.Count; i++) {
                if (m_StackLoopSEList[i].clip.name == sename) {
                    return;
                }
            }

            //スタックしているループSEListに、使用していないAudioSourceがあればそれを使用
            for (int i = 0; i < m_StackLoopSEList.Count; i++) {
                if (m_StackLoopSEList[i] != null && m_StackLoopSEList[i].clip == null) {
                    loopse = m_StackLoopSEList[i];
                    break;
                }
            }
            //この段階でloopseのAudioSourceが無ければ生成する。
            if (loopse == null) {
                loopse = new GameObject("AudioSource_LoopSE")?.AddComponent<AudioSource>() ?? null;
                if (loopse == null) {
                    return;
                }
                //オブジェクトの位置と親子関係を設定
                loopse.transform.position = transform.position;
                loopse.transform.SetParent(transform);
                //リストに追加。
                m_StackLoopSEList.Add(loopse);
            }

            //音量を設定
            if (m_ConfigFlag == true) {
                loopse.volume = m_ConfigSEVol;
            } else { 
                loopse.volume = ConfigBinarySaveManager.Instance?.getSEVolume ?? 0.0f;
            }
            //ループチェックをオン
            loopse.loop   = true;
            //SEの再生処理
            /*
             * AssetBundleを使用する際はこのコメントアウトの処理を使用する。
            m_AudioSource.clip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/SE/" + nextclipname + ".mp3");
            if (m_AudioSource.clip == null) {
               m_AudioSource.clip = MKTCachingLoad.CachingLoadExample.Instance.getBGMBundle.LoadAsset<AudioClip>("Assets/Product/Resources/SE/" + nextclipname + ".wav");
            }*/
            //Resource読込を行う
            loopse.clip = Resources.Load<AudioClip>("SE/" + sename);
            //clipが正常に読み込めれば再生
            if (loopse.clip != null) {
                loopse.Play();
            }
        }
        /// <summary>
        /// ループするSEの停止関数
        /// <param name="sename">  再生する音源名</param>
        /// </summary>
        public void LoopSEStop(string sename) {
            for (int i = 0; i < m_StackLoopSEList.Count; i++) {
                if (sename == m_StackLoopSEList[i].clip.name) {
                    m_StackLoopSEList[i].Stop();
                    m_StackLoopSEList[i].clip = null;
                    return;
                }
            }
        }
    }
}