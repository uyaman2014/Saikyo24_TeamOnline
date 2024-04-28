/**
 * @file    ConfigBinarySaveManager.cs
 * @brief   ゲーム設定のバイナリデータ保存管理クラス
 * @date    2021/08/30
 */
using MKTSingleton;
using UnityEngine;
using System.IO;

namespace Manager {
    public class ConfigBinarySaveManager : SingletonMonoBehaviour<ConfigBinarySaveManager> {
        [Header("BGMの大きさ")]
        [SerializeField] float m_BGMVolume      = 1.0f;
        public           float getBGMVolume      { get { return m_BGMVolume; } }
        [Header("SEの大きさ")]
        [SerializeField] float m_SEVolume       = 0.5f;
        public           float getSEVolume       { get { return m_SEVolume; } }
        [Header("フルスクリーンフラグ(0がウィンドウ1がフルスクリーン)")]
        [SerializeField] int   m_FullScreenFlag = 0;
        public           int   getFullScreenFlag { get { return m_FullScreenFlag; } }
        [Header("画面の横幅アスペクト比")]
        [SerializeField] int   m_AspectXNum     = 1024;
        public           int   getAspectXNum     { get { return m_AspectXNum; } }
        [Header("画面の縦幅アスペクト比")]
        [SerializeField] int   m_AspectYNum     = 576;
        public           int   getAspectYNum     { get { return m_AspectYNum; } }
        void Start() {
            DontDestroyOnLoad(gameObject);
            Load();
        }
        /*void Update() {
            //スクリーンの設定は、毎フレーム行う
            if (getFullScreenFlag == 0) {
                Screen.SetResolution(getAspectXNum, getAspectYNum, false);
            } else {
                Screen.SetResolution(getAspectXNum, getAspectYNum, true);
            }
        }*/
        /// <summary>
        /// 初期状態のセーブの処理関数
        /// </summary>
        private void DefaultSave() {
            //ファイルからの読み込み                                    //読み込み名　      //なかったら作る　//読み書き手段
            FileStream BinaryFile = new FileStream(Application.persistentDataPath + "/ConfigData.txt", FileMode.Create, FileAccess.ReadWrite);
            //ファイルへの書き込みを可能にする処理
            BinaryWriter Writer = new BinaryWriter(BinaryFile);

            //BGMの大きさのセーブ(double型)
            m_BGMVolume = 1.0f;
            Writer.Write((double)m_BGMVolume);
            //SEの大きさの変数のセーブ(double型)
            m_SEVolume = 0.5f;
            Writer.Write((double)m_SEVolume);

            //フルスクリーンかを取得する変数のセーブ(int型)
            m_FullScreenFlag = 0;
            Writer.Write(m_FullScreenFlag);
            //横幅のアスペクト比のセーブ(int型)
            m_AspectXNum = 1024;
            Writer.Write(m_AspectXNum);
            //縦幅のアスペクト比のセーブ(int型)
            m_AspectYNum = 576;
            Writer.Write(m_AspectYNum);

            //ファイル閉じる命令。
            BinaryFile.Close();
        }

        /// <summary>
        /// セーブ処理関数
        /// <param name="bgmvolume">      BGMの音量</param>
        /// <param name="sevolume">       SEの音量</param>
        /// <param name="fullscreenflag"> フルスクリーンにするかどうかの整数フラグ(0がウィンドウモード)</param>
        /// <param name="aspectxnum">     画面の横幅アスペクト比</param>
        /// <param name="aspectynum">     画面の縦幅アスペクト比</param>
        /// </summary>
        public void Save(float bgmvolume, float sevolume,int fullscreenflag, int aspectxnum, int aspectynum) {
            m_BGMVolume      = bgmvolume;
            m_SEVolume       = sevolume;
            m_FullScreenFlag = fullscreenflag;
            m_AspectXNum     = aspectxnum;
            m_AspectYNum     = aspectynum;

            //ファイルからの読み込み                                    //読み込み名　      //なかったら作る　//読み書き手段
            FileStream   BinaryFile = new FileStream(Application.persistentDataPath + "/ConfigData.txt", FileMode.Create, FileAccess.ReadWrite);
            //ファイルへの書き込みを可能にする処理
            BinaryWriter Writer     = new BinaryWriter(BinaryFile);

            //BGMの大きさのセーブ(double型)
            Writer.Write((double)m_BGMVolume);
            //SEの大きさの変数のセーブ(double型)
            Writer.Write((double)m_SEVolume);

            //フルスクリーンかを取得する変数のセーブ(int型)
            Writer.Write(m_FullScreenFlag);
            //横幅のアスペクト比のセーブ(int型)
            Writer.Write(m_AspectXNum);
            //縦幅のアスペクト比のセーブ(int型)
            Writer.Write(m_AspectYNum);

            //ファイル閉じる命令。
            BinaryFile.Close();
        }
        /// <summary>
        /// ロード処理関数
        /// </summary>
        public void Load() {
            //ファイルからの読み込み                                        //読み込み名　   //開く       　//読み書き手段
            FileStream BinaryFile = new FileStream(Application.persistentDataPath + "/ConfigData.txt", FileMode.OpenOrCreate, FileAccess.Read);
            if (BinaryFile != null) {
                //ファイルからの読み込みを可能にする処理
                BinaryReader Reader = new BinaryReader(BinaryFile);
                //読み込み位置
                BinaryFile.Seek(0, SeekOrigin.Begin);

                //ファイルを新規作成した場合
                if (Reader.BaseStream.Length <= 0) {
                    BinaryFile.Close();
                    DefaultSave();
                    return;
                }

                //BGMの大きさのセーブ(double型)
                m_BGMVolume      = (float)Reader.ReadDouble();
                //SEの大きさの変数のセーブ(double型)
                m_SEVolume       = (float)Reader.ReadDouble();

                //フルスクリーンかを取得する変数の読み出し(int型)
                m_FullScreenFlag = Reader.ReadInt32();
                //横幅のアスペクト比の読み出し(int型)
                m_AspectXNum     = Reader.ReadInt32();
                //縦幅のアスペクト比の読み出し(int型)
                m_AspectYNum     = Reader.ReadInt32();

                //ファイル閉じる命令。
                BinaryFile.Close();
            }
        }
    }
}