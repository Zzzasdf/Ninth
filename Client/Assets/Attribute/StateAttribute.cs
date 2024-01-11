using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, Inherited = true)]
public class StateAttribute : PropertyAttribute
{
    public string propertyName;

    public StateAttribute(string propertyName)
    {
        this.propertyName = propertyName;
    }
}