using GScriptNuget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GScript.Services
{
    public class ServiceScripts
    {
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceScripts(String url)
        {
            this.UriApi = new Uri(url);
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApi<T>(String request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = 
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }


        #region USERS
        public async Task<User> GetUserAsync(int id)
        {
            String request = $"api/users/getuser/{id}";
            return await this.CallApi<User>(request);
        }

        public async Task<bool> ExistsUserAsync(String email)
        {
            String request = $"api/users/existsuser/{email}";
            return await this.CallApi<bool>(request);
        }

        public async Task<User> LoginAsync(String email, String password)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "api/users/login";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                User u = new User // CHAPUZILLA PARA OBTENER LA PASS
                {
                    Email = email,
                    Name = password
                };
                String json = JsonConvert.SerializeObject(u);
                StringContent content =
                                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync(request, content);
                User user = await res.Content.ReadAsAsync<User>();
                return user;
            }
        }

        public async Task SignUpAsync(String name, String email, String password)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "api/users/login";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                User u = new User // CHAPUZILLA PARA OBTENER LA PASS
                {
                    Name = name,
                    Email = email,
                    Salt = password
                };
                String json = JsonConvert.SerializeObject(u);
                StringContent content =
                                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        #endregion



        #region APPS
        public async Task<List<App>> GetAppsAsync()
        {
            String request = "api/apps";
            return await this.CallApi<List<App>>(request);
        }

        public async Task<List<App>> GetAppsAsync(List<int> ids)
        {
            String request = "api/apps/GetAppsByIds?";
            String data = "";
            foreach (int id in ids)
            {
                data += $"id={id}&";
            }
            data = data.Trim('&');
            return await this.CallApi<List<App>>(request + data);
        }

        public async Task<App> GetAppAsync(int id)
        {
            String request = $"api/apps/{id}";
            return await this.CallApi<App>(request);
        }

        public async Task DeleteAppAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = $"api/apps/{id}";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                await client.DeleteAsync(request);
            }
        }

        public async Task PostAppAsync(String name, String exeName, String path, String icon)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "api/apps";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                App app = new App
                {
                    Name = name,
                    ExeName = exeName,
                    Path = path,
                    Icon = icon
                };
                String json = JsonConvert.SerializeObject(app);
                StringContent content =
                                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }

        public async Task PutAppAsync(int id, String name, String exeName,
            String path, String icon)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "api/apps";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                App app = new App
                {
                    Id = id,
                    Name = name,
                    ExeName = exeName,
                    Path = path,
                    Icon = icon
                };
                String json = JsonConvert.SerializeObject(app);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
        #endregion


        #region SCRIPTS
        //public async Task<List<App>> GetScriptsAsync()
        //{
        //    String request = "api/scripts";
        //    return await this.CallApi<List<App>>(request);
        //}

        public async Task<List<Script>> GetScriptsAsync(int id)
        {
            String request = $"api/scripts/{id}";
            return await this.CallApi<List<Script>>(request);
        }

        public async Task DeleteScriptAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = $"api/scripts/{id}";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                await client.DeleteAsync(request);
            }
        }

        public async Task PostScriptAsync(String name, String script, int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "api/scripts";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Script s = new Script
                {
                    Name = name,
                    Code = script,
                    UserId = userId
                };
                String json = JsonConvert.SerializeObject(s);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }
        #endregion


        #region SCRIPTS FILE
        public async Task<String> GetAppsScriptAsync(bool showConsole, List<int> ids, String dir)
        {
            String request = $"api/ScriptsFile/CreateApssScript/{showConsole}/";
            String data = "";
            foreach(int id in ids)
            {
                data += $"id={id}&";
            }
            data = data.Trim('&');
            if(dir == null)
            {
                request += $"%20?{data}";
            } else
            {
                request += $"%20{dir}?{data}";
            }
            return await this.CallApi<String>(request);
        }

        public async Task<String> GetShutScriptAsync(String operation, String msg, int time)
        {
            String request = "";
            if (msg == null)
            {
                request = $"api/ScriptsFile/CreateShutScript/{operation}/{time}/%20";
            } else
            {
                request = $"api/ScriptsFile/CreateShutScript/{operation}/{time}/{msg}/";
            }
            return await this.CallApi<String>(request);
        }
        #endregion
    }
}
