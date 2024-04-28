/**
 * @file    ParticleManager.cs
 * @brief   パーティクルの管理クラス
 * @date    2021/09/06
 */
using MKTSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class ParticleManager : SingletonMonoBehaviour<ParticleManager> {
        [Header("スタックするParticleリスト ※設定不要")]
        [SerializeField] List<GameObject> m_StackParticleList     = new List<GameObject>();
        [Header("スタックするループParticleリスト ※設定不要")]
        [SerializeField] List<GameObject> m_StackLoopParticleList = new List<GameObject>();
        
        void Start() {
            DontDestroyOnLoad(gameObject);
        }
        /// <summary>
        /// Particleの終了判定用コルーチン
        /// <param name="particleobj"> 使用しているParticleオブジェクト</param>
        /// <param name="activetime">  パーティクルの表示時間</param>
        /// <param name="parent">      パーティクルの親</param>
        /// <param name="callback">    再生終了後の処理</param>
        /// </summary>
        private IEnumerator ParticleStopChecking(GameObject particleobj,float activetime, Transform parent = null,System.Action callback = null) {
            while (activetime > 0.0f) {
                activetime -= Time.deltaTime;
                if (parent != null) {
                    particleobj.transform.position = parent.position;
                }
                yield return null;
            }
            particleobj.SetActive(false);

            callback?.Invoke();
            yield break;
        }
        /// <summary>
        /// Particleの生成関数
        /// <param name="particlename"> 出現させるParticle名</param>
        /// <param name="pos">          生成位置の座標</param>
        /// <param name="rot">          生成位置の回転値</param>
        /// <param name="activetime">   パーティクルの表示時間</param>
        /// <param name="parent">       パーティクルの親</param>
        /// <param name="callback">     終了後の処理</param>
        /// </summary>
        public void ParticlePlay(string particlename,Vector3 pos,Quaternion rot,float activetime, Transform parent = null, System.Action callback = null) {
            GameObject particle = null;

            //スタックしているParticleListに、使用していないパーティクルオブジェクトがあればそれを使用
            for (int i = 0; i < m_StackParticleList.Count; i++) {
                if (m_StackParticleList[i] != null && m_StackParticleList[i].activeInHierarchy == false && m_StackParticleList[i].name == particlename) {
                    particle = m_StackParticleList[i];
                    break;
                }
            }
            //この段階でparticleのパーティクルオブジェクトが無ければ生成する。
            if (particle == null) {
                //Particleの生成処理
                /*
                 * AssetBundleを使用する際はこのコメントアウトの処理を使用する。
                GameObject orgparticle = MKTCachingLoad.CachingLoadExample.Instance.getParticleBundle.LoadAsset<GameObject>("Assets/Product/Resources/Particle/" + nextclipname + ".prefab");
                if (orgparticle == null) {
                   orgparticle = MKTCachingLoad.CachingLoadExample.Instance.getParticleBundle.LoadAsset<GameObject>("Assets/Product/Resources/Particle/" + nextclipname + ".Prefab");
                }*/
                //Resource読込を行う
                GameObject orgparticle = Resources.Load<GameObject>("Particle/" + particlename);
                particle = Instantiate(orgparticle);
                if (particle == null) {
                    return;
                }
                particle.name = particlename;
                //オブジェクトの親子関係を設定
                particle.transform.SetParent(transform);
                //リストに追加。
                m_StackParticleList.Add(particle);
            }

            //パーティクルの位置と回転値を設定
            particle.transform.position = pos;
            particle.transform.rotation = rot;

            //particleが正常に読み込めていれば再生
            if (particle != null) {
                particle.SetActive(true);
            }
            //コルーチンを回して、終了判定を取る。
            StartCoroutine(ParticleStopChecking(particle, activetime, parent, callback));
        }
        /// <summary>
        /// ループするParticleの再生関数
        /// <param name="particlename"> 出現させるParticle名</param>
        /// <param name="pos">          生成位置の座標</param>
        /// <param name="rot">          生成位置の回転値</param>
        /// <param name="parent">       親オブジェクト設定用</param>
        /// </summary>
        public void LoopParticlePlay(string particlename, Vector3 pos, Quaternion rot, Transform parent = null) {
            GameObject loopparticle = null;

            //スタックしているループParticleListに、再生中の同名Particleがあれば処理しない。
            for (int i = 0; i < m_StackLoopParticleList.Count; i++) {
                if (m_StackLoopParticleList[i] != null && m_StackLoopParticleList[i].activeInHierarchy == true && m_StackLoopParticleList[i].name == particlename) {
                    return;
                }
            }

            //スタックしているループParticleListに、使用していないパーティクルオブジェクトがあればそれを使用
            for (int i = 0; i < m_StackLoopParticleList.Count; i++) {
                if (m_StackLoopParticleList[i] != null && m_StackLoopParticleList[i].activeInHierarchy == false && m_StackLoopParticleList[i].name == particlename) {
                    loopparticle = m_StackLoopParticleList[i];
                    break;
                }
            }
            //この段階でparticleのパーティクルオブジェクトが無ければ生成する。
            if (loopparticle == null) {
                //Particleの生成処理
                /*
                 * AssetBundleを使用する際はこのコメントアウトの処理を使用する。
                GameObject orgparticle = MKTCachingLoad.CachingLoadExample.Instance.getParticleBundle.LoadAsset<GameObject>("Assets/Product/Resources/Particle/" + nextclipname + ".prefab");
                if (orgparticle == null) {
                   orgparticle = MKTCachingLoad.CachingLoadExample.Instance.getParticleBundle.LoadAsset<GameObject>("Assets/Product/Resources/Particle/" + nextclipname + ".Prefab");
                }*/
                //Resource読込を行う
                GameObject orgparticle = Resources.Load<GameObject>("Particle/" + particlename);
                loopparticle = Instantiate(orgparticle);
                if (loopparticle == null) {
                    return;
                }
                loopparticle.name = particlename;
                //オブジェクトの親子関係を設定
                loopparticle.transform.SetParent(transform);
                //リストに追加。
                m_StackLoopParticleList.Add(loopparticle);
            }

            //パーティクルの位置と回転値を設定
            loopparticle.transform.position = pos;
            loopparticle.transform.rotation = rot;
            //親を設定
            if (parent = null) {
                loopparticle.transform.SetParent(parent);
            }
            //particleが正常に読み込めていれば再生
            if (loopparticle != null) {
                loopparticle.SetActive(true);
            }
        }
        /// <summary>
        /// ループするParticleの停止関数
        /// <param name="particlename"> 停止させるParticle名</param>
        /// </summary>
        public void LoopParticleStop(string particlename) {
            for (int i = 0; i < m_StackLoopParticleList.Count; i++) {
                if (particlename == m_StackLoopParticleList[i].name) {
                    m_StackLoopParticleList[i].SetActive(false);
                    return;
                }
            }
        }
        public void LoopParticleStopAll() {
            for (int i = 0; i < m_StackLoopParticleList.Count; i++) {
                m_StackLoopParticleList[i].SetActive(false);
            }
        }
    }
}