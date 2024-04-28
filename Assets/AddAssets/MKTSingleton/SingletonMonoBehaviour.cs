/**
 * @file    SingletonMonoBehaviour.cs
 * @brief   シングルトンクラス
 * @date    2021/01/29
 */
using UnityEngine;
using System;

namespace MKTSingleton {
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    // 1.探す
                    Type t = typeof(T);
                    instance = (T)FindObjectOfType(t);

                    // 2.なければ作る
                    if (instance == null) {
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        virtual protected void Awake() {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する。
            CheckInstance();
        }
        protected bool CheckInstance() {
            if (instance == null) {
                instance = this as T;
                return true;
            } else if (Instance == this) {
                return true;
            }
            Destroy(this);
            return false;
        }
        public static T getInstance {
            get {
                if (instance) {
                    return instance;
                }

                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);

                return instance;
            }
        }
        public static bool HasInstance {
            get { return instance != null; }
        }
    }
}