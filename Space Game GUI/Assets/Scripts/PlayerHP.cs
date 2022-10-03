using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PlayerHP : MonoBehaviour {
    /*
     * 공유 라이브러리
     *  (1) CTL_LED
     */
    [DllImport("SharedObject")] private static extern int CTL_LED(int num);

    /*
     * 데이터 멤버
     *  (1) float   플레이어의 최대 체력을 선언합니다.
     *  (2) float   플레이어의 현재 체력을 선언합니다.
     *  (3) SpriteRenderer      
     *  (4) PlayerController    
     */
    [SerializeField] private float mMaxHP = 8.0f;
    private float mCurHP = 0.0f;
    private SpriteRenderer mSpriteRenderer;
    private PlayerController mPlayerController;

    public float maxHP => mMaxHP;

    public float curHP {
        set => mCurHP = Mathf.Clamp(value, 0, mMaxHP);
        get => mCurHP;
    }

    /*
     * void Awake()
     *  참조: 없음
     *  설명: 1. 
     */
    private void Awake() {
        mCurHP = mMaxHP;
        CTL_LED((int)mCurHP);
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mPlayerController = GetComponent<PlayerController>();
    }

    /*
     * void TakeDamage(int damage)
     *  참조: Monster 스크립트, MonsterProjectile 스크립트
     *  설명: 1. 플레이어 오브젝트가 피격당했을 경우
     *         (1) 플레이어의 현재 체력을 대미지만큼 감소시킵니다.
     *         (2) coroutineColor 코루틴을 실행합니다.
     *         (3) 플레이어의 체력이 0보다 작은 경우, PlayerController 스크립트의 PlayerDie 함수를 호출합니다.
     */
    public void TakeDamage(int damage) {
        // (1)
        mCurHP -= damage;
        CTL_LED((int)mCurHP);
        
        // (2)
        StopCoroutine("coroutineColor");
        StartCoroutine("coroutineColor");

        // (3)
        if (mCurHP <= 0) {
            mPlayerController.PlayerDie();
        }
    }

    /*
     * IEnumerator coroutineColor 코루틴
     *  참조: 없음
     *  설명: 1. 플레이어 오브젝트의 색을 빨간 색과 흰 색으로 번갈아가며 변경합니다.
     */
    private IEnumerator coroutineColor() {
        mSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mSpriteRenderer.color = Color.white;
    }
}
