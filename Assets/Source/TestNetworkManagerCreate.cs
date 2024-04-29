using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using R3;

public class TestNetworkManagerCreate : MonoBehaviour
{
    [SerializeField]
    TMP_Text[] ReadyText;
    [SerializeField]
    TMP_Text ScoreCounter;
    public Dictionary<string, bool> ReadyMap = new Dictionary<string, bool>();
    // Start is called before the first frame update
    void Start()
    {
        _ = NetworkManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        int countSum = 0;
        var playerInfo = NetworkManager.Instance.PlayerInfos;
        foreach (var item in ReadyText)
        {
            item.text = "NoPlayer";
        }
        foreach (var item in ReadyMap)
        {
        }
        foreach (var pInfo in playerInfo)
        {
            countSum += pInfo.Value.Count;
            ReadyText[i].text = pInfo.Value.bIsReady.ToString();
            i++;
        }
        ScoreCounter.text = countSum.ToString();
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.Close();
    }
}
