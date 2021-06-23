using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestComponent2 : MonoBehaviour
{
    public Text Text1;
    public Text Text2;

    private IEnumerator Start()
    {
        Test1 test1 = new Test1(1, new Test3()
        {
            text = "COPY TEST2",
            color = Color.blue
        });

        SetText1(test1);
        SetText2(test1);

        yield return new WaitForSeconds(2f);

        test1.test.text = "TEST TEST 33";
        test1.test.color = Color.red;

        SetText1(test1);
    }

    private void SetText1(Test1 test1)
    {
        if (!Text1.IsValid()) return;

        Text1.text = $"{test1.test.text} 안녕";
        Text1.color = test1.test.color;
    }

    private void SetText2(Test1 test1)
    {
        if (!Text2.IsValid()) return;

        Text2.text = $"{test1.test.text} 안녕2";
        Text2.color = test1.test.color;
    }
}
