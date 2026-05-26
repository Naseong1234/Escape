using UnityEngine;

public class SwordController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. 부위별 콜라이더(머리, 몸통)가 몬스터의 자식 오브젝트일 수 있으므로,
        // GetComponentInParent를 사용하여 최상위 부모에 있는 MonsterController를 찾습니다.
        MonsterController monster = other.GetComponentInParent<MonsterController>();

        if (monster != null)
        {
            // 2. 검이 부딪힌 정확한 위치를 계산 (히트 이펙트용)
            Vector3 hitPoint = other.ClosestPoint(transform.position);

            // 3. 충돌한 콜라이더의 태그를 확인하여 데미지를 다르게 적용합니다.
            if (other.CompareTag("Head"))
            {
                // 머리 타격 시 20 데미지
                monster.TakeDamage(20, hitPoint);
            }
            else if (other.CompareTag("Body"))
            {
                // 몸통 타격 시 10 데미지
                monster.TakeDamage(10, hitPoint);
            }
        }
    }
}