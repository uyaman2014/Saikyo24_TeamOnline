/**
 * @file    GameStartManager.cs
 * @brief   ゲームスタート時の管理クラス
 * @date    2024/04/29
 */
using MKTSingleton;
using UnityEngine;

namespace Kikukawa {
    public class GameResultManager : SingletonMonoBehaviour<GameResultManager> {
        [SerializeField] GameObject m_GameClearObj;
        [SerializeField] GameObject m_GameOverObj;
        [SerializeField] bool m_GameClearFlag = false;

        void Start() {
            if (m_GameClearFlag) {
                Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
                Manager.FadeManager.Instance.SetFadeFlag(false);
                m_GameClearObj.SetActive(true);
            } else {
                Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
                m_GameOverObj.SetActive(true);
            }

        }
    }
}