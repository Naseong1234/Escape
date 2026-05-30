using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬의 이름을 가져오기 위해 반드시 추가해야 합니다!

public class UIEvent : MonoBehaviour
{


    // public을 붙여야 유니티 인스펙터 창에서 이미지를 연결할 수 있습니다.
    public GameObject intro_Image;
    public GameObject escape_Image;
    public GameObject freedom_Image; // FreedomScene용 이미지 변수 추가
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;


    void Start()
    {
        // 1. 현재 켜져 있는 씬의 이름을 가져옵니다.
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 2. 씬 이름에 따라 알맞은 코루틴을 실행합니다.
        if (currentSceneName == "EscapeScene")
        {
            StartCoroutine(Intro());
        }
        else if (currentSceneName == "FreedomScene")
        {
            StartCoroutine(Freedom());
        }
    }

    // Update is called once per frame
    void Update()
    {

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

    // FreedomScene에서 실행될 새로운 코루틴
    IEnumerator Freedom()
    {
        freedom_Image.SetActive(true);
        scoreText.text = "처치한 몬스터 숫자 : " + GameManager.instance.kill_Count.ToString();
        timeText.text = "공략에 걸린 시간 : " + GameManager.instance.time_Count.ToString("F2");

        yield return new WaitForSeconds(5.0f);
        freedom_Image.SetActive(false);
    }

    public void Set_PlayerHP()
    {
        hpText.text = "HP : " +  GameManager.instance.player_HP.ToString();
    }
}