using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro.EditorUtilities;
using TMPro;

public class UIController : MonoBehaviour
{

    public RectTransform startTurnText;
    private TextMeshProUGUI _text;

    public Canvas hudInputs;

    public TextMeshProUGUI enemyHpText;
    public TextMeshProUGUI enemyStatsText;

    


    // Start is called before the first frame update
    void Start()
    {
        BattleSystem.Instance.StartPlayerTurn += OnTurn;
        BattleSystem.Instance.OnStateChange += OnBattleStateChanged;
        BattleSystem.Instance.OnSelectedEnemyChanged += OnSelectedEnemy;
        BattleSystem.Instance.OnEnemyHit += OnEnemyHit;
        _text = startTurnText.GetComponent<TextMeshProUGUI>(); 
    }

    private void OnSelectedEnemy(EnemyBattler enemy)
    {
        enemyHpText.text = "HP: " + enemy.hp.ToString();
    }

    private void OnEnemyHit(int hp)
    {
        enemyHpText.text = "HP: " + hp.ToString();
    }

    private void OnBattleStateChanged(BattleState state)
    {
        switch(state)
        {
            case BattleState.PLAYER_TURN:
                hudInputs.enabled = true;
                break;
            default:
                hudInputs.enabled = false;
                break;
        }
    }

    private void OnTurn(bool isPlayer)
    {
        if (isPlayer)
        {
            _text.text = "Player Turn";
            enemyHpText.enabled = true;
            enemyStatsText.enabled = true;
        }
        else
        {
            _text.text = "Enemy Turn";
            enemyHpText.enabled = false;
            enemyStatsText.enabled = false;
        }

        Sequence sequence = DOTween.Sequence();

        // Move the RectTransform out of the screen
        sequence.Append(startTurnText.DOAnchorPosX(0f, 0.4f).SetEase(Ease.OutQuad));

        // Wait for 2 seconds
        sequence.AppendInterval(1f);

        // Move the RectTransform to the center of the screen
        sequence.Append(startTurnText.DOAnchorPosX(-1500f, 0.4f).SetEase(Ease.OutQuad));
        sequence.Play();
    }
}
