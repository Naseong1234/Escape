using UnityEngine;

public class BGMManager : MonoBehaviour
{

    public AudioClip[] monster_sounds;
    public AudioClip[] BGM_sounds;
    AudioSource sound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound(int index)
    {
        sound.PlayOneShot(monster_sounds[index]);
    }
}
