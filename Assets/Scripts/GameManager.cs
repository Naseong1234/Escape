using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    public ParticleSystem escapeDoor_Particle;
    GameObject escape_Door;

    int core_Count = 0;
    bool isEscape = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Core_Destruction_Count()
    {
        core_Count++;
    }

    void EscapeLogic()
    {
        if (core_Count >= 2)
        {
            isEscape = true;
            core_Count = 0;
        }

        if (isEscape)
        {
            player.EmergencyEscape();
            escapeDoor_Particle.Play();
        }
    }


}
