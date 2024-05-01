﻿using MediatR;

namespace MedicalAssist.Application.User.Commands.VerifyAccount;
public sealed record VerifyAccountCommand(
	string CodeHash) : IRequest;

