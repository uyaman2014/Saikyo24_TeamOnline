using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBootManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // �e�V���O���g���쐬
        _ = NetworkManager.Instance;
        _ = GameSequenceManager.Instance;
        _ = InputManager.Instance;
        _ = ScoreManager.Instance;
        NetworkManager.Instance.ResetGame();
        GameSequenceManager.Instance.GoToNextScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
