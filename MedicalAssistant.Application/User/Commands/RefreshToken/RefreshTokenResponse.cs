﻿namespace MedicalAssistant.Application.User.Commands.RefreshToken;
public sealed record RefreshTokenResponse (
    string AccessToken,
    string RefreshToken);