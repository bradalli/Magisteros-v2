using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAndDataManager : MonoBehaviour
{
    #region Data handling
    private Dictionary<string, object> dataDictionary;

    public void SetValue<T>(string key, T value)
    {
        if (dataDictionary.ContainsKey(key))
            dataDictionary[key] = value;

        else
            dataDictionary.Add(key, value);
    }
    public T GetValue<T>(string key)
    {
        if (dataDictionary.ContainsKey(key))
            return (T)dataDictionary[key];

        return default(T); // Return default value if key doesn't exist.
    }
    #endregion

    #region Event handling
    private Dictionary<string, Action> eventDictionary;

    public void AddEvent(string eventName, Action eventAction)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] += eventAction;

        else
            eventDictionary.Add(eventName, eventAction);
    }

    public void RemoveEvent(string eventName, Action eventAction)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] -= eventAction;
    }

    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName]?.Invoke();

        else
            Debug.LogWarning($"{gameObject.name} does not define an event for '{eventName}'");
    }
    #endregion
}
