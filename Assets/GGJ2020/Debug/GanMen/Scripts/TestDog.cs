using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDog : TestEnemyBase
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Vector2> moveRangeL;
    [SerializeField] private List<Vector2> moveRangeR;
    // Start is called before the first frame update
    public void Init()
    {
        MoveSpeed = moveSpeed;
        LeftMoveRange = moveRangeL;
        RightMoveRange = moveRangeR;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
