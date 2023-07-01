namespace DataMedic.Contracts.Routes;


public static partial class ApiRoutes
{
    public static class Portainer
    {
        public const string GetHosts = "portainer/hosts/all";
        public const string GetContainerDetailById = "portainer/{hostId:int}/{containerId}";
        public const string GetContainerList = "portainer/{hostId:int}";
    }
}