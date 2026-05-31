using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public ParticleSystem escapeDoor_Particle;
    PlayerController player;
    BGMManager BGM_Manager;
    UIEvent UIEvent;

    int core_Count = 0;
    public int player_HP = 100;
    public int kill_Count = 0;
    public float time_Count = 0f;
    public bool isEscape = false;

    public static GameManager instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 넘어가도 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        player = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        BGM_Manager = GameObject.Find("BGM Manager").GetComponent<BGMManager>();
        UIEvent = GameObject.Find("UI Event").GetComponent<UIEvent>();
        // 게임 시작될때 light Eye로 자동 설정하도록 하고 싶어서 AI의 도움을 받았습니다.
        // VR 기기가 켜질 때까지 기다리는 코루틴을 실행합니다.
        StartCoroutine(SetRightEyeDelay());

    }

    void Update()
    {
        // 탈출하지 않은 상태일 때만 매 프레임 시간을 더하고, 탈출 로직을 체크
        if (!isEscape)
        {
            time_Count += Time.deltaTime; 
            EscapeLogic();
        }
    }

    public void Core_Destruction_Count()
    {
        core_Count += 1;
    }

    void EscapeLogic()
    {
        if (core_Count >= 3)
        {
            isEscape = true;
            player.EmergencyEscape();
            escapeDoor_Particle.Play();
            BGM_Manager.BGMSound_Play(1);

            UIEvent.StartCoroutine(UIEvent.Escape());
        }
    }

    public void killCount_UP()
    {
        kill_Count++;
    }

    IEnumerator SetRightEyeDelay()
    {
        // 1. XR 기기가 완전히 인식되고 활성화될 때까지 매 프레임 대기합니다.
        yield return new WaitUntil(() => XRSettings.isDeviceActive);

        // 2. 활성화가 완료된 직후에 모니터 송출을 오른쪽 눈으로 강제 변경합니다.
        XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;
    }
}
