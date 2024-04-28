using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputManager : MonoBehaviour
{
    public void StartButton()
    {
        GameSequenceManager.Instance.GoToNextScene();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
