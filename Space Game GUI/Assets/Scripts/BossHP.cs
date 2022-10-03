using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour {
    /*
     * 데이터 멤버
     *  (1) float           보스의 최대 체력을 선언합니다.
     *  (2) float           보스의 현재 체력을 선언합니다.
     *  (3) SpriteRenderer  스프라이트 렌더러 컴포넌트를 선언합니다.
     *  (4) Boss            Boss 스크립트 컴포넌트를 선언합니다.
     */
    [SerializeField] private float mMaxHP = 800.0f;
    private float mCurHP = 0.0f;
    private SpriteRenderer mSpriteRenderer;
    private Boss mBoss;

    public float maxHP => mMaxHP;
    public float curHP => mCurHP;

    /*
     * void Awake()
     *  참조: 없음
     *  설명: 1. 
     */
    private void Awake() {
        mCurHP = mMaxHP;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mBoss = GetComponent<Boss>();
    }

    /*
     * void TakeDamage(float damage)
     *  참조: PlayerProjectile 스크립트
     *  설명: 1. 보스가 대미지를 받았을 때
     *         (1) 보스의 현재 체력을 대미지만큼 감소시킵니다.
     *         (2) coroutineColor 코루틴을 실행합니다.
     *         (3) 보스의 현재 체력이 0보다 작은 경우, Boss 스크립트의 BossDie 함수를 호출합니다.
     */
    public void TakeDamage(float damage) {
        mCurHP -= damage;
        StopCoroutine("coroutineColor");
        StartCoroutine("coroutineColor");

        if (mCurHP <= 0) {
            mBoss.BossDie();
        }
    }

    /*
     * IEnumerator coroutineColor 코루틴
     *  참조: 없음
     *  설명: 1. 보스 오브젝트의 색을 회색과 흰 색으로 번갈아가며 변경합니다.
     */
    private IEnumerator coroutineColor() {
        mSpriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(0.05f);
        mSpriteRenderer.color = Color.white;
    }
}