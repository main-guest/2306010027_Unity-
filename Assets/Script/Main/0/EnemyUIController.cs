//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class EnemyUIController : MonoBehaviour
//{
//    //private float moveSpeed = 0.4f;
//    public Text damageText;

//    private void Start()
//    {
//        Invoke("HideDamage", 0f);
//    }
//    // �_���[�W�e�L�X�g�̕\��
//    public void ShowDamage(float damage, Vector3 position)
//    {
//        // �e�L�X�g�I�u�W�F�N�g���ʒu�ɔz�u���A����ɕ\�����邽�߂̃I�t�Z�b�g��ǉ� 
//        Vector3 textPosition = Camera.main.WorldToScreenPoint(position);
//        //Vector3 textPosition = Camera.main.WorldToScreenPoint(position + new Vector3(0, 0, 0));
//        damageText.transform.position = textPosition;
//        damageText.text = damage.ToString();

//        // �e�L�X�g���A�N�e�B�u�ɂ��āA���b��ɔ�\���ɂ���
//        damageText.gameObject.SetActive(true);
//        Debug.Log("ON"+textPosition);

//        //transform.position += Vector3.up * moveSpeed * Time.deltaTime;
//        Invoke("HideDamage", 1f);
//    }

//    void HideDamage()
//    {
//        damageText.gameObject.SetActive(false);
//    }
//}
