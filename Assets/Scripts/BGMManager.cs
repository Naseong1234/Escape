using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] monster_sounds;
    public AudioClip[] BGM_sounds;

    // BGM용과 효과음(몬스터)용 오디오 소스를 따로 관리합니다.
    private AudioSource monsterSource;
    private AudioSource bgmSource;

    void Start()
    {
        // 코드에서 자동으로 AudioSource 컴포넌트 2개를 생성하고 붙여줍니다.
        // 이렇게 하면 유니티 인스펙터에서 매번 수동으로 세팅할 필요가 없어 편합니다.
        bgmSource = gameObject.AddComponent<AudioSource>();
        monsterSource = gameObject.AddComponent<AudioSource>();

        // BGM은 보통 무한 반복(Loop)이 필요하므로 true로 켜줍니다.
        bgmSource.loop = true;

        // 게임 시작 시 1번 BGM 재생

        // 1. 현재 켜져 있는 씬의 이름을 가져옵니다.
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 2. 씬 이름에 따라 알맞은 코루틴을 실행합니다.
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