using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GScript.Helpers
{
    public class PathProvider
    {
        IWebHostEnvironment environment;
        public enum Folders
        {
            Images = 0,
            Content = 1,
            Apps = 2,
            Users = 3,
        }
        public PathProvider(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public String MapPath(String filename, Folders folder)
        {
            String carpeta = folder.ToString();
            String path = "";

            if (folder == Folders.Images)
            {
                carpeta = "images";
                path = Path.Combine(this.environment.WebRootPath, carpeta, filename);

            }
            else if (folder == Folders.Content)
            {
                carpeta = "content";
                path = Path.Combine(this.environment.WebRootPath, "images", carpeta, filename);
            }
            else if (folder == Folders.Apps)
            {
                carpeta = "apps";
                path = Path.Combine(this.environment.WebRootPath, "images", carpeta, filename);

            }
            else if (folder == Folders.Users)
            {
                carpeta = "users";
                path = Path.Combine(this.environment.WebRootPath, "images", carpeta, filename);
            }

            return path;
        }
    }
}
