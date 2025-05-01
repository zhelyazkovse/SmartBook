using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
        class Helpers
        {
             public static string ReadInput()
            {
                return Console.ReadLine() ?? "";
            }

         public static string GetSavedFolderPath()
           {
            string rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            return Path.Combine(rootDirectory, "Saved");
           }
        }
    }

