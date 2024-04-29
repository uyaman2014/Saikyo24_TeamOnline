/**
 * @file    GameStartManager.cs
 * @brief   ゲームスタート時の管理クラス
 * @date    2023/08/06
 */
using MKTSingleton;
using UnityEngine;

namespace Kikukawa {
    public class GameStartManager : SingletonMonoBehaviour<GameStartManager> {
        [SerializeField] bool m_Test = false;
        private Vector3 m_pos = Vector3.zero;

        [SerializeField] Animator m_TitleStart = null;

        void Start() {
            Manager.BGMManager.Instance.FadeBGMChange("BGM_2");
            Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
            Manager.FadeManager.Instance.SetFadeFlag(false,()=> {
                if (m_TitleStart) { 
                    m_TitleStart.SetBool("TitleStart",true);
                }
            });
            m_pos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        }
       void Update() {
            if (m_Test) {
                Manager.ParticleManager.Instance.ParticlePlay("BigHeart", m_pos, Quaternion.identity,2.0f);
                Manager.SEManager.Instance.SEPlay("10Combo");
                m_Test = false;
            }
        }

        public void GameStart() {
            Manager.BGMManager.Instance.FadeBGMChange("");
            Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
            Manager.FadeManager.Instance.SetFadeFlag(true,()=> {
                GameSequenceManager.Instance.GoToNextScene();
            
                Manager.BGMManager.Instance.FadeBGMChange("Main");
                Manager.FadeManager.Instance.SetFadeFlag(false);
            });
        }
    }
}