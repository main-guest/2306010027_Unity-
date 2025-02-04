using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        // foward(z²)‚Ì•û‚ğŒü‚¯‚é‚±‚Æ‚Å•¶š‚ª”½“]‚·‚é‚Ì‚ğC³
        transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        // ©g‚ÌŒü‚«‚ğƒJƒƒ‰‚ÉŒü‚¯‚é
        transform.LookAt(Camera.main.transform);
    }
}
