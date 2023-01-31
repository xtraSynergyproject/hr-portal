using System;
using System.Threading.Tasks;

namespace CMS.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var migration = new DmsMigration();
            await migration.Extract();
            await migration.Transform();
            //await migration.Load();
        }
    }
}
