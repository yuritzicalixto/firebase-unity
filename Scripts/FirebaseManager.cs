/*
using UnityEngine;
using Firebase;

public class FirebaseManager : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log("✅ Firebase Inicializado Correctamente");
            }
            else
            {
                Debug.LogError("❌ Firebase no pudo inicializarse: " + task.Result);
            }
        });
    }
} */


/* *
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public FirebaseAuth auth;
    public DatabaseReference databaseRef;
    private bool firebaseReady = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene FirebaseManager en todas las escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                firebaseReady = true;
                Debug.Log("✅ Firebase Inicializado Correctamente.");
            }
            else
            {
                Debug.LogError("❌ Firebase no pudo inicializarse: " + task.Result);
            }
        });
    }

    public bool IsFirebaseReady()
    {
        return firebaseReady;
    }
} */

using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public FirebaseAuth auth;
    public DatabaseReference databaseRef;
    private bool firebaseReady = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("🔄 Intentando inicializar Firebase...");
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                firebaseReady = true;
                Debug.Log("✅ Firebase inicializado correctamente.");
            }
            else
            {
                Debug.LogError("❌ Error al inicializar Firebase: " + task.Result);
            }
        });
    }

    public bool IsFirebaseReady()
    {
        Debug.Log("📌 Estado de Firebase: " + firebaseReady);
        return firebaseReady;
    }
}
