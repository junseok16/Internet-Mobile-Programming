using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class ButtonEvent : MonoBehaviour
{
    /*
     * 공유 라이브러리
     * 
     */
    [DllImport("SharedObject")] private static extern int CTL_SEGMENT(int num);
    [DllImport("SharedObject")] private static extern int CTL_PIEZO(int music);
    [DllImport("SharedObject")] private static extern int CTL_LED(int num);
    [DllImport("SharedObject")] private static extern int CTL_LCD(int stage);
    [DllImport("SharedObject")] private static extern int CLEAR_LCD();
    [DllImport("SharedObject")] private static extern int CTL_FLED(int num);
    [DllImport("SharedObject")] private static extern int CTL_DOT(int num);
    [SerializeField] private GameObject mPauseWindow = null;

    /*
     * void LoadScene()
     *  참조: 없음
     *  설명: 1. 다음 신(scene)으로 전환합니다.
     *         (1) sceneName으로 신을 전환하고 게임 시간을 흘러가게 합니다.
     *         (2) 7 Segment를 0으로 초기화합니다.
     */
    public void LoadScene(string sceneName) {
        if (sceneName == "Mercury Stage") {
            CTL_LCD(1);
            CTL_FLED(4);
        }
        else if (sceneName == "Earth Stage") {
            CTL_LCD(2);
            CTL_FLED(2);
        }
        else if (sceneName == "Mars Stage") {
            CTL_LCD(3);
            CTL_FLED(1);
        }
        else if (sceneName == "Uranus Stage") {
            CTL_LCD(4);
            CTL_FLED(3);
        }
        else {
            CTL_LED(0);
            CLEAR_LCD();
            CTL_FLED(5);
        }

        CTL_PIEZO(0);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
        CTL_SEGMENT(0);
    }

    /*
     * void Quit()
     *  참조: 없음
     *  설명: 1. 게임을 종료합니다.
     */
    public void Quit()
    {
        CTL_SEGMENT(999999);
        Time.timeScale = 1.0f;
        Application.Quit();
    }

    /*
     * void Pause()
     *  참조: 없음
     *  설명: 1. 게임을 일시정지합니다.
     *         (1) 일시정지 창이 활성화되어있는 경우, 창을 활성화시키고 게임 시간을 흘러가게 합니다.
     *         (2) 일시정시 창이 비활성화되어있는 경우, 창을 활성화시키고 게임 시간을 멈춥니다.
     */
    public void Pause() {
        CTL_PIEZO(0);
        if (mPauseWindow.activeSelf) {
            mPauseWindow.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else {
            mPauseWindow.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    /*
     * void ClearRecord()
     *  참조: 없음
     *  설명: 1. 스코어보드를 초기화합니다.
     *         (1) 수성 스테이지 기록을 초기화합니다.
     *         (2) 지구 스테이지 기록을 초기화합니다.
     *         (3) 화성 스테이지 기록을 초기화합니다.
     *         (4) 천왕성 스테이지 기록을 초기화합니다.
     */
    public void ClearRecord() {
        CTL_PIEZO(0);
        CTL_DOT(3);
        PlayerPrefs.SetInt("Mercury Stage Record", 0);
        PlayerPrefs.SetInt("Earth Stage Record", 0);
        PlayerPrefs.SetInt("Mars Stage Record", 0);
        PlayerPrefs.SetInt("Uranus Stage Record", 0);
    }
}
