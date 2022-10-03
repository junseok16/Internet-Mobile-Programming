using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
    /*
     * 데이터 멤버
     *  (1) Transform   배경 이미지의 트랜스폼을 선언합니다.
     *  (2) float       배경 이미지의 높이를 선언합니다.
     *  (3) float       배경 이미지의 속력을 선언합니다.
     *  (4) Vector3     배경 이비지의 방향을 선언합니다.
     */
    [SerializeField] private Transform mBackground = null;
    [SerializeField] private float mLength = 10.0f;
    [SerializeField] private float mSpeed = 0.5f;
    [SerializeField] private Vector3 mDirection = Vector3.down;

    /*
     * void Update()
     *  참조: 없음
     *  설명: 1. 배경 이미지를 무한하게 아래로 이동시킵니다.
     *         (1) 배경 이미지를 아래로 이동시킵니다.
     *         (2) 배경 이미지의 y 좌표가 이미지 높이보다 작아졌을 경우, 상대 배경 이미지의 위로 이동시킵니다.
     */
    private void Update() {
        transform.position += mDirection * mSpeed * Time.deltaTime;
        if (transform.position.y <= -mLength) {
            transform.position = mBackground.position + Vector3.up * mLength;
        }
    }
}