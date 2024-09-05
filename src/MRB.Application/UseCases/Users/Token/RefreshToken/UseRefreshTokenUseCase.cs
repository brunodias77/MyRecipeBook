using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRB.Communication.Requests.Users.Token;
using MRB.Communication.Responses;
using MRB.Domain.Repositories;
using MRB.Domain.Security.Token;
using MRB.Domain.ValueObjects;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Users.Token.RefreshToken
{
    public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public UseRefreshTokenUseCase(ITokenRepository tokenRepository, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IUnitOfWork unitOfWork)
        {
            _tokenRepository = tokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseTokenJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _tokenRepository.Get(request.RefreshToken);

            if (refreshToken is null)
                throw new RefreshTokenNotFoundException();

            var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(MyRecipesBookRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
            if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
                throw new RefreshTokenExpiredException();

            var newRefreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = refreshToken.UserId
            };

            await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

            await _unitOfWork.Commit();

            return new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(refreshToken.User.Id),
                RefreshToken = newRefreshToken.Value
            };
        }
    }
}