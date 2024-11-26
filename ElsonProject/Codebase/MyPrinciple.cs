using ElsonProject.Models;
using System;
using System.Security.Principal;

namespace ElsonProject
{
    [Serializable]

    public class MyPrincipal : IPrincipal
    {
        public UsersModel User { get; set; }
        public MyPrincipal(IIdentity identity, UsersModel user)
        {
            Identity = identity;
            User = user;
        }
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return User.AcctType; }
    }

    public static class IdentityEx
    {
        public static int GetUserId(this IPrincipal principal)
        {
            var myPrincipal = principal as MyPrincipal;

            if (myPrincipal != null) return myPrincipal.User.Id;

            throw new Exception("Unable to cast IPrincipal to MyPrincipal");
        }
        public static string GetUsername(this IPrincipal principal)
        {
            var myPrincipal = principal as MyPrincipal;

            if (myPrincipal != null) return myPrincipal.User.Username;

            throw new Exception("Unable to cast IPrincipal to MyPrincipal");
        }

        public static int GetUserType(this IPrincipal principal)
        {
            var myPrincipal = principal as MyPrincipal;

            if (myPrincipal != null) return myPrincipal.User.UserType;

            throw new Exception("Unable to cast IPrincipal to MyPrincipal");
        }
        public static string GetPassword(this IPrincipal principal)
        {
            var myPrincipal = principal as MyPrincipal;

            if (myPrincipal != null) return myPrincipal.User.Password;

            throw new Exception("Unable to cast IPrincipal to MyPrincipal");
        }

        public static int GetUserRole(this IPrincipal principal)
        {
            var myPrincipal = principal as MyPrincipal;

            if (myPrincipal != null) return myPrincipal.User.RoleId;

            throw new Exception("Unable to cast IPrincipal to MyPrincipal");
        }

    }
}