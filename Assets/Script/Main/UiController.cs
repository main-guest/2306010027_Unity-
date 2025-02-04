using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    // �X�e�[�^�X��ʂ𑀍삷�邽�߂�UI�֘A
    [SerializeField] private GameObject statusPanelWindow;
    [SerializeField] private Text HpText, AttackText, DefenceText;

    //GAME OVER���
    [SerializeField] private GameObject GameOverWindow;

    //HP�Q�[�Wscript
    private HpGauge hpGauge;

    private float PlayerHp;
    private float PlayerAttack;
    private float PlayerDefence;

    // Start is called before the first frame update
    void Start()
    {
        //HP�Q�[�Wscript
        hpGauge = FindObjectOfType<HpGauge>();

        // �X�e�[�^�X��ʂ�������ԂŔ�\���ɂ���
        statusPanelWindow.SetActive(false);

        //�Q�[���I�[�o�[��ʂ��\��
        GameOverWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // PLAYER�̃X�e�[�^�X���X�V
        UpdateStatus();

        // �X�e�[�^�X��ʂ̕\��/��\����؂�ւ�
        HandleStatusPanelToggle();

        // UI���X�V
        UpdateUI();
    }

    // PLAYER�̃X�e�[�^�X���X�V
    private void UpdateStatus()
    {
        PlayerHp = PlayerController.currentHp;
        PlayerAttack = PlayerController.attack;
        PlayerDefence = PlayerController.defence;
    }

    // �E�N���b�N�ŃX�e�[�^�X��ʂ�\��/��\��
    private void HandleStatusPanelToggle()
    {
        if (Input.GetMouseButton(1)) // �E�N���b�N�ŃX�e�[�^�X�\��
        {
            statusPanelWindow.SetActive(true);  // �X�e�[�^�X��ʂ�\��
        }
        else if (Input.GetMouseButtonUp(1)) // �E�N���b�N�𗣂��ƃX�e�[�^�X��\��
        {
            statusPanelWindow.SetActive(false); // �X�e�[�^�X��ʂ��\��
        }
    }

    // UI�iHP�A�U���́A�h��́j���X�V
    private void UpdateUI()
    {
        HpText.text = PlayerHp.ToString();  // HP���X�V
        AttackText.text = PlayerAttack.ToString();  // �U���͂��X�V
        DefenceText.text = PlayerDefence.ToString();  // �h��͂��X�V
    }

    public void gameOver()
    {
        GameOverWindow.SetActive(true);
    }

    public void gameReStart()
    {
        // HP�Q�[�W�����Z�b�g
        hpGauge.ResetHp();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
