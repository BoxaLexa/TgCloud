using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgCloud
{
    /// <summary>
    /// Конекст бд приложения
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Создавет контекст
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
