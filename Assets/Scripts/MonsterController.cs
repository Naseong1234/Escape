using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public ParticleSystem hitParticle;
    GameObject player;
    NavMeshAgent navMesh;
    Animator ani;
    BGMManager BGM_Manager;

    int HP;

    bool isAttack = false;
    bool isChasing = false; // 플레이어 알림을 받았는지 여부

    private SpawnManager myManager;
    private int mySpawnPointIndex;
    float distance;

    private void Start()
    {
        HP = 30;
        player = GameObject.Find("XR Origin (XR Rig)");
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        BGM_Manager = GameObject.Find("BGM Manager").GetComponent<BGMManager>();

        // 처음 스폰 시에는 알림을 받기 전이므로 멈추기
        navMesh.isStopped = true;
    }

    private void Update() // 플레이어의 위치기준에서 알림을 보내고, 몬스터가 이를 받을 경우에만 따라가도록 구현 -> AI의 도움을 받았습니다.
    {
        // 1. 알림을 받지 못했다면 아래 로직을 아예 실행X
        if (!isChasing) return;

        // 2. 알림을 받아 쫓는 상태일 때만 거리 갱신 및 이동 로직 실행
        if (player != null)
        {
            distance = Vector3.Distance(player.transform.position, this.transform.position);

            
            if (distance <= 2.0f) // 공격 사거리(2.0f) 이내
            {
                navMesh.isStopped = true;
                if (isAttack == false)
                {
                    
                    ani.SetBool("isWalking", false);
                    StartCoroutine(Attack());
                }
            }
            else // 공격 사거리 밖이면 플레이어를 향해 이동
            {
                navMesh.isStopped = false;
                navMesh.SetDestination(player.transform.position); // 목적지 계속 갱신
                ani.SetBool("isWalking", true);
            }
        }
    }

    // 플레이어의 SendAlert()에서 닿았을 때 호출되는 함수
    public void OnReceiveAlert(Vector3 targetPos)
    {
        if (!isChasing)
        {
            // 알림을 받는 순간 추적 모드 ON
            isChasing = true;
            navMesh.isStopped = false;
            if (gameObject.CompareTag("Zombie"))
            {
                BGM_Manager.MonsterSound_Play(0);
            }
            else if (gameObject.CompareTag("Skeleton"))
            {
                BGM_Manager.MonsterSound_Play(1);
            }
            else if (gameObject.CompareTag("Ghost"))
            {
                BGM_Manager.MonsterSound_Play(2);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        yield return new WaitForSeconds(2.0f);

        ani.SetBool("Attack", true);
        yield return new WaitForSeconds(0.5f);

        if (player != null)
        {
            player.GetComponent<PlayerController>().ApplyDamage(10);
        }

        isAttack = false;
        ani.SetBool("Attack", false);
    }

    public void TakeDamage(int damageAmount, Vector3 hitPosition)
    {
        if (hitParticle != null)
        {
            // 피 나오는 파티클을 피격 위치에 생성
            ParticleSystem effect = Instantiate(hitParticle, hitPosition, Quaternion.identity, this.transform);
            // 파티클 한 번 실행
            effect.Play();

            Destroy(effect.gameObject, effect.main.duration);
        }

        HP -= damageAmount;

        if (HP <= 0)
        {
            
            Die();
        }
    }

    public void Initialize(SpawnManager manager, int spawnIndex)
    {
        myManager = manager;
        mySpawnPointIndex = spawnIndex;
    }

    public void Die()
    {
        if (myManager != null)
        {
            myManager.OnMonsterDied(mySpawnPointIndex);
            GameManager.instance.killCount_UP();
        }

        Destroy(gameObject);
    }
}