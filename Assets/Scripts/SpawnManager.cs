using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject monster;
    public float spawnTime = 4f;

    [HideInInspector]
    int max_monster_spawnpoint; // 최대 스폰 포인트 갯수
    int max_spawn; // 최대 스폰수

    // 외부에서 함부로 수정하지 못하도록 private으로 숨기고 인스펙터에서만 확인 가능하게(필요시)
    int current_spawn = 0;

    int Max_spawn_count = 3; // 위치당 최대 스폰수
    int[] current_spawn_count; // 위치당 현재 스폰수

    void Start()
    {
        if (gameObject.name == "Zombie Spawn Manager")
        {
            max_monster_spawnpoint = 8;
            max_spawn = 20;
        }
        else
        {
            max_monster_spawnpoint = 5;
            max_spawn = 10;
        }

        current_spawn_count = new int[max_monster_spawnpoint]; // 위치 인덱스 기록용

        for (int i = 0; i < current_spawn_count.Length; i++)
        {
            current_spawn_count[i] = 0;
        }

        StartCoroutine(MonsterSpawn());
    }

    IEnumerator MonsterSpawn()
    {
        while (true)
        {
            if (current_spawn < max_spawn)
            {
                int point = Random.Range(0, max_monster_spawnpoint);

                if (current_spawn_count[point] < Max_spawn_count)
                {
                    current_spawn_count[point]++;
                    current_spawn++;

                    // [추가된 부분] 스폰 포인트 기준 반경 1.0f 내에서 랜덤한 위치 계산
                    Vector2 randomCircle = Random.insideUnitCircle * 1.0f;
                    Vector3 randomOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
                    Vector3 finalSpawnPos = spawnPoint[point].position + randomOffset;

                    // 몬스터 생성 (랜덤 오프셋이 적용된 finalSpawnPos 사용)
                    GameObject newMonster = Instantiate(monster, finalSpawnPos, spawnPoint[point].rotation);

                    MonsterController mc = newMonster.GetComponent<MonsterController>();
                    if (mc != null)
                    {
                        mc.Initialize(this, point);
                    }

                    yield return new WaitForSeconds(spawnTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitUntil(() => current_spawn < max_spawn);
            }
        }
    }

    // 3. 몬스터가 죽었을 때 호출될 퍼블릭 함수
    public void OnMonsterDied(int pointIndex)
    {
        // 안전 장치: 카운트가 0 이하로 떨어지는 것을 방지
        if (current_spawn > 0) current_spawn--;
        if (current_spawn_count[pointIndex] > 0) current_spawn_count[pointIndex]--;
    }
}