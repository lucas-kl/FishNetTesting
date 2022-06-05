using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {

    private static T _Instance;
    public static T Instance {
        get {
            if (_Instance == null) {
                T[] objects = FindObjectsOfType<T>();
                if (objects.Length == 1) {
                    _Instance = objects[0];
                } else if (objects.Length > 1) {
                    Debug.LogErrorFormat("Expected exactly 1 {0} but found {1}", typeof(T).ToString(), objects.Length);
                }
            }
            return _Instance;
        }
    }
}
