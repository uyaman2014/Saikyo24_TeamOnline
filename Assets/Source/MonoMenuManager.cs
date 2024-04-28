using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoMenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] HiddenWhenOffline;
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
    }

    IEnumerator WaitNetworkConnection()
    {
        foreach (var item in HiddenWhenOffline)
        {
            item.SetActive(false);
        }
        while (!NetworkManager.Instance.IsOnline)
        {
            yield return new WaitForFixedUpdate();
        }
        foreach (var item in HiddenWhenOffline)
        {
            item.SetActive(true);
        }
    }
}
