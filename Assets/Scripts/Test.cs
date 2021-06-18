using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public DataMapper DataMapper;

    private IEnumerator Start()
    {
        Test1 text1 = new Test1(1, new Test3()
        {
            text = "COPY TEST2",
            color = Color.white
        });
        Test2 text2 = new Test2()
        {
            text = "TEST2"
        };

        DataMapper.SetData(text1);

        yield return new WaitForSeconds(2f);

        text1.test.text = "TEST TEST 33";
        text1.test.color = Color.red;
        DataMapper.RefreshData();
    }
}
