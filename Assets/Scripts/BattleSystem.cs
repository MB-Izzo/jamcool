using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

public enum BattleState
{
    PLAYER_ATTACKING,
    ENEMY_ATTACKING,
    PLAYER_TURN,
    ENEMY_TURN,
}

public class BattleSystem : MonoBehaviour
{
    private static BattleSystem instance = null;
    public static BattleSystem Instance => instance;

    public List<EnemyBattler> enemies;
    public PlayerBattler player;


    public CinemachineTargetGroup camTargetGroup;
    public PlayableDirector cine;

    [SerializeField]
    private int _selectedEnemyIdx = 0;

    [SerializeField]
    private int _playingEnemyIdx = 0;

    [SerializeField]
    private BattleState _battleState = BattleState.PLAYER_TURN;

    public BattleState GetBattleState
    {
        get { return _battleState; }
        set { _battleState = value; }
    }

    [SerializeField]
    private int _playerAction = 2;
    [SerializeField]
    private int _enemiesAction = 2;

    public Action<bool> StartPlayerTurn;

    public Action<BattleState> OnStateChange;

    public Action<EnemyBattler> OnSelectedEnemyChanged;
    public Action<int> OnEnemyHit;

    public AudioManager audio;

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

        player.OnRightAction += SelectNextEnemy;
        player.OnLeftAction += SelectPreviousEnemy;
        player.OnAttackAction += AttackAction;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (StartPlayerTurn != null) StartPlayerTurn(true);
        OnStateChange?.Invoke(_battleState);
    }

    private void AttackAction()
    {
        if (_battleState != BattleState.PLAYER_TURN)
        {
            return;
        }
        audio.confirm.Play();
        _battleState= BattleState.PLAYER_ATTACKING;
        OnStateChange?.Invoke(_battleState);

        foreach (var playableAssetOutput in cine.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName == "AttackerTrack")
            {
                cine.SetGenericBinding(playableAssetOutput.sourceObject, player.GetComponentInChildren<Animator>());
            }

            if (playableAssetOutput.streamName == "TargetTrack")
            {
                cine.SetGenericBinding(playableAssetOutput.sourceObject, GetSelectedEnemy().GetComponentInChildren<Animator>());
            }
        }
        cine.enabled = true;
    }


    public void MoveToPos()
    {
        if (_battleState == BattleState.PLAYER_ATTACKING)
        {
            player.transform.DOMove(GetSelectedEnemy().runTargetPos.position, 0.8f);
        }
        else if (_battleState == BattleState.ENEMY_ATTACKING)
        {
            GetNextEnemy().transform.DOMove(player.runTargetPos.position, 0.8f);
        }
    }
    public void ResetPlayerPos()
    {
        if (_battleState == BattleState.PLAYER_ATTACKING)
        {
            player.transform.position = player.initialPos;
            player.transform.DOLookAt(player.initialPos, 0.0f, AxisConstraint.Y);
            if (_playerAction == 1)
            {
                if (StartPlayerTurn != null) StartPlayerTurn(false);
            }
        }
        else if (_battleState == BattleState.ENEMY_ATTACKING)
        {
            GetNextEnemy().transform.position = GetNextEnemy().initialPos;
            GetNextEnemy().transform.DOLookAt(player.initialPos, 0.0f, AxisConstraint.Y);
            if (_enemiesAction == 1)
            {
                if (StartPlayerTurn != null) StartPlayerTurn(true);
            }
        }

        
    }

    public void SetRotationTowardEnemy()
    {
        if (_battleState == BattleState.PLAYER_ATTACKING)
        {
            player.transform.DOLookAt(BattleSystem.Instance.GetSelectedEnemy().runTargetPos.position, 0.5f, AxisConstraint.Y);
        }
        else if (_battleState == BattleState.ENEMY_ATTACKING)
        {
            GetNextEnemy().transform.DOLookAt(player.runTargetPos.position, 0.5f, AxisConstraint.Y);
        }
    }

    public void CineEnd()
    {
        cine.enabled = false;
        cine.time = 0;
        if (_battleState == BattleState.PLAYER_ATTACKING)
        {
            _playerAction--;
            _battleState = BattleState.PLAYER_TURN;
            OnStateChange?.Invoke(_battleState);
        }
        else if (_battleState == BattleState.ENEMY_ATTACKING)
        {
            _enemiesAction--;
            _battleState = BattleState.ENEMY_TURN;
            OnStateChange?.Invoke(_battleState);
        }
        ComputeNextTurn();
    }

    public void TakeHit()
    {
        if (_battleState == BattleState.PLAYER_ATTACKING)
        {
            GetSelectedEnemy().hp -= player.dmg;
            OnEnemyHit?.Invoke(GetSelectedEnemy().hp);
            if (GetSelectedEnemy().hp <= 0)
            {
                Destroy(GetSelectedEnemy().gameObject);
                enemies.Remove(GetSelectedEnemy());

                camTargetGroup.RemoveMember(GetSelectedEnemy().transform);
                _selectedEnemyIdx++;
                camTargetGroup.AddMember(GetSelectedEnemy().transform, 1f, 0f);
                GetSelectedEnemy().ToggleTarget();
            }
        }
        else if (_battleState == BattleState.ENEMY_ATTACKING)
        {
            player.hp -= GetNextEnemy().dmg;
            
        }
    }

    public void SoundHit()
    {
        audio.punch.Play();
    }

    private void ComputeNextTurn()
    {
        if (_playerAction == 0 && _battleState != BattleState.ENEMY_TURN)
        {
            _battleState = BattleState.ENEMY_TURN;
            OnStateChange?.Invoke(_battleState);
            _enemiesAction = 2;
        }

        if (_battleState == BattleState.ENEMY_TURN)
        {
            _playingEnemyIdx++;
            if (_enemiesAction > 0)
            {

                _battleState = BattleState.ENEMY_ATTACKING;
                OnStateChange?.Invoke(_battleState);
                foreach (var playableAssetOutput in cine.playableAsset.outputs)
                {
                    if (playableAssetOutput.streamName == "AttackerTrack")
                    {
                        cine.SetGenericBinding(playableAssetOutput.sourceObject, GetNextEnemy().GetComponentInChildren<Animator>());
                    }

                    if (playableAssetOutput.streamName == "TargetTrack")
                    {
                        cine.SetGenericBinding(playableAssetOutput.sourceObject, player.GetComponentInChildren<Animator>());
                    }
                }
                camTargetGroup.m_Targets[1].target = GetNextEnemy().transform; 
                cine.enabled = true;
            }
            else
            {
                //if (StartPlayerTurn != null) StartPlayerTurn(true);
                _battleState = BattleState.PLAYER_TURN;
                OnStateChange?.Invoke(_battleState);
                _playerAction = 2;
            }
        }
    }

    public EnemyBattler GetSelectedEnemy()
    {
        return enemies[_selectedEnemyIdx % enemies.Count];
    }
    public EnemyBattler GetNextEnemy()
    {
        return enemies[_playingEnemyIdx % enemies.Count];
    }

    private void SelectNextEnemy()
    {
        if (_battleState != BattleState.PLAYER_TURN)
        {
            return;
        }
        camTargetGroup.RemoveMember(GetSelectedEnemy().transform);
        GetSelectedEnemy().ToggleTarget();

        _selectedEnemyIdx++;
        camTargetGroup.AddMember(GetSelectedEnemy().transform, 1f, 0f);
        GetSelectedEnemy().ToggleTarget();
        OnSelectedEnemyChanged?.Invoke(GetSelectedEnemy());
    }

    private void SelectPreviousEnemy()
    {
        if (_battleState != BattleState.PLAYER_TURN)
        {
            return;
        }
        camTargetGroup.RemoveMember(GetSelectedEnemy().transform);
        GetSelectedEnemy().ToggleTarget();

        _selectedEnemyIdx++;
        camTargetGroup.AddMember(GetSelectedEnemy().transform, 1f, 0f);
        GetSelectedEnemy().ToggleTarget();
        OnSelectedEnemyChanged?.Invoke(GetSelectedEnemy());
    }
}
