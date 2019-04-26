using System.Collections.Generic;


public static class GlobalVar
{
    private static Dictionary<string, object> dataStorage = new Dictionary<string, object>();

    #region Genaric Style

    public static T Get<T>(string varName, T defaultValue = default(T))
    {
        if (dataStorage.ContainsKey(varName))
            return (T)dataStorage[varName];
        return defaultValue;
    }

    public static void Set(string varName, object value)
    {
        if (dataStorage.ContainsKey(varName))
            dataStorage[varName] = value;
        else
            dataStorage.Add(varName, value);
    }

    public static void Set(string varName, ref object value)
    {
        if (dataStorage.ContainsKey(varName))
            dataStorage[varName] = value;
        else
            dataStorage.Add(varName, value);
    }

    #endregion
}
