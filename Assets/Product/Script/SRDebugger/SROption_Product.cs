/**
 * @file    SROption_Product.cs
 * @brief   SRDebuggerのOption拡張クラス
 * @date    2024/04/28
 */
using UnityEngine;
using System;
using System.ComponentModel;

public partial class SROptions {
    #region 定数

    /// <summary>
    /// 全般カテゴリ
    /// </summary>
    private const string GENERAL_CATEGORY = "General";
    /// <summary>
    /// サウンドカテゴリ
    /// </summary>
    private const string SOUND_CATEGORY = "Sound";
    /// <summary>
    /// フェードカテゴリ
    /// </summary>
    private const string FADE_CATEGORY = "Fade";
    /// <summary>
    /// パーティクルカテゴリ
    /// </summary>
    private const string PARTICLE_CATEGORY = "Particle";

    #endregion


    #region デバッグ機能

    [Category(SOUND_CATEGORY)]
    [DisplayName("BGM名 ※無音に設定する場合は空文字を入れてください")]
    [Sort(0)]
    public string BGMName {
        get;
        set;
    }
    [Category(SOUND_CATEGORY)]
    [DisplayName("BGM変更")]
    [Sort(1)]
    public void FadeBGMChange() {
        Manager.BGMManager.Instance.FadeBGMChange(BGMName);
    }

    [Category(SOUND_CATEGORY)]
    [DisplayName("SE名")]
    [Sort(2)]
    public string SEName {
        get;
        set;
    }
    [Category(SOUND_CATEGORY)]
    [DisplayName("SE再生")]
    [Sort(3)]
    public void SEPlayCool() {
        Manager.SEManager.Instance.SEPlay(SEName);
    }


    [Category(FADE_CATEGORY)]
    [DisplayName("フェードイン処理 ※処理後にフェードイン完了ログが呼ばれる")]
    [Sort(4)]
    public void FadeInCool() {
        Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
        Manager.FadeManager.Instance.SetFadeFlag(true,()=> {
            Debug.Log("フェードイン完了！");
        });
    }
    [Category(FADE_CATEGORY)]
    [DisplayName("フェードアウト処理 ※処理後にフェードアウト完了ログが呼ばれる")]
    [Sort(5)]
    public void FadeOutCool() {
        Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
        Manager.FadeManager.Instance.SetFadeFlag(false, () => {
            Debug.Log("フェードアウト完了！");
        });
    }


    [Category(PARTICLE_CATEGORY)]
    [DisplayName("パーティクル名")]
    [Sort(6)]
    public string ParticleName {
        get;
        set;
    }
    private Vector3 m_pos = Vector3.zero;
    [Category(PARTICLE_CATEGORY)]
    [DisplayName("パーティクル再生")]
    [Sort(7)]
    public void ParticlePlayCool() {
        if (m_pos == Vector3.zero) { 
            m_pos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        }
        Manager.ParticleManager.Instance.ParticlePlay(ParticleName, m_pos, Quaternion.identity, 2.0f);
    }
    #endregion
}
