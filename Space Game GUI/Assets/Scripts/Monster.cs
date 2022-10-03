using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Monster : MonoBehaviour {
    /*
     * 공유 라이브러리
     *  (1) CTL_SEGMENT
     */
    [DllImport("SharedObject")] private static extern int CTL_SEGMENT(int num);

    [SerializeField] private int mDamage = 0;           // 몬스터 오브젝트의 공격력을 설정합니다.
    [SerializeField] private int mPoint = 0;            // 몬스터 처치 점수를 설정합니다.
    [SerializeField] private GameObject[] mItemPrefabs; // 몬스터에서 생성될 아이템 오브젝트 배열을 선언합니다.
    [SerializeField] private int[] mItemPercentage;     // 생성될 아이템 오브젝트의 확률을 선언합니다.
    private PlayerController mPlayerController;

    /*
     * void Awake()
     *  참조: 없음
     *  설명: 
     */
    private void Awake() {
        mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    /*
     * void OnTriggerEnter2D(Collider2D collision)
     *  참조: 없음
     *  설명: 1. 몬스터 오브젝트에 부딪힌 오브젝트의 태그를 분석합니다.
     *         (1) 오브젝트의 태그가 Player인 경우, 몬스터의 체력을 감소시키고 미사일 오브젝트를 삭제합니다.
     *         (2) Monster 스크립트의 MonsterDie 함수를 호출합니다.
     *         (3) PlayerHP 스크립트의 TakeDamage 함수를 호출합니다.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            MonsterDie();
            collision.GetComponent<PlayerHP>().TakeDamage(mDamage);
        }
    }
    
    /*
     * void MonsterDie()
     *  참조: Monster 스크립트, MosterHP 스크립트
     *  설명: 1. 몬스터가 사망하는 경우, 
     *         (1) 몬스터의 점수만큼 누적 점수를 증가시킵니다.
     *         (2) Monster 스크립트의 SpawnItem 함수를 호출합니다.
     *         (3) 몬스터 오브젝트를 삭제합니다.
     */
    public void MonsterDie() {
        mPlayerController.score += mPoint;
        CTL_SEGMENT(mPlayerController.score);
        SpawnItem();
        Destroy(gameObject);
    }

    /*
     * void SpawnItem()
     *  참조: Monster 스크립트
     *  설명: 1. 몬스터가 사망하고 아이템을 생성합니다.
     *         (1) 첫 번째 아이템의 확률 안에 있을 경우, 파워 업 아이템을 생성합니다. 
     *         (2) 두 번째 아이템의 확률 안에 있을 경우, 체력 업 아이템을 생성합니다.
     */
    private void SpawnItem() {
        int randomNumber = Random.Range(1, 100);
        if (randomNumber < mItemPercentage[0]) {
            Instantiate(mItemPrefabs[0], transform.position, Quaternion.identity);
        }
        else if (randomNumber < mItemPercentage[1]) {
            Instantiate(mItemPrefabs[1], transform.position, Quaternion.identity);
        }
    }
}
