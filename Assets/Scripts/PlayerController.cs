using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Animation damageEffect;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;

    // 알림 반경 설정 (기본 2)
    public float alertRadius = 2.0f; 

    int HP;
    int score;

    void Start()
    {
        HP = 100;
        score = 0;
        scoreText.text = "Score:" + score;
        hpText.text = "HP: " + HP;
    }

    void Update()
    {
        // 매 프레임 주변에 알림을 뿌립니다.
        SendAlert(); 
    }

    void SendAlert()
    {
        // 플레이어 위치를 기준으로 alertRadius 반경 내에 있는 모든 콜라이더를 찾습니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius);
        
        foreach (Collider col in colliders)
        {
            // 범위 내에 들어온 오브젝트가 몬스터인지 확인합니다.
            MonsterController monster = col.GetComponent<MonsterController>();
            if (monster != null)
            {
                // 몬스터에게 플레이어의 현재 위치를 전달하며 알림을 보냅니다.
                monster.OnReceiveAlert(transform.position); 
            }
        }
    }

    // 긴급 탈출 스킬 등을 사용할 때 이 함수를 호출하면 범위가 10으로 늘어납니다.
    public void EmergencyEscape() // 나중에 탈출 부분때 이 함수 호출하면 될듯
    {
        alertRadius = 10.0f;
    }


    public void ApplyDamage(int damage)
    {
        damageEffect.Play();
        HP -= damage;
        hpText.text = "HP: " + HP;
        
        // HP가 0과 같을 때도 죽도록 수정 (<=)
        if (HP <= 0) 
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ScoreUP(int score)
    {
        this.score += score;
        scoreText.text = "Score:" + this.score;
    }

    // Unity 에디터 씬(Scene) 창에서 플레이어의 알림 반경을 붉은색 반투명 원으로 보여줍니다.
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, alertRadius);
    }
}