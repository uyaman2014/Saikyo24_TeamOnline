using UnityEngine;

namespace Kikukawa {
    public class Inochi_Anim : MonoBehaviour {
        public void OnSEPlay() {
            Manager.SEManager.Instance.SEPlay("New/ゲームオーバー_鎌");
        }
    }
}