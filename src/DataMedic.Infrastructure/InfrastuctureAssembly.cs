using System.Reflection;

namespace DataMedic.Infrastructure;

public static class InfrastructureAssembly
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
