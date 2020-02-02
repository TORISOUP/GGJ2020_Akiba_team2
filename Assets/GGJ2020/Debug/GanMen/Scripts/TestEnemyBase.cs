using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBase : MonoBehaviour
{
    public float MoveSpeed { set; get; }
    public List<Vector2> LeftMoveRange { set; get; }
    public List<Vector2> RightMoveRange { set; get; }
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPoint(bool LeftOrRight)
    {
        switch (LeftOrRight)
        {
            case false:
                var posx = Random.Range(LeftMoveRange[0].x, LeftMoveRange[1].x);

                break;
            case true:
                break;
        }
    }
}
