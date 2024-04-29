/**
 * @file    GameStartManager.cs
 * @brief   ゲームリザルト画面の管理クラス
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
            var pInfos = NetworkManager.Instance.PlayerInfos;
            int countSum = 0;
            foreach (var pInfo in pInfos)
            {
                countSum += pInfo.Value.Count;
            }
            m_GameClearFlag = GameParameterManager.Instance.TargetClickCount >= countSum && GameParameterManager.Instance.TargetClickCount - 10 < countSum;
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
                Manager.FadeManager.Instance.SetFadeFlag(false);
                m_GameOverObj.SetActive(true);
            }

        }

        public void SuccessLost() {
            m_SuccessGrimReaper_Anim.SetBool("Lost", true);
            m_SuccessHukidasiObj.SetActive(true);
            m_TitleChangeObj.SetActive(true);
            Manager.SEManager.Instance.SEPlay("New/ゲームオーバー笑い声");

            m_SuccessButtonObj.SetActive(false);
        }

        public void FailureLost() {
            m_FailureGrimReaper_Anim.SetBool("Lost", true);
            m_FailureHukidasiObj.SetActive(true);
            m_TitleChangeObj.SetActive(true);
            Manager.SEManager.Instance.SEPlay("New/ゲームオーバー笑い声");

            m_FailureButtonObj.SetActive(false);
        }

        public void GoToNextScene() { 
            Manager.BGMManager.Instance.FadeBGMChange("");
            Manager.FadeManager.Instance.SetFadeFlag(true,()=>{ 
                GameSequenceManager.Instance.GoToNextScene();
            });
        }
    }
}