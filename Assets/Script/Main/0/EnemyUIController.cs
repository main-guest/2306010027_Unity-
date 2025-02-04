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
//    // ダメージテキストの表示
//    public void ShowDamage(float damage, Vector3 position)
//    {
//        // テキストオブジェクトを位置に配置し、頭上に表示するためのオフセットを追加 
//        Vector3 textPosition = Camera.main.WorldToScreenPoint(position);
//        //Vector3 textPosition = Camera.main.WorldToScreenPoint(position + new Vector3(0, 0, 0));
//        damageText.transform.position = textPosition;
//        damageText.text = damage.ToString();

//        // テキストをアクティブにして、数秒後に非表示にする
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
