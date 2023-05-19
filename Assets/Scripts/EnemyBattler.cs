using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattler : MonoBehaviour
{
    public SpriteRenderer targetSprite;
    public Transform runTargetPos;

    public int hp;
    public int dmg;

    public Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTarget()
    {
        targetSprite.enabled = !targetSprite.enabled;
    }
}
