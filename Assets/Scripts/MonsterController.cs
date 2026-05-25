using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public GameObject hitEffect;

    GameObject player;
    NavMeshAgent navMesh;
    Animator ani;
    int HP;
    bool isAttack = false;


    private SpawnManager myManager;
    private int mySpawnPointIndex;
    float distance;

    private void Start()
    {
        HP = 50;
        player = GameObject.Find("XR Origin (XR Rig)");
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        //navMesh.destination = player.transform.position; 플레이어 위치 찾는건데 이거는 나중에 인식 범위 내에 들경우 활성화 하는 시긍로 수정 할 예정
    }

    private void Update()
    {
        //distance = Vector3.Distance(player.transform.position, this.transform.position); 이것도 이따가 인식할 경우 계속 갱신하도록
        if (distance <= 2.0f) //거리가 가까워지면 스탑
        {
            navMesh.isStopped = true;
            if (isAttack == false)
            {
                ani.SetBool("Idle", true);
                StartCoroutine(Attack());
            }

        }
        else // 거리가 멀면 다시 이동 시작
        {
            ani.SetBool("Idle", false);

        }
    }

    IEnumerator Attack() // 이거 좀더 손보고
    {
        isAttack = true;
        yield return new WaitForSeconds(3.0f);

        ani.SetBool("Attack", true);
        yield return new WaitForSeconds(0.5f);

        player.GetComponent<PlayerController>().ApplyDamage(10);
        isAttack = false;
        ani.SetBool("Attack", true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            GameObject effect = Instantiate(hitEffect, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject); // 총알 제거
            Destroy(effect, 2.0f);
            HP -= 10;
            if (HP < 0f)
            {
                Destroy(gameObject);
                player.GetComponent<PlayerController>().ScoreUP(100);
            }

        }
    }


    // 스폰 매니저가 몬스터를 생성할 때 호출하여 정보를 전달해주는 함수
    public void Initialize(SpawnManager manager, int spawnIndex)
    {
        myManager = manager;
        mySpawnPointIndex = spawnIndex;
    }

    // 몬스터 체력이 0이 되어 죽거나 파괴될 때 실행되는 로직
    public void Die()
    {
        // 1. 매니저에게 내가 죽었다고 알림 (카운트 감소)
        if (myManager != null)
        {
            myManager.OnMonsterDied(mySpawnPointIndex);
        }

        // 2. 몬스터 오브젝트 파괴
        Destroy(gameObject);
    }
}