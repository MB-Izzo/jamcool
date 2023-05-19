using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBattlers : MonoBehaviour
{
    private static EnemiesBattlers instance = null;
    public static EnemiesBattlers Instance => instance;
    public EnemyBattler[] enemies;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
