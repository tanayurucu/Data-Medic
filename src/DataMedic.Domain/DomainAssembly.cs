using System.Reflection;
namespace DataMedic.Domain;

public static class DomainAssembly
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}