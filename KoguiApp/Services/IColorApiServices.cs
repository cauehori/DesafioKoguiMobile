using KoguiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace KoguiApp.Services
{
    public interface IColorApiService
    {
        Task<string> GetColorNameAsync(string hex);
    }
}
