using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour {
    /*
     * SerializeField
     *  설명: 1. 인스펙터 창에서 정의할 변수를 선언합니다. 
     *         (1) 몬스터를 생성할 때 참조할 프리팹 배열을 선언합니다. 
     *         (2) 몬스터 체력 바를 나타낼 슬라이더 UI 프리팹을 선언합니다.
     *         (3)
     *         (4) 몬스터가 생성될 시간 간격을 선언합니다.
     *         (5) 몬스터를 생성될 위치를 갖는 트랜스폼 배열을 선언합니다.
     *         (6) 
     */
    [SerializeField] private GameObject[] mMonsterPrefab;
    [SerializeField] private GameObject mMonsterHPSliderPrefab;
    [SerializeField] private Transform mCanvasTransofrm;// Canvas 오브젝트의 Transform을 선언합니다.
    [SerializeField] private float mSpawnTime;
    [SerializeField] private Transform[] mSpawnLocation;
    [SerializeField] List<SpawnFormat> mSpawnList = new List<SpawnFormat>();
    
    private int mSpawnIndex = 0;
    private GameObject monster;

    /*
     * void Awake()
     *  설명:
     *  
     */
    private void Awake() {
        ReadStageFile();
        StartCoroutine("coroutineSpawn");
    }

    /*
     * void ReadStageFile()
     *  참조: MonsterSpawner 스크립트
     *  설명: 1. 리스트를 초기화하고 텍스트 파일을 엽니다.
     *         (1) 신 이름으로 된 텍스트 파일을 Resource 폴더에서 엽니다.
     *         
     *        2. 텍스트 파일에서 스테이지 데이터를 불러옵니다.
     *         (1) 텍스트 파일을 다 읽을 때까지 한 줄씩 읽습니다.
     *         (2) mSpawnFormat 오브젝트의 데이터 멤버에 스테이지 데이터(생성 시간, 몬스터 유형, 생성 위치, 발사 시간)를 저장합니다.
     *         (3) mSpawnFormat 오브젝트를 리스트에 추가합니다.
     *         
     *        3. 텍스트 파일을 닫습니다.
     *         (1) 텍스트 파일을 닫고 첫 번째 몬스터의 생성 시간을 설정합니다.
     */
    private void ReadStageFile() {
        // 1. (1)
        mSpawnList.Clear();
        mSpawnIndex = 0;
        string sceneName = SceneManager.GetActiveScene().name;
        TextAsset mTextAsset = Resources.Load(sceneName) as TextAsset;
        StringReader mStringReader = new StringReader(mTextAsset.text);
        
        while (mStringReader != null) {
            // 2. (1)
            string mLine = mStringReader.ReadLine();
            if (mLine == null) {
                break;
            }

            // 2. (2)
            SpawnFormat mSpawnFormat = new SpawnFormat();
            mSpawnFormat.time = float.Parse(mLine.Split('/')[0]);
            mSpawnFormat.type = mLine.Split('/')[1];
            mSpawnFormat.location = int.Parse(mLine.Split('/')[2]);
            mSpawnFormat.fireTime = float.Parse(mLine.Split('/')[3]);

            // 2. (3)
            mSpawnList.Add(mSpawnFormat);
        }

        // 3. (1)
        mStringReader.Close();
        mSpawnTime = mSpawnList[0].time;
        Debug.Log("read file completed!");
    }

    /*
     * IEnumerator coroutineSpawn()
     *  참조: MonsterSpawner 스크립트
     *  설명: 
     */
    private IEnumerator coroutineSpawn() {
        while (true) {
            int monsterPrefab = 0;

            if(mSpawnList[mSpawnIndex].type == "D") {
                StartCoroutine("coroutineBossSpawn");
                break;
            }

            switch (mSpawnList[mSpawnIndex].type) {
                case "A":
                    monsterPrefab = 2;
                    break;
                case "B":
                    monsterPrefab = 1;
                    break;
                case "C":
                    monsterPrefab = 0;
                    break;
            }

            int monsterLocation = mSpawnList[mSpawnIndex].location;// 몬스터를 생성할 위치 값을 불러옵니다.
            mMonsterPrefab[monsterPrefab].GetComponent<MonsterFire>().mFireTime = mSpawnList[mSpawnIndex].fireTime;// 몬스터 미사일 발사 시간을 불러옵니다.

            monster = Instantiate(mMonsterPrefab[monsterPrefab], mSpawnLocation[monsterLocation].position, Quaternion.identity);// 몬스터 오브젝트를 생성합니다.
            MonsterHPSlider(monster);// 몬스터 체력 슬라이더를 생성합니다.

            // 오른쪽 위치에서 왼쪽 아래로 스폰됩니다.
            if (5 <= monsterLocation && monsterLocation <= 6) {
                monster.GetComponent<Move>().MoveTo(new Vector3(-1, -1.0f, 0));
            }
            // 왼쪽 위치에서 오른쪽 아래로 스폰됩니다.
            else if (7 <= monsterLocation && monsterLocation <= 8) {
                monster.GetComponent<Move>().MoveTo(new Vector3(1, -1.0f, 0));
            }

            mSpawnIndex++;// 스폰 인덱스를 증가시킵니다.
            mMonsterPrefab[monsterPrefab].GetComponent<MonsterFire>().mFireTime = 0;

            if (mSpawnIndex == mSpawnList.Count) {
                break;
            }
            yield return new WaitForSeconds(mSpawnTime);// 몬스터 스폰 시간만큼 기다립니다. 
            mSpawnTime = mSpawnList[mSpawnIndex].time;// 다음 몬스터 스폰 시간을 설정합니다.
        }
    }

    /*
     * IEnumerator coroutineBossSpawn()
     *  참조: MonsterSpawner 스크립트
     *  설명: 
     */
    private IEnumerator coroutineBossSpawn() {
        int monsterLocation = mSpawnList[mSpawnIndex].location;
        GameObject boss = Instantiate(mMonsterPrefab[3], mSpawnLocation[monsterLocation].position, Quaternion.identity);
        boss.GetComponent<Boss>().ChangeState(BossState.coroutineMoveToCenter);
        yield return new WaitForSeconds(1.0f);
    }

    /*
     * void MonsterHPSlider(Gameobject)
     *  참조: MonsterSpawner 스크립트
     *  설명: 1. 몬스터 체력을 나타내는 슬라이더 UI를 생성합니다.
     *         (1) 
     */
    private void MonsterHPSlider(GameObject monster) {
        GameObject slider = Instantiate(mMonsterHPSliderPrefab);        // 몬스터 체력을 나타내는 Slider UI를 생성합니다.
        slider.transform.SetParent(mCanvasTransofrm);        // Slider UI를 Canvas 오브젝트의 자식으로 설정합니다.
        slider.transform.localScale = Vector3.one;
        slider.GetComponent<MonsterHPLocater>().SetUp(monster.transform);
        slider.GetComponent<MonsterHPViewer>().SetUp(monster.GetComponent<MonsterHP>());
    }
}
