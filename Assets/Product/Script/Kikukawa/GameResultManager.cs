/**
 * @file    GameStartManager.cs
 * @brief   �Q�[���X�^�[�g���̊Ǘ��N���X
 * @date    2024/04/29
 */
using MKTSingleton;
using UnityEngine;

namespace Kikukawa {
    public class GameResultManager : SingletonMonoBehaviour<GameResultManager> {
        [SerializeField] GameObject m_Inochi_Anim;

        void Start() {
            Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
            if (m_Inochi_Anim) {
                m_Inochi_Anim.SetActive(true);
            }
        }
    }
}