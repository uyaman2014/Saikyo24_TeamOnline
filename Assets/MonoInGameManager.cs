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
    [SerializeField] GameObject[] WhiteCandles;
    [SerializeField] GameObject RedCandles;
    [SerializeField] GameObject ClickButton;
    [SerializeField] float SetsumeiTime;
    [SerializeField] float StartWaitTime;
    [SerializeField] float SmokeCountRatio;
    [SerializeField] float TimeLimit = 20;

    [SerializeField] Image RockSourceImage;
    [SerializeField] Sprite OddRockTexture;
    [SerializeField] Sprite EvenRockTexture;

    // Start is called before the first frame update
    void Start()
    {
        var pInfos = NetworkManager.Instance.PlayerInfos;
        GameParameterManager.Instance.TimeLimit = TimeLimit;
        StartCoroutine(SetUpTimer());
        int MaxCount = (int)(TimeLimit * 8 * pInfos.Count);
        GameParameterManager.Instance.TargetClickCount = Random.Range((int)(MaxCount * 0.8f), (int)(MaxCount));
        foreach (var item in TargetClickCountText)
        {
            item.text = GameParameterManager.Instance.TargetClickCount.ToString();
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
        for (int j = 0; j < WhiteCandles.Length; j++)
        {
            if((float)j / WhiteCandles.Length <= (float)countSum / GameParameterManager.Instance.TargetClickCount)
            {
                WhiteCandles[j].GetComponent<CandleFireManager>().FireCandle();
            }
        }
        if(countSum > GameParameterManager.Instance.TargetClickCount)
        {
            RedCandles.GetComponent<CandleFireManager>().FireCandle();
        }
        if(SmokeCountRatio < (float)countSum / GameParameterManager.Instance.TargetClickCount)
        {

        }
    }

    IEnumerator SetUpTimer()
    {
        ClickButton.SetActive(false);
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
        ClickButton.SetActive(true);

        while (TimeLimit > 0)
        {
            yield return new WaitForEndOfFrame();
            TimeLimit -= Time.deltaTime;
        }
        StartCoroutine(ResultTimer());
    }

    IEnumerator ResultTimer()
    {
        GameSequenceManager.Instance.GoToNextScene();
        yield return null;
    }
}
