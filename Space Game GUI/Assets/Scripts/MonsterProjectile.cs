using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    [SerializeField] private int mDamage = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터 미사일에 부딪힌 오브젝트의 태그가 Player일 경우
        if (collision.CompareTag("Player"))
        {
            // 플레이어의 체력을 감소시킵니다.
            collision.GetComponent<PlayerHP>().TakeDamage(mDamage);
            // 몬스터 미사일 오브젝트를 삭제합니다.
            Destroy(gameObject);
        }
    }
}
