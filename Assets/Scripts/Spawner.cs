using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs; //동전 프리팹
    public GameObject MissilePrefabs; //미사일 프리팹

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f; //최소 생성 간격 (초)
    public float maxSpawnInterval = 2.0f; //최대 생성 간격 (초)

    [Header("동전 스폰 확률 설정")]
    [Range(0, 100)] //유니티 UI에서 할 수 있게한다.
    public int coinSpawnChance = 50; //동전이 생성될 확률 (0 ~ 100)

    public float timer = 0.0f;
    public float nextSpawnTime; //다음 생성 시간

    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //생성 시간이 되면 오브젝트 생성
        if(timer >= nextSpawnTime)
        {
            SpawnObject(); //함수를 호출 해준다.
            timer = 0.0f; //시간을 초기화 시켜준다.
            SetNextSpawnTime(); //다시 함수를 실행
        }
    }

    void SpawnObject()
    {
        Transform spawnTransfrom = transform; //스포너 오브젝트의 위치와 회전 값을 가져온다.

        //확률에 따라 동전 또는 미사일 생성
        int randomValue = Random.Range(0, 100); //0~100의 랜덤 값을 뽑아낸다.
        if (randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransfrom.position, spawnTransfrom.rotation); //코인 프리랩을 해당 위치에 생성한다.
        }
        else
        {
            Instantiate(MissilePrefabs, spawnTransfrom.position, spawnTransfrom.rotation); //미사일 프리랩을 해당 위치에 생성한다.
        }
    }

    void SetNextSpawnTime()
    {
        //최소-최대 사이의 랜덤한 시간 설정
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
