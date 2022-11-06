﻿using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace MovieCatalogBackend.Services.Auth;

public interface IPasswordHasher : IPasswordHasher<object> { } 

public class SimplePasswordHasher : IPasswordHasher
{
    private SHA256 _hasher = SHA256.Create();
    public SimplePasswordHasher() { }
    
    public string HashPassword(object user, string password)
    {
        return Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(password)));
    }

    public PasswordVerificationResult VerifyHashedPassword(object user, string hashedPassword, string providedPassword)
    {
        return HashPassword(user, providedPassword) == hashedPassword
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}