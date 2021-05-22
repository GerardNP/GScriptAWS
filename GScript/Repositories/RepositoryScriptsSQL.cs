using GScript.Data;
using GScript.Helpers;
using GScriptNuget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GScript.Repositories
{
    public class RepositoryScriptsSQL : IRepositoryScripts
    {
        private ScriptsContext context;

        public RepositoryScriptsSQL(ScriptsContext context)
        {
            this.context = context;
        }

        #region USERS
        private int GetMaxIdUsers()
        {
            int amount = this.context.Users.Count();
            if (amount == 0)
            {
                return 0;
            }
            else
            {
                return this.context.Users.Max(x => x.Id);
            }
        }

        public void Signup(String name, String email, String password)
        {
            User user = new User();
            user.Id = this.GetMaxIdUsers() + 1;
            user.Name = name;
            user.Email = email;
            String salt = "";
            byte[] cypherPass = CypherService.Cypher(password, ref salt);
            user.Salt = salt;
            user.Password = cypherPass;
            user.Role = "user";

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public User LogIn(String email, String password)
        {
            User user = this.context.Users.Where(x => x.Email == email)
                .FirstOrDefault();

            if (user == null) // USUARIO NO ENCONTRADO
            {
                return null;
            }
            else
            {
                String salt = user.Salt;
                byte[] temporalPass = CypherService.Cypher(password, salt);
                byte[] bddPass = user.Password;

                bool check = ToolkitService.EqualsBytesArray(temporalPass, bddPass);
                if (check) // USUARIO LOGUEADO 
                {
                    return user;
                }
                else // CONTRASEÑA NO VÁLIDA
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
