using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod]
    static void Reload() {
        _instance = null;
    }

    static ObjectPool _instance = null;
    Dictionary<UnityEngine.Object, List<GameObject>> _pools = new Dictionary<Object, List<GameObject>>();

    static ObjectPool GetInstance() {
        if ( _instance == null ) {
            _instance = ObjectPool.FindObjectOfType<ObjectPool>();
        }
        return _instance;
    }

    public static T Get<T>(T obj) where T : Object {
        
        var i = GetInstance();
        if ( !i._pools.ContainsKey(obj) ) {
            i._pools[obj] = new List<GameObject>();
        }

        var pool = i._pools[obj];

        foreach(var go in pool) {
            if ( go.activeSelf ) continue;
            go.transform.SetParent(i.transform);
            go.SetActive(true);
            return go.GetComponent<T>();
        }

        var ni = GameObject.Instantiate(obj, i.transform);
        pool.Add(ni.GameObject());

        return ni;
    }

}