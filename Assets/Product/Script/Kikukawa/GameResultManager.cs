/**
 * @file    GameStartManager.cs
 * @brief   ゲームスタート時の管理クラス
 * @date    2024/04/29
 */
using MKTSingleton;
using UnityEngine;

namespace Kikukawa {
    public class GameResultManager : SingletonMonoBehaviour<GameResultManager> {
        [SerializeField] bool m_GameClearFlag = false;
        [SerializeField] GameObject m_Success;
        [SerializeField] GameObject m_Failure;

        [SerializeField] GameObject m_GameClearObj;
        [SerializeField] GameObject m_GameOverObj;

        [SerializeField] GameObject m_SuccessButtonObj;
        [SerializeField] GameObject m_FailureButtonObj;

        [SerializeField] Animator m_SuccessGrimReaper_Anim;
        [SerializeField] Animator m_FailureGrimReaper_Anim;

        [SerializeField] GameObject m_SuccessHukidasiObj;
        [SerializeField] GameObject m_FailureHukidasiObj;

        [SerializeField] GameObject m_TitleChangeObj;

        void Start() {
            if (m_GameClearFlag) {
                m_Success.SetActive(true);
                Manager.BGMManager.Instance.FadeBGMChange("Result2");
                Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
                Manager.FadeManager.Instance.SetFadeFlag(false,()=> {
                    m_GameClearObj.SetActive(true);
                });
            } else {
                m_Failure.SetActive(true);
                Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
                m_GameOverObj.SetActive(true);
            }

        }

        public void SuccessLost() {
            m_SuccessButtonObj.SetActive(false);
            m_SuccessGrimReaper_Anim.SetBool("Lost", true);
            m_SuccessHukidasiObj.SetActive(true);
            m_TitleChangeObj.SetActive(true);
        }

        public void FailureLost() {
            m_FailureButtonObj.SetActive(false);
            m_FailureGrimReaper_Anim.SetBool("Lost", true);
            m_FailureHukidasiObj.SetActive(true);
            m_TitleChangeObj.SetActive(true);
        }

        public void GoToNextScene() { 
            Manager.BGMManager.Instance.FadeBGMChange("");
            Manager.FadeManager.Instance.SetFadeFlag(true,()=>{ 
                GameSequenceManager.Instance.GoToNextScene();
            });

        }
    }
}