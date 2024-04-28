using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // �e�V���O���g���쐬
        _ = GameSequenceManager.Instance;
        _ = InputManager.Instance;
        _ = NetworkManager.Instance;
        _ = ScoreManager.Instance;

        GameSequenceManager.Instance.GoToNextScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
