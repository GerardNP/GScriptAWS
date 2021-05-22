using GScriptNuget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GScript.Repositories
{
    public interface IRepositoryScripts
    {
        #region USERS
        void Signup(String name, String email, String password);
        User LogIn(String email, String password);
        #endregion
    }

}
