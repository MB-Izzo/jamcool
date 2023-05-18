using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLauncher : MonoBehaviour
{
    public Player player;
    public CinemachineVirtualCamera camTo;
    // Start is called before the first frame update
    void Start()
    {
        
        player.OnCombatStart += StartCombat; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartCombat()
    {
        Debug.Log("Starting combat now");
        camTo.enabled = true;
    }
}
