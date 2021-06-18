using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public DataMapper DataMapper;

    private void Start()
    {
        Test1 text1 = new Test1(1, new Test3()
        {
            text = "COPY TEST2"
        });
        Test2 text2 = new Test2()
        {
            text = "TEST2"
        };

        DataMapper.SetData(text1);
    }
}
