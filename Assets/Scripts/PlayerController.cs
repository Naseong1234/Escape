using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animation damageEffect;
    UIEvent UImanager;

    // 알림 반경 설정 
    float alertRadius = 12.0f; 


    void Start()
    {
        UImanager = GameObject.Find("UI Event").GetComponent<UIEvent>();
        UImanager.Set_PlayerHP();
    }

    void Update()
    {
        // 매 프레임마다 주변에 알림 뿌리기
        SendAlert(); 
    }
    // 여기부터
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
    // 여기까지 AI의 도움을 받았습니다.

    public void EmergencyEscape() // 나중에 탈출 부분때 이 함수 호출하면 될듯
    {
        alertRadius = 100.0f;
    }


    public void ApplyDamage(int damage)
    {
        damageEffect.Play();
        GameManager.instance.player_HP -= damage;
        UImanager.Set_PlayerHP();
        
        if (GameManager.instance.player_HP <= 0) 
        {
            SceneManager.LoadScene(0);
        }
    }

    
    // 씬 창에서 플레이어의 알림 반경을 붉은색 반투명 원으로 보여주는 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, alertRadius);
    }
    
}