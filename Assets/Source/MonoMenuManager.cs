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
            var pInfos = NetworkManager.Instance.PlayerInfos;
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
            bool bIsAllReady = true;
            if (pInfos.Count <= 0) 
                bIsAllReady = false;
            foreach (var pInfo in pInfos)
            {
                bIsAllReady = bIsAllReady && pInfo.Value.bIsReady;
                if (pInfo.Value.bIsSelf && pInfo.Value.bIsReady)
                {
                    StatusText.text = "Waiting Other Player";
                }
            }
            if (bIsAllReady)
            {
                GameSequenceManager.Instance.GoToNextScene();
                Destroy(this);
            }
        }
    }

    IEnumerator WaitNetworkConnection()
    {
        var startTime = Time.time;
        foreach (var item in HiddenWhenOffline)
        {
            item.SetActive(false);
            StatusText.text = "No Connection";
        }
        while (!NetworkManager.Instance.IsOnline)
        {
            if (Time.time - startTime > 5)
            {
                startTime = Time.time;
                NetworkManager.Instance.Open();
            }
            yield return new WaitForFixedUpdate();
        }
        foreach (var item in HiddenWhenOffline)
        {
            item.SetActive(true);
            StatusText.text = "Connected";
        }
    }
}
