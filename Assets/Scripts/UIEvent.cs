using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvent : MonoBehaviour
{
    public GameObject intro_Image;
    public GameObject escape_Image;
    public GameObject freedom_Image; 
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public GameObject hp_obj;
    public GameObject score_obj;
    public GameObject time_obj;


    void Start()
    {
        // 현재 켜져 있는 씬의 이름을 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 씬 이름에 따라 알맞은 코루틴을 실행
        if (currentSceneName == "EscapeScene")
        {
            StartCoroutine(Intro());
        }
        else if (currentSceneName == "FreedomScene")
        {
            StartCoroutine(Freedom());
        }
    }

    IEnumerator Intro()
    {
        intro_Image.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        intro_Image.SetActive(false);
    }

    public IEnumerator Escape()
    {
        escape_Image.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        escape_Image.SetActive(false);
    }

    IEnumerator Freedom()
    {
        freedom_Image.SetActive(true);

        // 시간을 분 초 하는 법 까먹어서 AI 도움 받았습니다. 
        // 1. 전체 시간을 분과 초로 계산
        int minutes = Mathf.FloorToInt(GameManager.instance.time_Count / 60f);
        int seconds = Mathf.FloorToInt(GameManager.instance.time_Count % 60f);

        // 2. 텍스트 적용 (문자열 보간 사용)
        scoreText.text = $"처치한 몬스터 숫자 : {GameManager.instance.kill_Count}";
        timeText.text = $"공략에 걸린 시간 : {minutes}분 {seconds}초";

        yield return new WaitForSeconds(5.0f);

        freedom_Image.SetActive(false);
        hp_obj.SetActive(true);
        score_obj.SetActive(true);
        time_obj.SetActive(true);
    }

    public void Set_PlayerHP()
    {
        hpText.text = "HP : " +  GameManager.instance.player_HP.ToString();
    }
}