using UnityEngine;

namespace Kikukawa {
    public class Inochi_Anim : MonoBehaviour {
        public void OnSEPlay() {
            Manager.SEManager.Instance.SEPlay("New/�Q�[���I�[�o�[_��");
        }
        public void OnResultBGMPlay() { 
            Manager.BGMManager.Instance.FadeBGMChange("Result2");
        }
    }
}