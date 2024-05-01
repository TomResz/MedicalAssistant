﻿using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;

namespace MedicalAssist.Infrastructure.Auth;
internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserContext(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public UserId GetUserId
		=> _contextAccessor
			.HttpContext?
			.User
			.GetUserId() ??
		throw new AuthenticationException("User context is unavailable");

	public bool IsAuthenticated => 
		_contextAccessor
			.HttpContext?
			.User
			.Identity?
			.IsAuthenticated ??
		throw new AuthenticationException("User context is unavailable");
}
