using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace WebApiBasicTutorial.Helper
{
    public class AssemblyHelper
    {
        public static List<Assembly> GetAllAssemblies(SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<Assembly> assemblies = new List<Assembly>();

            foreach (string assemblyPath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", searchOption))
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

                    if (assemblies.Find(a => a == assembly) != null)
                        continue;

                    assemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return assemblies;
        }
    }
}
