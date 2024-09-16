﻿using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.User.Commands.SignIn;

namespace MedicalAssistant.Application.User.Commands.ExternalAuthentication;
internal sealed record ExternalAuthenticationCommand(
	ExternalApiResponse? response,
	string Provider) : IRequest<SignInResponse>;