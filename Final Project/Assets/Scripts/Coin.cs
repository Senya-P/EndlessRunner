using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")  // player collects the coin
        {
            GameManager.numberOfCoins++;
            Destroy(gameObject);
        }
    }
}
