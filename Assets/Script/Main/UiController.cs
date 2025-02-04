using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    // ステータス画面を操作するためのUI関連
    [SerializeField] private GameObject statusPanelWindow;
    [SerializeField] private Text HpText, AttackText, DefenceText;

    //GAME OVER画面
    [SerializeField] private GameObject GameOverWindow;

    //HPゲージscript
    private HpGauge hpGauge;

    private float PlayerHp;
    private float PlayerAttack;
    private float PlayerDefence;

    // Start is called before the first frame update
    void Start()
    {
        //HPゲージscript
        hpGauge = FindObjectOfType<HpGauge>();

        // ステータス画面を初期状態で非表示にする
        statusPanelWindow.SetActive(false);

        //ゲームオーバー画面を非表示
        GameOverWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // PLAYERのステータスを更新
        UpdateStatus();

        // ステータス画面の表示/非表示を切り替え
        HandleStatusPanelToggle();

        // UIを更新
        UpdateUI();
    }

    // PLAYERのステータスを更新
    private void UpdateStatus()
    {
        PlayerHp = PlayerController.currentHp;
        PlayerAttack = PlayerController.attack;
        PlayerDefence = PlayerController.defence;
    }

    // 右クリックでステータス画面を表示/非表示
    private void HandleStatusPanelToggle()
    {
        if (Input.GetMouseButton(1)) // 右クリックでステータス表示
        {
            statusPanelWindow.SetActive(true);  // ステータス画面を表示
        }
        else if (Input.GetMouseButtonUp(1)) // 右クリックを離すとステータス非表示
        {
            statusPanelWindow.SetActive(false); // ステータス画面を非表示
        }
    }

    // UI（HP、攻撃力、防御力）を更新
    private void UpdateUI()
    {
        HpText.text = PlayerHp.ToString();  // HPを更新
        AttackText.text = PlayerAttack.ToString();  // 攻撃力を更新
        DefenceText.text = PlayerDefence.ToString();  // 防御力を更新
    }

    public void gameOver()
    {
        GameOverWindow.SetActive(true);
    }

    public void gameReStart()
    {
        // HPゲージをリセット
        hpGauge.ResetHp();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
