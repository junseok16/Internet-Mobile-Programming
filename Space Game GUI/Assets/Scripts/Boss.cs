using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/*
 * 열거체
 *  (0) 보스가 맵 중앙으로 위치하는 패턴입니다.
 *  (1) 보스의 첫 번째 패턴입니다.
 *  (2) 보스의 두 번째 패턴입니다.
 *  (3) 보스의 세 번째 패턴입니다.
 *  (4) 보스의 네 번째 패턴입니다.
 */
public enum BossState {
    coroutineMoveToCenter = 0,
    coroutineFirstPhase = 1,
    coroutineSecondPhase = 2,
    coroutineThirdPhase = 3,
    coroutineFourthPhase = 4
}

public class Boss : MonoBehaviour {
    /*
     * 공유 라이브러리
     *  (1) CTL_SEGMENT
     */
    [DllImport("SharedObject")] private static extern int CTL_SEGMENT(int num);

    /*
     * 데이터 멤버
     *  (1) StageBorder 맵 경계 애셋을 선언합니다.
     *  (2) float       
     *  (3) Sprite[]      
     *  (4) int         
     */
    [SerializeField] private StageBorder mStageBorder;
    [SerializeField] private float mBossAppearPoint = 2.5f;
    [SerializeField] private Sprite[] mBossSprite;
    [SerializeField] private int mBossPoint = 5000;

    private BossState mBossState = BossState.coroutineMoveToCenter;
    private Move mMove;
    private BossFirePattern mBossFirePattern;
    private BossHP mBossHP;
    private SpriteRenderer mSpriteRenderer;
    private PlayerController mPlayerController;

    private void Awake() {
        mMove = GetComponent<Move>();
        mBossFirePattern = GetComponent<BossFirePattern>();
        mBossHP = GetComponent<BossHP>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<PlayerHP>().TakeDamage(8);
        }
    }

    public void ChangeState(BossState state) {
        StopCoroutine(mBossState.ToString());// 패턴을 종료합니다.
        mBossState = state;// 새로운 패턴으로 바꿉니다.
        StartCoroutine(mBossState.ToString());// 패턴을 시작합니다.
    }

    private IEnumerator coroutineMoveToCenter() {
        mMove.MoveTo(Vector3.down);// 방향을 아래로 설정합니다.
        while (true) {
            if (transform.position.y <= mBossAppearPoint) {
                mMove.MoveTo(Vector3.zero);// 방향을 0으로 설정합니다.
                ChangeState(BossState.coroutineFirstPhase);
            }
            yield return null;
        }
    }

    private IEnumerator coroutineFirstPhase() {
        mBossFirePattern.StartFire(FirePattern.ForwardFire);

        while (true) {
            if (mBossHP.curHP <= mBossHP.maxHP * 0.8f) {
                mSpriteRenderer.sprite = mBossSprite[1];
                mBossFirePattern.StopFire(FirePattern.ForwardFire);
                ChangeState(BossState.coroutineSecondPhase);
            }
            yield return null;
        }
    }

    /*
     * IEnumerator coroutineSecondPhase 코루틴
     *  참조: 없음
     *  설명: 1. 보스의 두 번째 패턴을 구현합니다.
     *         (1)
     *         (2) 
     *         (3) 
     *         (4) 보스의 현재 체력이 최대 체력의 60% 이하가 되면 이 패턴을 멈추고 세 번째 패턴으로 넘어갑니다.
     *         
     */
    private IEnumerator coroutineSecondPhase() {
        mBossFirePattern.StartFire(FirePattern.CircleFire);
        while (true) {
            if (mBossHP.curHP <= mBossHP.maxHP * 0.6f) {
                mSpriteRenderer.sprite = mBossSprite[2];
                mBossFirePattern.StopFire(FirePattern.CircleFire);
                ChangeState(BossState.coroutineThirdPhase);
            }
            yield return null;
        }
    }

    /*
     * IEnumerator coroutineThirdPhase 코루틴
     *  참조: 없음
     *  설명: 1. 보스의 세 번째 패턴을 구현합니다.
     *         (1) 
     *         (2) 보스를 오른쪽으로 이동시킵니다.
     *         (3) 보스가 맵 경계에 맞닿으면 반대 방향으로 이동합니다.
     *         (4) 보스의 현재 체력이 최대 체력의 40% 이하가 되면 이 패턴을 멈추고 네 번째 패턴으로 넘어갑니다.
     *         
     */
    private IEnumerator coroutineThirdPhase() {
        // (1)
        mBossFirePattern.StartFire(FirePattern.CurveFire);
        
        // (2)
        Vector3 direction = Vector3.right;
        mMove.MoveTo(direction);

        while (true) {
            // (3)
            if (transform.position.x <= mStageBorder.minBorder.x ||
                transform.position.x >= mStageBorder.maxBorder.x) {
                direction *= -1;
                mMove.MoveTo(direction);
            }

            // (4)
            if (mBossHP.curHP <= mBossHP.maxHP * 0.4f) {
                mSpriteRenderer.sprite = mBossSprite[3];
                mBossFirePattern.StopFire(FirePattern.CurveFire);
                ChangeState(BossState.coroutineFourthPhase);
            }
            yield return null;
        }
    }

    /*
     * IEnumerator coroutineFourthPhase 코루틴
     *  참조: 없음
     *  설명: 1. 보스의 네 번째 패턴을 구현합니다.
     */
    private IEnumerator coroutineFourthPhase() {
        mBossFirePattern.StartFire(FirePattern.CircleCurveFire);
        Vector3 direction = Vector3.right;
        mMove.MoveTo(direction);

        while (true) {
            if (transform.position.x <= mStageBorder.minBorder.x ||
                transform.position.x >= mStageBorder.maxBorder.x) {
                direction *= -1;
                mMove.MoveTo(direction);
            }

            if (mBossHP.curHP <= mBossHP.maxHP * 0.05f) {
                mSpriteRenderer.sprite = mBossSprite[4];
                mBossFirePattern.StopFire(FirePattern.CircleCurveFire);
            }
            yield return null;
        }
    }

    /*
     * void BossDie()
     *  참조: BossHP 스크립트
     *  설명: 1. 보스가 죽었을 때 
     *         (1) 보스 점수만큼 점수를 증가시킵니다.
     *         (2) 7 Segment에 획득한 점수를 표시합니다.
     *         (3) PlayerController 스크립트의 PlayerWin 함수를 호출합니다.
     *         (4) 보스 오브젝트를 삭제합니다.
     */
    public void BossDie() {
        mPlayerController.score += mBossPoint;
        CTL_SEGMENT(mPlayerController.score);
        mPlayerController.PlayerWin();
        Destroy(gameObject);
    }
}
