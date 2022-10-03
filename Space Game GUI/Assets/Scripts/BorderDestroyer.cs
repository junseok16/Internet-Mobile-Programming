using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderDestroyer : MonoBehaviour {
    /*
     * 데이터 멤버
     *  (1) StageBorder 맵 경계 애셋을 선언합니다.
     *  (2) float       맵 경계에 적당한 여유 공간을 선언합니다.
     */
    [SerializeField] private StageBorder mStageBorder;
    private float mOffset = 3.0f;

    /*
     * void LateUpdate()
     *  참조: 없음
     *  설명: 1. 오브젝트가 맵 경계를 벗어난 몬스터 오브젝트를 삭제합니다.
     *         (1) 맵 경계에 밖으로 벗어난 몬스터 오브젝트를 삭제합니다.
     */
    private void LateUpdate() {
        if (transform.position.y < mStageBorder.minBorder.y - mOffset ||
            transform.position.y > mStageBorder.maxBorder.y + mOffset ||
            transform.position.x < mStageBorder.minBorder.x - mOffset ||
            transform.position.x > mStageBorder.maxBorder.x + mOffset) {
            Destroy(gameObject);
        }
    }
}