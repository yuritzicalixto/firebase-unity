/*
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseLogin : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI messageText;

    private FirebaseAuth auth;
    private DatabaseReference databaseRef;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });

        loginButton.onClick.AddListener(() => StartCoroutine(CheckUserExists(emailInput.text, passwordInput.text)));
    }

    IEnumerator CheckUserExists(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageText.text = "❌ Ingresa correo y contraseña.";
            yield break;
        }

        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            messageText.text = "❌ Usuario no registrado.";
        }
        else
        {
            FirebaseUser user = loginTask.Result.User;

            // Verificar si el usuario está en la base de datos
            var dbTask = databaseRef.Child("users").Child(user.UserId).GetValueAsync();
            yield return new WaitUntil(() => dbTask.IsCompleted);

            if (dbTask.Exception != null || !dbTask.Result.Exists)
            {
                messageText.text = "❌ Usuario no encontrado en la base de datos.";
            }
            else
            {
                messageText.text = "✅ Login exitoso.";
                PlayerPrefs.SetString("UserID", user.UserId);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
    }
} */


/* */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseLogin : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI messageText;

    void Start()
    {
        loginButton.onClick.AddListener(() =>
        {
            if (FirebaseManager.Instance.IsFirebaseReady())
            {
                StartCoroutine(CheckUserExists(emailInput.text, passwordInput.text));
            }
            else
            {
                messageText.text = "❌ Esperando inicialización de Firebase...";
            }
        });
    }

    IEnumerator CheckUserExists(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageText.text = "❌ Ingresa correo y contraseña.";
            yield break;
        }

        var auth = FirebaseManager.Instance.auth;
        var databaseRef = FirebaseManager.Instance.databaseRef;

        if (auth == null || databaseRef == null)
        {
            messageText.text = "❌ Firebase no está listo.";
            yield break;
        }

        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            messageText.text = "❌ Usuario no registrado.";
        }
        else
        {
            FirebaseUser user = loginTask.Result.User;

            var dbTask = databaseRef.Child("users").Child(user.UserId).GetValueAsync();
            yield return new WaitUntil(() => dbTask.IsCompleted);

            if (dbTask.Exception != null || !dbTask.Result.Exists)
            {
                messageText.text = "❌ Usuario no encontrado en la base de datos.";
            }
            else
            {
                messageText.text = "✅ Login exitoso.";
                PlayerPrefs.SetString("UserID", user.UserId);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            }
        }
    }
} 
