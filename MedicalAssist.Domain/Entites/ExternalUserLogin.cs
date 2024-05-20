﻿using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class ExternalUserLogin
{
    public UserId  UserId { get; private set; }
    public ProvidedKey ProvidedKey { get; private set; }
    public Provider  Provider { get; private set; }
    protected ExternalUserLogin() { }
    internal ExternalUserLogin(UserId userId, ProvidedKey providedKey,Provider provider)
    {
        UserId = userId;
        ProvidedKey = providedKey;
        Provider = provider;
    }

}