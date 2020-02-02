using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public bool IsStartSpawn { get { return isStartSpawn; } }
        [SerializeField] private GGJ2020.Managers.TimeManager timeManager;
        [SerializeField] private GameObject dogPrefab;
        [SerializeField] private GameObject roombaPrefab;
        [SerializeField] private Transform[] leftSpawnerPoints;
        [SerializeField] private Transform[] rightSpawnerPoints;
        [SerializeField] private int[] remmainTime;

        private bool isStartSpawn = false;
        private bool[] spawnTimes;

        // Start is called before the first frame update
        private void Awake()
        {
            spawnTimes = new bool[remmainTime.Length];
            for(int i = 0; i < spawnTimes.Length; i++)
            {
                spawnTimes[i] = false;
            }
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckRemainTime();
        }

        public void CheckRemainTime()
        {
            for(int i = 0; i < remmainTime.Length; i++)
            {
                if ((int)timeManager.RemainingTime.Value == remmainTime[i] && spawnTimes[i] == false)
                {
                    spawnTimes[i] = true;
                    StartSpawn();
                }
            }
            
        }

        public void StartSpawn()
        {
            Debug.Log("SpawnEnemy");
            leftSpawnerPoints = SpawnerShuffle(leftSpawnerPoints);
            rightSpawnerPoints = SpawnerShuffle(rightSpawnerPoints);
            var caseNum = Random.Range(0, 2);
            switch (caseNum)
            {
                case 0:
                    Instantiate(dogPrefab, leftSpawnerPoints[0].position, Quaternion.identity);
                    Instantiate(roombaPrefab, rightSpawnerPoints[0].position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(dogPrefab, rightSpawnerPoints[0].position, Quaternion.identity);
                    Instantiate(roombaPrefab, leftSpawnerPoints[0].position, Quaternion.identity);
                    break;
            }

            isStartSpawn = true;
        }

        private Transform[] SpawnerShuffle(Transform[] array)
        {
            int length = array.Length;
            Transform[] result = new Transform[length];
            array.CopyTo(result, 0);

            for (int i = 0; i < length; i++)
            {
                Transform tmp = result[i];
                int randomIndex = Random.Range(i, length);
                result[i] = result[randomIndex];
                result[randomIndex] = tmp;
            }

            return result;
        }
    }
}


