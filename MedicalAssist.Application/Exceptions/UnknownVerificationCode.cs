﻿using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class UnknownVerificationCode : BadRequestException
{
    public UnknownVerificationCode() : base("User with given verification code was not found.")
    {
        
    }
}
