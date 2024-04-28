/**
 * @file    SROption_Product.cs
 * @brief   SRDebugger��Option�g���N���X
 * @date    2024/04/28
 */
using UnityEngine;
using System;
using System.ComponentModel;

public partial class SROptions {
    #region �萔

    /// <summary>
    /// �S�ʃJ�e�S��
    /// </summary>
    private const string GENERAL_CATEGORY = "General";
    /// <summary>
    /// �T�E���h�J�e�S��
    /// </summary>
    private const string SOUND_CATEGORY = "Sound";
    /// <summary>
    /// �t�F�[�h�J�e�S��
    /// </summary>
    private const string FADE_CATEGORY = "Fade";
    /// <summary>
    /// �p�[�e�B�N���J�e�S��
    /// </summary>
    private const string PARTICLE_CATEGORY = "Particle";

    #endregion


    #region �f�o�b�O�@�\

    [Category(SOUND_CATEGORY)]
    [DisplayName("BGM�� �������ɐݒ肷��ꍇ�͋󕶎������Ă�������")]
    [Sort(0)]
    public string BGMName {
        get;
        set;
    }
    [Category(SOUND_CATEGORY)]
    [DisplayName("BGM�ύX")]
    [Sort(1)]
    public void FadeBGMChange() {
        Manager.BGMManager.Instance.FadeBGMChange(BGMName);
    }

    [Category(SOUND_CATEGORY)]
    [DisplayName("SE��")]
    [Sort(2)]
    public string SEName {
        get;
        set;
    }
    [Category(SOUND_CATEGORY)]
    [DisplayName("SE�Đ�")]
    [Sort(3)]
    public void SEPlayCool() {
        Manager.SEManager.Instance.SEPlay(SEName);
    }


    [Category(FADE_CATEGORY)]
    [DisplayName("�t�F�[�h�C������ ��������Ƀt�F�[�h�C���������O���Ă΂��")]
    [Sort(4)]
    public void FadeInCool() {
        Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
        Manager.FadeManager.Instance.SetFadeFlag(true,()=> {
            Debug.Log("�t�F�[�h�C�������I");
        });
    }
    [Category(FADE_CATEGORY)]
    [DisplayName("�t�F�[�h�A�E�g���� ��������Ƀt�F�[�h�A�E�g�������O���Ă΂��")]
    [Sort(5)]
    public void FadeOutCool() {
        Manager.FadeManager.Instance.SetFadeColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
        Manager.FadeManager.Instance.SetFadeFlag(false, () => {
            Debug.Log("�t�F�[�h�A�E�g�����I");
        });
    }


    [Category(PARTICLE_CATEGORY)]
    [DisplayName("�p�[�e�B�N����")]
    [Sort(6)]
    public string ParticleName {
        get;
        set;
    }
    private Vector3 m_pos = Vector3.zero;
    [Category(PARTICLE_CATEGORY)]
    [DisplayName("�p�[�e�B�N���Đ�")]
    [Sort(7)]
    public void ParticlePlayCool() {
        if (m_pos == Vector3.zero) { 
            m_pos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        }
        Manager.ParticleManager.Instance.ParticlePlay(ParticleName, m_pos, Quaternion.identity, 2.0f);
    }
    #endregion
}
