/*
 *   Name:MKTkakumei
 *   Script:モデルから孫や曾孫を検索し、 そのすべてに統一した先頭名を与える事が出来る拡張システム
 *   Day:2020/12/09
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MKTkakumei.Editor
{
    public class NameChangeALL : EditorWindow
    {
        private string m_AddName = "";

        //以下ウィンドウの中身--------------------------------------------------------------------------------------------------------------------------------
        /*変数定義
            @param	m_AddNameModel		[GameObject] 親モデルの格納用変数
            @param	m_AddName			[string] 追加文字列
        */
        private GameObject m_AddNameModel;

        private void OnGUI()
        {
            EditorGUILayout.LabelField("モデルの子全てに統一した先頭名を与えます。");
            EditorGUILayout.LabelField(" ");
            //WindowへのGameObject格納変数の表示定義
            m_AddNameModel = (GameObject)EditorGUILayout.ObjectField(new GUIContent("検索したい親モデル: ", "Your Model."),
                m_AddNameModel, typeof(GameObject), true);
            //Windowへの追加文字列変数の表示定義
            m_AddName = EditorGUILayout.TextField(m_AddName);

            //NameChangeStartボタンが押され、格納変数にモデルがあった時
            if (GUILayout.Button("NameChangeStart") && m_AddNameModel)
            {
                //子オブジェクトを全て収集
                var Childrens = m_AddNameModel.GetComponentsInChildren<Transform>();
                //子オブジェクト分繰り返す。
                for (var i = 0; i < Childrens.Length; i++)
                    //名前を入れる
                    Childrens[i].gameObject.name = m_AddName + Childrens[i].gameObject.name;

                //使用している値の初期化
                m_AddNameModel = null;
                m_AddName = "";
            }

            EditorGUILayout.LabelField("モデルの子全てに統一した後名を与えます。");
            //NameChangeStartボタンが押され、格納変数にモデルがあった時
            if (GUILayout.Button("NameChangeStart") && m_AddNameModel)
            {
                //子オブジェクトを全て収集
                var Childrens = m_AddNameModel.GetComponentsInChildren<Transform>();
                //子オブジェクト分繰り返す。
                for (var i = 0; i < Childrens.Length; i++)
                    //名前を入れる
                    Childrens[i].gameObject.name = Childrens[i].gameObject.name + m_AddName;

                //使用している値の初期化
                m_AddNameModel = null;
                m_AddName = "";
            }

            EditorGUILayout.LabelField(" ");
            EditorGUILayout.LabelField("簡単な文字列であれば、これで削除できます");
            //NameCatStartボタンが押され、格納変数にモデルがあった時
            if (GUILayout.Button("NameCatStart") && m_AddNameModel)
            {
                //子オブジェクトを全て収集
                var Childrens = m_AddNameModel.GetComponentsInChildren<Transform>();
                //子オブジェクト分繰り返す。
                for (var i = 0; i < Childrens.Length; i++)
                    //名前を削除
                    Childrens[i].gameObject.name = Childrens[i].gameObject.name.Replace(m_AddName, "");

                //使用している値の初期化
                m_AddNameModel = null;
                m_AddName = "";
            }
        }

        [MenuItem("MKTkakumeiEditor/NameChangeALL")]
        private static void ShowWindow()
        {
            // ウィンドウを表示
            GetWindow<NameChangeALL>();
        }
    }
}