using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent1 : MonoBehaviour
{
    public DataMapper DataMapper;

    private IEnumerator Start()
    {
        Test1 test1 = new Test1(1, new Test3()
        {
            text = "COPY TEST2",
            color = Color.blue
        });

        DataMapper.SetData(test1);

        yield return new WaitForSeconds(2f);

        test1.test.text = "TEST TEST 33";
        test1.test.color = Color.red;

        DataMapper.RefreshData();
    }
}
