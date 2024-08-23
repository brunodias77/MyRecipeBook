using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRB.Application.UseCases.Users.Login.External;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Security.Token;

namespace MRB.Application.UseCases.Users.Login.Google
{
    public class LoginGoogleUseCase : IExternalLoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public LoginGoogleUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<string> Execute(string name, string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user is null)
            {
                user = new User
                {
                    Name = name,
                    Email = email,
                    Password = "_"
                };
                await _userRepository.AddAsync(user);
                await _unitOfWork.Commit();
            }
            return _accessTokenGenerator.Generate(user.Id);
        }
    }
}