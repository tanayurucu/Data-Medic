using System.Reflection;

namespace DataMedic.Persistence;

public static class PersistenceAssembly
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}