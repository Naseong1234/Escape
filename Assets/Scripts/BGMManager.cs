using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] monster_sounds;
    public AudioClip[] BGM_sounds;

    private AudioSource monsterSource;
    private AudioSource bgmSource;


    //사운드에서 BGM -> 겹침x, 몬스터 -> 겹침o 하도록 하는법이 헷갈려서 AI의 도움을 받았습니다.
    void Start()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        monsterSource = gameObject.AddComponent<AudioSource>();

        // BGM은 보통 무한 반복(Loop)이 필요하므로 true로 켜줍니다.
        bgmSource.loop = true;

        // 게임 시작 시 1번 BGM 재생

        // 현재 켜져 있는 씬의 이름가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        //씬 이름에 따라 알맞은 코루틴을 실행
        if (currentSceneName == "EscapeScene")
        {
            BGMSound_Play(0);
        }
        else if (currentSceneName == "FreedomScene")
        {
            BGMSound_Play(2);
        }
    }

    public void MonsterSound_Play(int index)
    {
        // 효과음은 겹쳐서 나야 하므로 PlayOneShot을 사용합니다.
        monsterSource.PlayOneShot(monster_sounds[index], 2);
    }

    public void BGMSound_Play(int index)
    {
        // BGM은 기존 소리가 꺼지고 안 겹쳐야 합니다.
        // 메인 트랙(clip)을 갈아끼우고 Play()를 호출하면 이전 소리가 멈추고 새 소리가 납니다.
        bgmSource.clip = BGM_sounds[index];
        bgmSource.Play();
    }
}