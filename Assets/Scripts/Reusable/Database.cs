using UnityEngine;

public class Database : MonoBehaviour {

#region STATIC

    [RuntimeInitializeOnLoadMethod]
    static void Reload() {
        _instance = null;
    }
    
    static Database _instance = null;

    public static Database GetInstance() {
        if ( _instance == null ) {
            _instance = Object.FindObjectOfType<Database>();
        }
        return _instance;
    }


#endregion

    public DatabaseMain Main;
    public DatabaseEffects Effects;


}