using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    // [수정됨] GameObject 대신 ParticleSystem을 직접 받습니다.
    public ParticleSystem hitParticle;
    GameObject player;
    NavMeshAgent navMesh;
    Animator ani;
    BGMManager BGM_Manager;
    GameManager gameManager;

    int HP;

    bool isAttack = false;
    bool isChasing = false; // 플레이어 알림을 받았는지 여부

    private SpawnManager myManager;
    private int mySpawnPointIndex;
    float distance;

    private void Start()
    {
        HP = 30;
        // XR 환경의 플레이어를 찾습니다.
        player = GameObject.Find("XR Origin (XR Rig)");
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        BGM_Manager = GameObject.Find("BGM Manager").GetComponent<BGMManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // 처음 스폰 시에는 알림을 받기 전이므로 멈춰둡니다.
        navMesh.isStopped = true;
    }

    private void Update()
    {
        // 1. 알림을 받지 못했다면 아래 로직(거리 계산 등)을 아예 실행하지 않습니다.
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
                    if (gameObject.name == "Zombie")
                    {
                        BGM_Manager.MonsterSound_Play(0);
                    }
                    else if(gameObject.name == "Skeleton")
                    {
                        BGM_Manager.MonsterSound_Play(1);
                    }
                    else if (gameObject.name == "Ghost")
                    {
                        BGM_Manager.MonsterSound_Play(2);
                    }
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
        // 주의: 공격이 끝난 후에는 애니메이션을 false로 꺼주어야 다음 동작이 꼬이지 않습니다.
        ani.SetBool("Attack", false);
    }

    // ==========================================
    // [수정됨] 기존 OnTriggerEnter를 삭제하고, 외부에서 호출하는 TakeDamage 함수 생성
    // ==========================================
    public void TakeDamage(int damageAmount, Vector3 hitPosition)
    {
        // 1. 파티클 이펙트 생성 및 실행
        if (hitParticle != null)
        {
            // 파티클을 피격 위치에 생성
            ParticleSystem effect = Instantiate(hitParticle, hitPosition, Quaternion.identity, this.transform);
            // 파티클 한 번 실행
            effect.Play();

            // 2.0f 처럼 고정된 시간이 아니라, 해당 파티클의 실제 재생 길이(duration)만큼 기다렸다가 파괴
            Destroy(effect.gameObject, effect.main.duration);
        }

        // 2. 데미지 적용
        HP -= damageAmount;

        // 3. 체력이 0 이하가 되면 사망 처리
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
            gameManager.killCount_UP();
        }

        Destroy(gameObject);
    }
}