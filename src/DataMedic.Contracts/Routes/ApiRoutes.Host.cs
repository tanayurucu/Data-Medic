namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class Hosts
    {
        public const string GetWithPagination = "hosts";
        public const string GetAll = "hosts/all";
        public const string GetById = "hosts/{hostId:guid}";
        public const string Create = "hosts";
        public const string Update = "hosts/{hostId:guid}";
        public const string Delete = "hosts/{hostId:guid}";
        public const string GetPortainerHosts = "hosts/{hostId:guid}/portainer/hosts";
        public const string GetPortainerContainers =
            "hosts/{hostId:guid}/portainer/hosts/{portainerHostId:int}/containers";
        public const string GetKafkaTopics = "hosts/{hostId:guid}/kafka/topics";
    }
}
