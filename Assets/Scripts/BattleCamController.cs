using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamController : MonoBehaviour
{
    public PlayerBattler playerBattler;

    public GameObject[] enemies;
    public CinemachineTargetGroup targetGroup;
    // Start is called before the first frame update
    void Start()
    {
        playerBattler.OnRightAction += OnRight;
        playerBattler.OnLeftAction += OnLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRight()
    {
/*        if (targetGroup.m_Targets[1].target.gameObject == enemies[0])
        {

            targetGroup.RemoveMember(BattleSystem.Instance.enemies[0].transform);
            targetGroup.AddMember(BattleSystem.Instance.enemies[1].transform, 1f, 0f);
            enemies[0].GetComponent<EnemyBattler>().ToggleTarget();
            enemies[1].GetComponent<EnemyBattler>().ToggleTarget();
        }
*/    }
    public void OnLeft()
    {
/*        if (targetGroup.m_Targets[2].target.gameObject == enemies[1])
        {
            targetGroup.RemoveMember(enemies[2].transform);
            targetGroup.AddMember(enemies[1].transform, 1f, 0f);
            enemies[1].GetComponent<EnemyBattler>().ToggleTarget();
            enemies[2].GetComponent<EnemyBattler>().ToggleTarget();
        }
*/    }

    private void GetEnemyByIdx(int idx)
    {

    }

}
