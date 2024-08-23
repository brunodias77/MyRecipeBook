using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRB.Application.UseCases.Users.Login.External
{
    public interface IExternalLoginUseCase
    {
        Task<string> Execute(string name, string email);
    }
}