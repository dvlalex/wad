using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data.entity;
using data.repository;

namespace data.service
{
    public interface IUserSessionService
    {
        void CreateUserSession(UserSession session);
        void UpdateUserSession(UserSession session);
        void DeleteUserSession(UserSession session);
        UserSession RetrieveUserSession(string hash);
    }

    public class UserSessionService : IUserSessionService
    {
        private readonly IUserSessionRepository _userSessionRepository;

        public UserSessionService(IUserSessionRepository userSessionRepository)
        {
            if (userSessionRepository == null)
                throw new ArgumentNullException("userSessionRepository");

            _userSessionRepository = userSessionRepository;
        }

        public void CreateUserSession(UserSession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _userSessionRepository.Add(session);
        }

        public void UpdateUserSession(UserSession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _userSessionRepository.Update(session);
        }

        public void DeleteUserSession(UserSession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _userSessionRepository.Delete(session);
        }

        public UserSession RetrieveUserSession(string hash)
        {
            return _userSessionRepository.Get(u => u.UserHash == hash);
        }
    }
}
