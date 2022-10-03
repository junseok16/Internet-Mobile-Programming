using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RatingScoreViewer : MonoBehaviour {
    /*
     * [SerializedField]
     *  설명: 1. 수성 스테이지의 기록 텍스트를 선언합니다.
     *        2. 지구 스테이지의 기록 텍스트를 선언합니다.
     *        3. 화성 스테이지의 기록 텍스트를 선언합니다.
     *        4. 천왕성 스테이지의 기록 텍스트를 선언합니다.
     */
    [SerializeField] private TextMeshProUGUI mMercuryRecordText;
    [SerializeField] private TextMeshProUGUI mEarthRecordText;
    [SerializeField] private TextMeshProUGUI mMarsRecordText;
    [SerializeField] private TextMeshProUGUI mUranusRecordText;

    /*
     * void Awake()
     *  참조: 없음
     *  설명: 1. 프리퍼런스로 스테이지 기록을 불러와 텍스트로 출력합니다.
     *         (1) 각 스테이지마다 프리퍼런스로 저장된 기록을 불러옵니다.
     *         (2) 불러온 기록을 문자열로 텍스트에 출력합니다.
     */
    private void Awake() {
        // (1)
        int mercuryRecord = PlayerPrefs.GetInt("Mercury Stage Record");
        int earthRecord = PlayerPrefs.GetInt("Earth Stage Record");
        int marsRecord = PlayerPrefs.GetInt("Mars Stage Record");
        int uranusRecord = PlayerPrefs.GetInt("Uranus Stage Record");

        // (2)
        mMercuryRecordText.text = mercuryRecord.ToString();
        mEarthRecordText.text = earthRecord.ToString();
        mMarsRecordText.text = marsRecord.ToString();
        mUranusRecordText.text = uranusRecord.ToString();
    }
}
