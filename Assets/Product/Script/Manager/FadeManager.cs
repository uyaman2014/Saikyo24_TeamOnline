/**
 * @file    FadeManager.cs
 * @brief   フェードの管理クラス
 * @date    2021/08/28
 */
using MKTSingleton;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Manager {
    public class FadeManager : SingletonMonoBehaviour<FadeManager> {
        [Header("フェード用のCanvas")]
        [SerializeField] Canvas m_Canvas          = null;
        [Header("フェード用の画像")]
        [SerializeField] Image  m_FadeImage       = null;
        public           Image  getFadeImage { get {
                if (CheckCanvasOrFadeImage() == false) {
                    Debug.Log("FadeManager.cs getFadeImage : フェード用Canvasまたは、フェード用の画像がありません。");
                    return null;
                }
                return m_FadeImage;
            } }

        [Header("フェードフラグ")]
        [SerializeField] bool   m_FadeFlag        = false;
        public           bool   getFadeFlag { get { return m_FadeFlag; } }
        [Header("フェードスピード")]
        [SerializeField] float  m_Speed           = 1.25f;
        [Header("フェード中フラグ")]
        [SerializeField] bool   m_FadeingFlag     = false;

        //フェードイン完了時のコールバック
        private          Action m_FadeInCallback  = null;
        //フェードアウト完了時のコールバック
        private          Action m_FadeOutCallback = null;
        void Start() {
            DontDestroyOnLoad(gameObject);
            if (CheckCanvasOrFadeImage() == false) {
                Debug.Log("FadeManager.cs Start() : フェード用Canvasまたは、フェード用の画像がありません。");
            }
        }
        void Update() {
            if (CheckCanvasOrFadeImage() == false) {
                Debug.Log("FadeManager.cs Update() : フェード用Canvasまたは、フェード用の画像がありません。");
                return;
            }

            //フェードフラグが立っている時はフェードイン処理
            if (m_FadeFlag) {
                if (m_FadeImage.enabled == false) {
                    m_FadeImage.enabled = true;
                }
                // フェードイン完了中の処理
                if (m_FadeImage.color.a >= 1.0f) {
                    m_FadeImage.color = new Color(m_FadeImage.color.r, m_FadeImage.color.g, m_FadeImage.color.b, 1.0f);
                    if (m_FadeingFlag == true) {
                        m_FadeingFlag = false;
                    }
                    //コールバック処理があれば処理
                    if (m_FadeInCallback != null) {
                        m_FadeInCallback();
                        m_FadeInCallback = null;
                    }
                // フェードイン中の処理
                } else {
                    m_FadeImage.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime * m_Speed);
                    if (m_FadeingFlag == false) {
                        m_FadeingFlag = true;
                    }
                }
            //フェードフラグが立っていない時はフェードアウト処理
            } else {
                // フェードアウト完了中の処理
                if (m_FadeImage.color.a <= 0.0f) {
                    m_FadeImage.color = new Color(m_FadeImage.color.r, m_FadeImage.color.g, m_FadeImage.color.b, 0.0f);
                    if (m_FadeingFlag == true) {
                        m_FadeingFlag = false;
                    }
                    //コールバック処理があれば処理
                    if (m_FadeOutCallback != null) {
                        m_FadeOutCallback();
                        m_FadeOutCallback = null;
                    }

                    if (m_FadeImage.enabled == true) {
                        m_FadeImage.enabled = false;
                    }
                // フェードアウト中の処理
                } else {
                    m_FadeImage.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime * m_Speed);
                    if (m_FadeingFlag == false) {
                        m_FadeingFlag = true;
                    }
                }
            }
        }
        /// <summary>
        /// フェード用Canvasと画像を取得する処理関数
        /// <returns>true(正常に取得成功)false(取得失敗)</returns>
        /// </summary>
        private bool CheckCanvasOrFadeImage() {
            bool successflag = true;

            if (m_Canvas == null) {
                GameObject fadecanvasorg = (GameObject)Resources.Load("Prefab/FadeCanvas");
                m_Canvas = Instantiate(fadecanvasorg, Vector3.zero, Quaternion.identity, transform)?.GetComponent<Canvas>() ?? null;
                if (m_Canvas == null) {
                    Debug.Log("FadeManager.cs Update() : フェード用Canvasの生成に失敗。");
                    successflag = false;
                }
            }
            if (m_FadeImage == null) {
                m_FadeImage = m_Canvas?.transform?.GetChild(0)?.GetComponent<Image>() ?? null;
                if (m_FadeImage == null) {
                    Debug.Log("FadeManager.cs Update() : フェード用の画像がありません。");
                    successflag = false;
                }
            }
            return successflag;
        }
        /// <summary>
        /// フェード処理設定関数
        /// <param name="fadeflag"> フェードイン(true)かフェードアウト(false)かの設定フラグ</param>
        /// <param name="coolback"> フェード処理後の処理</param>
        /// </summary>
        public void SetFadeFlag(bool fadeflag, Action coolback = null) {
            m_FadeFlag = fadeflag;

            if (m_FadeFlag == true) {
                m_FadeInCallback  = coolback;
            } else {
                m_FadeOutCallback = coolback;
            }
        }
        /// <summary>
        /// フェードカラー設定関数
        /// <param name="color"> 指定したい色</param>
        /// </summary>
        public void SetFadeColor(Color color) {
            if (CheckCanvasOrFadeImage() == false) {
                Debug.Log("FadeManager.cs SetFadeColor() : フェード用Canvasまたは、フェード用の画像がありません。");
                return;
            }
            m_FadeImage.color = color;
        }
    }
}