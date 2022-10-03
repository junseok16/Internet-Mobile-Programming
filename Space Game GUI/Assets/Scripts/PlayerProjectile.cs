using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [SerializeField] private int mDamage = 0;

    /*
     * void OnTriggerEnter2D(Collider2D collision)
     *  참조: 없음
     *  설명: 1. 플레이어의 미사일 오브젝트에 부딪힌 오브젝트의 태그를 분석합니다.
     *         (1) 오브젝트의 태그가 Monster인 경우, 몬스터의 체력을 감소시키고 미사일 오브젝트를 삭제합니다.
     *         (2) 오브젝트의 태그가 Boss인 경우, 보스의 체력을 감소시키고 미사일 오브젝트를 삭제합니다.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        // (1)
        if (collision.CompareTag("Monster")) {
            collision.GetComponent<MonsterHP>().TakeDamage(mDamage);
            Destroy(gameObject);
        }
        // (2)
        else if (collision.CompareTag("Boss")) {
            collision.GetComponent<BossHP>().TakeDamage(mDamage);
            Destroy(gameObject);
        }
    }
}
