using System;

[Serializable]
public class UserData  // Se cambió de 'User' a 'UserData' para evitar conflictos
{
    public string userId;
    public string email;

    public UserData(string userId, string email)
    {
        this.userId = userId;
        this.email = email;
    }
}
