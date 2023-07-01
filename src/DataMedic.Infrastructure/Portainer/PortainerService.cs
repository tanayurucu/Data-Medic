using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Models.Portainer;
using DataMedic.Infrastructure.Portainer;

using ErrorOr;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace DataMedic.Infrastructure.Services;

public sealed class PortainerService : IPortainerService
{
    private readonly HttpClient _httpClient;
    private readonly List<int> _hostList = Enumerable.Range(1, 12).ToList();
    public PortainerService(Uri baseAddress, string apiKey)
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = 
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
        _httpClient = new HttpClient(handler) { BaseAddress = baseAddress };
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }

    public async Task<ErrorOr<PortainerContainerStatus>> GetContainerInformationAsync(
        int portainerHostId,
        string containerId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/portainer/api/endpoints/{portainerHostId}/docker/containers/{containerId}/json",
                cancellationToken
            );
            if (response.IsSuccessStatusCode)
            {
                var containerStatusResponse = PortainerContainerDetailResponse.FromJson(
                    await response.Content.ReadAsStringAsync(cancellationToken)
                );
                return containerStatusResponse is null
                    ? Error.Failure(
                        code: "Host.Portainer.GetContainerInformationAsync.JsonConvert",
                        description: "Unable to deserialize response."
                    )
                    : new PortainerContainerStatus(
                        containerStatusResponse.Id,
                        containerStatusResponse.Name,
                        containerStatusResponse.State.Running,
                        containerStatusResponse.State.Status,
                        containerStatusResponse.State.StartedAt.DateTime
                    );
            }
            else
            {
                return Error.Failure(
                    code: "Host.Portainer.GetContainerInformationAsync",
                    description:
                    $"Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                );
            }
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "Host.Portainer.GetContainerInformationAsync.Exception",
                description:
                $"Exception occurred while requesting container information from portainer. Exception: {ex.Message}"
            );
        }
    }

    public async Task<ErrorOr<string>> GetContainerLogsAsync(
        int portainerHostId,
        string containerId,
        int lastLogCount = 5,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/portainer/api/endpoints/{portainerHostId}/docker/containers/{containerId}/logs?since=0&stderr=1&stdout=1&tail={lastLogCount}&timestamps=0",
                cancellationToken
            );
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsStringAsync(cancellationToken)
                : Error.Failure(
                    code: "Host.Portainer.GetContainerLogsAsync",
                    description:
                    $"Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                );
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "Host.Portainer.GetContainerLogsAsync.Exception",
                description:
                $"Exception occurred while requesting container logs from portainer. Exception: {ex.Message}"
            );
        }
    }

    public async Task<string> FindContainerLogsAsync(int portainerHostId, string containerId, int lastLogCount = 5,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/portainer/api/endpoints/{portainerHostId}/docker/containers/{containerId}/logs?since=0&stderr=1&stdout=1&tail={lastLogCount}&timestamps=0",
                cancellationToken
            );
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            else
            {
                Console.WriteLine($"FindContainerLogsAsync Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}");
                return "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while requesting container logs from portainer(FindContainerLogsAsync). Exception: {ex.Message}");
            return "";
        }
    }

    public async Task<ErrorOr<List<PortainerContainerInformation>>> GetContainersAsync(
        int portainerHostId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/portainer/api/endpoints/{portainerHostId}/docker/containers/json?all=1",
                cancellationToken
            );
            if (response.IsSuccessStatusCode)
            {
                var containersResponse = PortainerContainerResponse.FromJson(
                    await response.Content.ReadAsStringAsync(cancellationToken)
                );
                return containersResponse is null
                    ? Error.Failure(
                        code: "Host.Portainer.GetContainersAsync.JsonConvert",
                        description: "Unable to deserialize response."
                    )
                    : containersResponse
                        .Select(
                            containerResponse =>
                                new PortainerContainerInformation(
                                    containerResponse.Id,
                                    containerResponse.Names[0]
                                )
                        )
                        .ToList();
            }
            else
            {
                return Error.Failure(
                    code: "Host.Portainer.GetContainersAsync",
                    description:
                    $"Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                );
            }
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "Host.Portainer.GetContainersAsync.Exception",
                description: $"Exception occurred while requesting containers from portainer. Exception: {ex.Message}"
            );
        }
    }

    public async Task<ErrorOr<List<PortainerHostInformation>>> GetHostsAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync("/portainer/api/endpoints", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var hostsResponse = PortainerHostResponse.FromJson(
                    await response.Content.ReadAsStringAsync(cancellationToken)
                );
                return hostsResponse is null
                    ? Error.Failure(
                        code: "Host.Portainer.GetHostsAsync.JsonConvert",
                        description: "Unable to deserialize response."
                    )
                    : hostsResponse
                        .Select(
                            hostResponse =>
                                new PortainerHostInformation(hostResponse.Id, hostResponse.Name)
                        )
                        .ToList();
            }
            else
            {
                return Error.Failure(
                    code: "Host.Portainer.GetHostsAsync",
                    description:
                    $"Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                );
            }
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "Host.Portainer.GetHostsAsync.Exception",
                description: $"Exception occurred while requesting hosts from portainer. Exception: {ex.Message}"
            );
        }
    }

    public async Task<ErrorOr<List<ContainerInfoFromPortainer>>> GetDockerContainerStatusListAsync(long hostId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_hostList.Contains((int)hostId))
            {
                return Error.Failure(
                    code: "Host.Portainer.GetDockerContainerStatusListAsync",
                    description:
                    $"The Portainer HostId value must be in range of {_hostList.Min()} and {_hostList.Max()}"
                );
            }

            var response = await _httpClient.GetAsync(
                $"/portainer/api/endpoints/{hostId}/docker/containers/json?all=1",
                cancellationToken
            );
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseObject = JsonConvert.DeserializeObject<List<ContainerInfoFromPortainer>>(responseBody);
                if (responseObject != null)
                {
                    return responseObject;
                }
                else
                {
                    return Error.Failure(
                        code: "Host.Portainer.GetDockerContainerStatusListAsync",
                        description:
                        $"The response of the request couldn't be serialized. Response: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                    );
                }
            }
            else
            {
                return Error.Failure(
                    code: "Host.Portainer.GetDockerContainerStatusListAsync",
                    description:
                    $"Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}"
                );
            }
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "Host.Portainer.GetDockerContainerStatusListAsync.Exception",
                description: $"Exception occurred while requesting hosts from portainer. Exception: {ex.Message}"
            );
        }
    }

    public async Task<List<ContainerInfoFromPortainer>> FindDockerContainerStatusListAsync(long hostId,
        CancellationToken cancellationToken = default)
    {
        var returnData = new List<ContainerInfoFromPortainer>();
        if (!_hostList.Contains((int)hostId))
        {
            Console.WriteLine(
                $"The Portainer HostId value must be in range of {_hostList.Min()} and {_hostList.Max()}");
        }
        var response = await _httpClient.GetAsync(
            $"/portainer/api/endpoints/{hostId}/docker/containers/json?all=1",
            cancellationToken
        );
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseObject = JsonConvert.DeserializeObject<List<ContainerInfoFromPortainer>>(responseBody);
            if (responseObject != null)
            {
                return responseObject;
            }
            else
            {
                Console.WriteLine(
                    $"The response of the request couldn't be serialized. Response: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }
        }
        else
        {
            Console.WriteLine(
                $"Request was unsuccessful. Status Code: {response.StatusCode} Response: {await response.Content.ReadAsStringAsync(cancellationToken)}");
        }
        return returnData;
    }
}