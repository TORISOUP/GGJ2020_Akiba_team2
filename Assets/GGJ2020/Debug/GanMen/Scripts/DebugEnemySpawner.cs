using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanGanKamen
{
    public class DebugEnemySpawner : MonoBehaviour
    {
        private GGJ2020.Enemies.EnemySpawner enemySpawner;
        // Start is called before the first frame update
        private void Awake()
        {
            enemySpawner = GetComponent<GGJ2020.Enemies.EnemySpawner>();
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                enemySpawner.StartSpawn();
            }
        }
    }
}


