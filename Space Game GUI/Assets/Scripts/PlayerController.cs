using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour {
    /*
     * 공유 라이브러리
     *  (1) CTL_PIEZO
     *  (2) CTL_DOT
     */
    [DllImport("SharedObject")] private static extern int CTL_PIEZO(int music);
    [DllImport("SharedObject")] private static extern int CTL_DOT(int num);

    /*
     * 
     */
    [SerializeField] private float mSpeed = 0.01f;
    [SerializeField] private StageBorder mStageBorder;
    [SerializeField] private GameObject mYouLoseWindow = null;
    [SerializeField] private GameObject mYouWinWindow = null;
    [SerializeField] private TextMeshProUGUI mLoseScoreText;
    [SerializeField] private TextMeshProUGUI mLoseRecordText;
    [SerializeField] private TextMeshProUGUI mWinScoreText;
    [SerializeField] private TextMeshProUGUI mWinRecordText;

    private Touch mTouch;    // 모바일 터치를 선언합니다.
    private Move mMove;
    private int mScore = 0;    // 점수로 사용할 mScore 변수를 선언합니다.
    private int mRecord = 0;

    /*
     * 프로퍼티
     *  참조:
     *  설명: 1. 프로퍼티로 누적 점수와 최고 점수에 접근할 수 있도록 합니다.
     */
    public int score {
        set => mScore = Mathf.Max(0, value);
        get => mScore;
    }

    public int record {
        set => mRecord = Mathf.Max(0, value);
        get => mRecord;
    }

    private void Awake() {
        mMove = GetComponent<Move>();
    }

    /*
     * void Update()
     *  참조: 없음
     *  설명: 1. 플레이어 오브젝트를 터치로 이동시킵니다.
     */
    private void Update() {
        if (Input.touchCount > 0) {
            mTouch = Input.GetTouch(0);
            if (mTouch.phase == TouchPhase.Moved) {
                transform.position = new Vector3(transform.position.x + mTouch.deltaPosition.x * mSpeed, transform.position.y + mTouch.deltaPosition.y * mSpeed, 0);
            }
        }

        // 키보드로 Move 컴포넌트의 MoveToward 함수를 호출하여 플레이어 오브젝트의 방향을 재설정합니다.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        mMove.MoveTo(new Vector3(x, y, 0)); 
    }

    /*
     * void LateUpdate()
     *  참조: 없음
     *  설명: 1. 플레이어가 맵 경계 밖으로 넘어가지 못하도록 합니다.
     */
    private void LateUpdate() {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mStageBorder.minBorder.x, mStageBorder.maxBorder.x), 
                                         Mathf.Clamp(transform.position.y, mStageBorder.minBorder.y, mStageBorder.maxBorder.y));
    }

    /*
     * void PlayerDie()
     *  참조: PlayerHP 스크립트
     *  설명: 1.
     *          (1) 최종 점수가 최고 점수보다 큰 경우, 최고 점수를 변경합니다.
     *          (2) You Lose UI를 활성화하여 눈에 보이도록 합니다.
     *          (3) 최종 점수와 최고 점수를 텍스트 UI에 출력합니다.
     *          (4) 게임이 진행되지 않도록 시간을 멈춥니다.
     *          (5) 플레이어 오브젝트를 삭제합니다.
     *          (6) 
     */
    public void PlayerDie() {
        // (1)
        string sceneName = SceneManager.GetActiveScene().name;
        if (mScore > PlayerPrefs.GetInt(sceneName + " Record")) {
            PlayerPrefs.SetInt(sceneName + " Record", mScore);
        }
        
        // (2)
        mYouLoseWindow.SetActive(true);

        // (3)
        mLoseScoreText.text = mScore.ToString();
        mLoseRecordText.text = PlayerPrefs.GetInt(sceneName + " Record").ToString();

        // (4)
        Time.timeScale = 0.0f;
        
        // (5)
        Destroy(gameObject);

        // (6)
        CTL_PIEZO(2);
        CTL_DOT(2);
    }

    /*
     *  void PlayerWin()
     *   참조: Boss 스크립트
     *   설명: 1. 
     *          (1) 최종 점수가 최고 점수보다 큰 경우, 최고 점수를 변경합니다.
     *          (2) You Win UI를 활성화하여 눈에 보이도록 합니다.
     *          (3) 최종 점수와 최고 점수를 텍스트 UI에 출력합니다.
     *          (4) 게임이 진행되지 않도록 시간을 멈춥니다.
     *          (5) 플레이어 오브젝트를 삭제합니다.
     *          (6) 
     */
    public void PlayerWin() {
        // (1)
        string sceneName = SceneManager.GetActiveScene().name;
        if (mScore > PlayerPrefs.GetInt(sceneName + " Record")) {
            PlayerPrefs.SetInt(sceneName + " Record", mScore);
        }

        // (2)
        mYouWinWindow.SetActive(true);
        
        // (3)
        mWinScoreText.text = mScore.ToString();
        mWinRecordText.text = PlayerPrefs.GetInt(sceneName + " Record").ToString();
        
        // (4)
        Time.timeScale = 0.0f;
        
        // (5)
        Destroy(gameObject);

        // (6)
        CTL_PIEZO(1);
        CTL_DOT(1);
    }
}
