using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] TMP_Text curTime;
    [SerializeField] GameObject bestTime;

    private float surviveTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        surviveTime = 0f;
    }

    public void EndGameText() {
        bestTime.SetActive(true);
        TMP_Text bestScore = bestTime.transform.GetChild(1).GetComponent<TMP_Text>();

        if (bestScore != null) {
            float bestTime = PlayerPrefs.GetFloat("BestTime");
            int min = (int)bestTime / 60;
            int sec = (int)bestTime % 60;
            bestScore.text = string.Format("{0:D2} : {1:D2}", min, sec);
        }
    }

    public void SetBestTime() {
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime) {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
    }

    public void CalTime() {
        surviveTime += Time.deltaTime;
        int min = (int)surviveTime / 60;
        int sec = (int)surviveTime % 60;
        curTime.text = string.Format("{0:D2} : {1:D2}", min, sec);
    }

    public void SetDefault() {
        bestTime.SetActive(false);
        surviveTime = 0f;
    }
    // 시간 텍스트 변경
    // 게임 오버 시 베스트 기록 텍스트 출력
}
