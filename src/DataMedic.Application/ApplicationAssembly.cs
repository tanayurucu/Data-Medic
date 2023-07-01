using System.Reflection;

namespace DataMedic.Application;

public static class ApplicationAssembly
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
