/*
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseRegister : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button registerButton;
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

        registerButton.onClick.AddListener(() => StartCoroutine(RegisterUser(emailInput.text, passwordInput.text)));
    }

    IEnumerator RegisterUser(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageText.text = "❌ Ingresa correo y contraseña.";
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            messageText.text = "❌ Error: " + registerTask.Exception.Message;
        }
        else
        {
            FirebaseUser user = registerTask.Result.User;
            UserData newUser = new UserData(user.UserId, email); // Cambio de 'User' a 'UserData'
            string json = JsonUtility.ToJson(newUser);

            databaseRef.Child("users").Child(user.UserId).SetRawJsonValueAsync(json);

            messageText.text = "✅ Registro exitoso.";
        }
    }
} 
*/

/* */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseRegister : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button registerButton;
    public TextMeshProUGUI messageText;

    void Start()
    {
        registerButton.onClick.AddListener(() =>
        {
            if (FirebaseManager.Instance.IsFirebaseReady())
            {
                StartCoroutine(RegisterUser(emailInput.text, passwordInput.text));
            }
            else
            {
                messageText.text = "❌ Esperando inicialización de Firebase...";
            }
        });
    }

    IEnumerator RegisterUser(string email, string password)
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

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            messageText.text = "❌ Error: " + registerTask.Exception.Message;
        }
        else
        {
            FirebaseUser user = registerTask.Result.User;
            UserData newUser = new UserData(user.UserId, email);
            string json = JsonUtility.ToJson(newUser);

            databaseRef.Child("users").Child(user.UserId).SetRawJsonValueAsync(json);

            messageText.text = "✅ Registro exitoso.";
        }
    }
}
