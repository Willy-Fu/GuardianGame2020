using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI物件需要增加此行

public class ButtonCreateTest : MonoBehaviour

{
    public GameObject Instantiate_Position; //物件的生成點。
    public GameObject TestBox; //要生成的物件。

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void click()
    {
Instantiate(TestBox, Instantiate_Position.transform.position,
Instantiate_Position.transform.rotation);

        //生成(Box, 物件的位置:生成點的位置, 物件的旋轉值:生成點的旋轉值);
        //Instantiate實例化(要生成的物件, 物件位置, 物件旋轉值);
        Debug.Log("Skill activate");
    }


}
