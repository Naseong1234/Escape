using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    public ParticleSystem escapeDoor_Particle;
    BGMManager BGM_Manager;

    int core_Count = 0;
    int kill_Count = 0;
    public bool isEscape = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        BGM_Manager = GameObject.Find("BGM Manager").GetComponent<BGMManager>();
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEscape)
        {
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

        }

    }

    public void killCount_UP()
    {
        kill_Count++;
    }
}
