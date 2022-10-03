using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum ItemType {
    POWER = 0,
    REPAIR = 1
}

public class Item : MonoBehaviour {
    /*
     * 공유 라이브러리
     *  (1) CTL_SEGMENT
     */
    [DllImport("SharedObject")] private static extern int CTL_SEGMENT(int num);
    [DllImport("SharedObject")] private static extern int CTL_LED(int num);

    [SerializeField] private ItemType mItemType;
    [SerializeField] private int mPoint = 0;

    private Move mMove;
    private PlayerController mPlayerController;

    /*
     * void Awake()
     *  참조: 없음
     *  설명: 1. 아이템 오브젝트의 속력과 방향을 설정합니다.
     *         (1) 아이템 오브젝트의 x, y 방향을 무작위로 설정합니다.
     *         (2) 설정한 방향대로 아이템 오브젝트를 이동시킵니다.
     */
    // 아이템 오브젝트의 방향을 설정합니다.
    private void Awake() {
        mMove = GetComponent<Move>();
        mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        mMove.MoveTo(new Vector3(x, y, 0));
    }

    /*
     * void OnTriggerEnter2D(Collider2D collision)
     *  참조: 
     *  설명: 1. 아이템 오브젝트에 부딪힌 오브젝트의 태그를 분석합니다.
     *         (1) 오브젝트의 태그가 Player인 경우, 아이템 점수만큼 누적 점수를 증가시킵니다.
     *         (2) UseItem 함수를 호출합니다.
     *         (3) 아이템 오브젝트를 삭제합니다.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            mPlayerController.score += mPoint;
            UseItem(collision.gameObject);
            Destroy(gameObject);
        }
    }

    /*
     * void UseItem(GameObject player)
     *  참조: 
     *  설명: 1. 아이템의 종류에 따라 효과를 각각 적용합니다. 
     *         (1) 파워 업 아이템인 경우, 플레이어 미사일의 레벨을 증가시킵니다.
     *         (2) 체력 업 아이템인 경우, 플레이어의 현재 체력을 2만큼 증가시킵니다.
     */
    public void UseItem(GameObject player) {
        CTL_SEGMENT(mPlayerController.score);
        switch (mItemType) {
            // (1)
            case ItemType.POWER:
                player.GetComponent<PlayerFire>().projectileLevel++;
                break;
            // (2)
            case ItemType.REPAIR:
                player.GetComponent<PlayerHP>().curHP += 2;
                CTL_LED((int)player.GetComponent<PlayerHP>().curHP);
                break;
        }
    }
}
