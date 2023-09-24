using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiggerRandomFruit : MonoBehaviour
{
    [SerializeField] private GameObject[] _fruits;

     [SerializeField] Vector2 intervals = new Vector2(1f, 3f);


    IEnumerator Start()
    {
        while (_fruits.Length > 0)
        {
            yield return new WaitForSeconds(Random.Range(intervals.x, intervals.y));

            int randomFriut = Random.Range(0, _fruits.Length);
            //getAnimation 
            GameObject fruit = _fruits[randomFriut];            
            //playAnimation
            fruit.GetComponent<Animation>().Play();
            //wait for animation to finish
            yield return new WaitForSeconds(fruit.GetComponent<Animation>().clip.length);
            
        }
    }
}
