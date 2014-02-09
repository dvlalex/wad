using System;
using System.Web.Security;
using WebMatrix.WebData;
using data.entity;

namespace data.service
{
    public interface IMembershipService
    {
        Boolean IsAuthenticated { get; }
        String CreateUserAndLogin();
        Boolean Logout();
        int GetUserSessionId(String userhash);
    }

    public class MembershipService : IMembershipService
    {
        private readonly IUserSessionService _userSessionService;

        public MembershipService(IUserSessionService userSessionService)
        {
            if (userSessionService == null)
                throw new ArgumentNullException("userSessionService");

            _userSessionService = userSessionService;
        }

        public bool IsAuthenticated
        {
            get { return WebSecurity.IsAuthenticated; }
        }

        public string CreateUserAndLogin()
        {
            var userHash = Membership.GeneratePassword(15, 7);

            try
            {
                WebSecurity.CreateUserAndAccount(userHash, userHash + "pwdD");
                WebSecurity.Login(userHash, userHash + "pwdD", true);

                return userHash;
            }
            catch (MembershipCreateUserException e)
            {
                if(e.StatusCode == MembershipCreateStatus.DuplicateUserName)
                {
                    //do another try with diff userhash
                    return CreateUserAndLogin();
                }
            }

            return null;
        }

        public bool Logout()
        {
            WebSecurity.Logout();
            return true;
        }

        public int GetUserSessionId(string userhash)
        {
            var session = _userSessionService.RetrieveUserSession(userhash);
            return session.UserId;
        }

        public UserSession GetUserSession(string userhash)
        {
            return _userSessionService.RetrieveUserSession(userhash);
        }
    }
}
