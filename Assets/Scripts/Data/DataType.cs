using System;
using System.Collections.Generic;

public abstract class BaseData { }

public class Test1 : BaseData
{
    public int ID;

    public Test2 test { get; }

    public Test1(int id)
    {
        ID = id;
    }
}

public class Test2 : BaseData
{
    public string text;
}

public class Test3 : BaseData
{
}