using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{
    [SerializeField] Text countdownText = null;
    [SerializeField] Button startButton = null;
    [SerializeField] GameObject titleScreen = null;
    [SerializeField]
    Text ResultText;

    void Start()
    {
        startButton.onClick.AddListener(()=>StartCoroutine(CountDown()));
        StageManager.Instance.OnGameEnded += ResetToTitle;
    }

    IEnumerator CountDown()
    {
        ResultText.gameObject.SetActive(false);
        titleScreen.SetActive(false);

        int count = 3;
        countdownText.text = count.ToString();

        while(count > 0) {
            
            yield return new WaitForSeconds(1);
            count--;
            countdownText.text = count.ToString();
        }

        countdownText.text = "";
        countdownText.enabled = false;
        StageManager.Instance.StartGame();
    }

    public void ResetToTitle()
    {
        titleScreen.SetActive(true);
        countdownText.enabled = true;
    }
}
