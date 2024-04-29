using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonoInGameManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] OddScoreText;
    [SerializeField] TMP_Text[] EvenScoreText;
    [SerializeField] TMP_Text[] TargetClickCountText;
    [SerializeField] TMP_Text PlayerCountText;
    [SerializeField] TMP_Text CurrentClickCountText;

    [SerializeField] GameObject Hukidasi_Setsumei;
    [SerializeField] GameObject Hukidasi_Start;
    [SerializeField] GameObject Hukidasi_Info;
    [SerializeField] float SetsumeiTime;
    [SerializeField] float StartWaitTime;
    public float TimeLimit = 20;
    public int TargetClickCount = 0;

    [SerializeField] Image RockSourceImage;
    [SerializeField] Sprite OddRockTexture;
    [SerializeField] Sprite EvenRockTexture;

    // Start is called before the first frame update
    void Start()
    {
        var pInfos = NetworkManager.Instance.PlayerInfos;
        StartCoroutine(SetUpTimer());
        int MaxCount = (int)(TimeLimit * 10 * pInfos.Count);
        TargetClickCount = Random.Range((int)(MaxCount * 0.8f), (int)(MaxCount));
        foreach (var item in TargetClickCountText)
        {
            item.text = TargetClickCount.ToString();
        }

        for (int i = 0; i < pInfos.Count; i++)
        {
            if (pInfos.Count % 2 == 0)
            {
                EvenScoreText[i].gameObject.SetActive(true);
                RockSourceImage.sprite = EvenRockTexture;
            }
            else
            {
                OddScoreText[i].gameObject.SetActive(true);
                RockSourceImage.sprite = OddRockTexture;
            }
        }
        PlayerCountText.text = pInfos.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        var pInfos = NetworkManager.Instance.PlayerInfos;
        int countSum = 0;
        foreach (var pinfo in pInfos)
        {
            countSum += pinfo.Value.Count;
        }
        int i = 0;
        foreach (var pinfo in pInfos)
        {
            if (pInfos.Count % 2 == 0)
            {
                EvenScoreText[i].text = pinfo.Value.Count.ToString();
            }
            else
            {
                OddScoreText[i].text = pinfo.Value.Count.ToString();
            }
            i++;
        }

        CurrentClickCountText.text = countSum.ToString();
    }

    IEnumerator SetUpTimer()
    {
        Hukidasi_Setsumei.SetActive(true);
        Hukidasi_Start.SetActive(false);
        Hukidasi_Info.SetActive(false);
        yield return new WaitForSeconds(SetsumeiTime);
        Hukidasi_Setsumei.SetActive(false);
        Hukidasi_Start.SetActive(true);
        Hukidasi_Info.SetActive(false);
        yield return new WaitForSeconds(StartWaitTime);
        Hukidasi_Setsumei.SetActive(false);
        Hukidasi_Start.SetActive(false);
        Hukidasi_Info.SetActive(true);
        StartCoroutine(InGameTimer());
    }

    IEnumerator InGameTimer()
    {
        while(TimeLimit > 0)
        {
            yield return new WaitForEndOfFrame();
            TimeLimit -= Time.deltaTime;
        }
        StartCoroutine(ResultTimer());
    }

    IEnumerator ResultTimer()
    {
        yield return null;
    }
}
