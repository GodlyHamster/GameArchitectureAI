using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    public T GetVariable<T>(string name)
    {
        if (variables.ContainsKey(name))
        {
            return (T)variables[name];
        }
        return default(T);
    }

    public void SetVariable<T>(string name, T variable)
    {
        if (variables.ContainsKey(name))
        {
            variables[name] = variable;
        }
        else
        {
            variables.Add(name, variable);
        }
    }
}
