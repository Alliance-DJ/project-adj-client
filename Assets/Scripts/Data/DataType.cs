using System;
using System.Collections.Generic;

public abstract class BaseData { }

public class Test1 : BaseData
{
    public int ID;

    public Test3 test { get; }

    public Test1(int id, Test3 test2)
    {
        ID = id;
        test = test2;
    }
}

public class Test2 : BaseData
{
    public string text;
}

public class Test3 : Test2
{
}