using System;
using System.Collections.Generic;

public abstract class BaseData { }

public static class DataTypes
{
	public static Dictionary<string, Type> maps = new Dictionary<string, Type>{
		{"Test1", typeof(Test1)},
		{"Test2", typeof(Test2)},
		{"Test3", typeof(Test3)},
	};
}

public class Test1 : BaseData
{
    public int ID;
    public Test2 test {
        get;
    }

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