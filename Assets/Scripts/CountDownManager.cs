using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{
    [SerializeField] Text countdownText = null;
    [SerializeField] Button startButton = null;
    [SerializeField] GameObject titleScreen = null;

    void Start()
    {
        startButton.onClick.AddListener(()=>StartCoroutine(CountDown()));
    }

    IEnumerator CountDown()
    {
        titleScreen.SetActive(false);

        int count = 3;
        countdownText.text = count.ToString();

        while(count > 0) {
            
            yield return new WaitForSeconds(1);
            countdownText.text = count.ToString();
            count--;
        }

        countdownText.enabled = false;
    }
}
