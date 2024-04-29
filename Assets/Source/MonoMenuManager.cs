using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonoMenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] HiddenWhenOffline;
    [SerializeField] TMP_Text StatusText;
    [SerializeField] GameObject StartButton;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitNetworkConnection());
    }

    // Update is called once per frame
    void Update()
    {
        if(!NetworkManager.Instance.IsOnline)
        {
            foreach (var item in HiddenWhenOffline)
            {
                item.SetActive(false);
            }
        }
        else
        {
            if(!NetworkManager.Instance.IsJoinedRoom)
            {
                StartButton.SetActive(false);
                StatusText.text = "Not Joined Room";
            }
            else
            {
                StartButton.SetActive(true);
                StatusText.text = "Room Player Count:" + NetworkManager.Instance.PlayerInfos.Count;
            }
        }
    }

    IEnumerator WaitNetworkConnection()
    {
        foreach (var item in HiddenWhenOffline)
        {
            item.SetActive(false);
            StatusText.text = "No Connection";
        }
        while (!NetworkManager.Instance.IsOnline)
        {
            yield return new WaitForFixedUpdate();
        }
        foreach (var item in HiddenWhenOffline)
        {
            item.SetActive(true);
            StatusText.text = "Connected";
        }
    }
}
