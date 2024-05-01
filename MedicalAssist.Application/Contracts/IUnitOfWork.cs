﻿namespace MedicalAssist.Application.Contracts;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
