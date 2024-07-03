using Microsoft.EntityFrameworkCore;
using ProjectC.Server.Data;
using ProjectC.Server.Data.Entities;
using ProjectC.Server.Services.Interfaces;
using System.Text.Json;

namespace ProjectC.Server.Services
{
    public class WorkflowStorageService : IWorkflowStorageService
    {
        private readonly string EMPTY_STORAGE_DATA = "[]";
        private readonly ProjectCDbContext context;

        public WorkflowStorageService(ProjectCDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WorkflowStorage>> GetAsync()
        {
            return await context.WorkflowStorage.ToArrayAsync();
        }

        public async Task<WorkflowStorage?> GetByWorkflowIdAsync(int workflowId)
        {
            return await context.WorkflowStorage.FirstOrDefaultAsync(
                x => x.WorkflowId == workflowId
            );
        }

        public async Task CreateAsync(WorkflowStorage workflowStorage)
        {
            workflowStorage.Data = EMPTY_STORAGE_DATA;
            context.WorkflowStorage.Add(workflowStorage);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkflowStorage workflowStorage)
        {
            var currentWorkflowStorage =
                await context.WorkflowStorage.FirstOrDefaultAsync(x => x.Id == workflowStorage.Id)
                ?? throw new Exception("Workflow storage not found");

            currentWorkflowStorage.PropertyIdentifier = workflowStorage.PropertyIdentifier;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workflowStorage = await context.WorkflowStorage.FirstOrDefaultAsync(
                x => x.Id == id
            );
            if (workflowStorage is not null)
            {
                context.WorkflowStorage.Remove(workflowStorage);
                await context.SaveChangesAsync();
            }
        }

        public async Task<string?> HandleRequestAsync(
            IQueryCollection queryParams,
            int workflowId,
            int requestEventId
        )
        {
            var workflowStorage = await context.WorkflowStorage.FirstOrDefaultAsync(
                x => x.WorkflowId == workflowId
            );
            var requestEvent = await context.RequestEvent.FirstOrDefaultAsync(
                x => x.Id == requestEventId
            );

            if (workflowStorage is not null && requestEvent is not null)
            {
                var querParamId = queryParams[workflowStorage.PropertyIdentifier].ToString();

                switch (requestEvent.Method)
                {
                    case "GET":
                        return HandleGetRequest(workflowStorage, querParamId);
                    case "POST":
                        await HandlePostRequest(workflowStorage, requestEvent);
                        break;
                    case "PUT":
                        await HandlePutRequest(workflowStorage, requestEvent, querParamId);
                        break;
                    case "DELETE":
                        await HandleDeleteRequest(workflowStorage, querParamId);
                        break;
                }
            }

            return null;
        }

        private string? HandleGetRequest(
            WorkflowStorage workflowStorage,
            string? queryParamId = null
        )
        {
            try
            {
                if (string.IsNullOrEmpty(workflowStorage.Data))
                {
                    workflowStorage.Data = EMPTY_STORAGE_DATA;
                }
                var storage = JsonSerializer.Deserialize<List<dynamic>>(workflowStorage.Data);
                if (storage is not null)
                {
                    if (!string.IsNullOrEmpty(queryParamId))
                    {
                        var item = storage.FirstOrDefault(
                            x =>
                                x.GetProperty(workflowStorage.PropertyIdentifier).ToString()
                                == queryParamId
                        );
                        if (item is not null)
                        {
                            return JsonSerializer.Serialize(item);
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task HandlePostRequest(
            WorkflowStorage workflowStorage,
            RequestEvent requestEvent
        )
        {
            try
            {
                if (string.IsNullOrEmpty(workflowStorage.Data))
                {
                    workflowStorage.Data = EMPTY_STORAGE_DATA;
                }
                var data = JsonSerializer.Deserialize<dynamic>(requestEvent.Body);
                var storage = JsonSerializer.Deserialize<List<dynamic>>(workflowStorage.Data);
                if (data is not null && storage is not null)
                {
                    var id = data.GetProperty(workflowStorage.PropertyIdentifier).ToString();
                    if (id is not null)
                    {
                        storage.Add(data);
                    }
                }
                workflowStorage.Data = JsonSerializer.Serialize(storage);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task HandlePutRequest(
            WorkflowStorage workflowStorage,
            RequestEvent requestEvent,
            string? queryParamId = null
        )
        {
            try
            {
                if (string.IsNullOrEmpty(workflowStorage.Data))
                {
                    workflowStorage.Data = EMPTY_STORAGE_DATA;
                }
                var data = JsonSerializer.Deserialize<dynamic>(requestEvent.Body);
                var storage = JsonSerializer.Deserialize<List<dynamic>>(workflowStorage.Data);
                if (data is not null && storage is not null)
                {
                    var id =
                        queryParamId
                        ?? data.GetProperty(workflowStorage.PropertyIdentifier).ToString();
                    if (id is not null)
                    {
                        var item = storage.FirstOrDefault(
                            x => x.GetProperty(workflowStorage.PropertyIdentifier).ToString() == id
                        );
                        if (item is not null)
                        {
                            storage.Remove(item);
                            storage.Add(data);
                        }
                    }
                }
                workflowStorage.Data = JsonSerializer.Serialize(storage);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task HandleDeleteRequest(
            WorkflowStorage workflowStorage,
            string? queryParamId = null
        )
        {
            try
            {
                if (string.IsNullOrEmpty(workflowStorage.Data))
                {
                    workflowStorage.Data = EMPTY_STORAGE_DATA;
                }
                var storage = JsonSerializer.Deserialize<List<dynamic>>(workflowStorage.Data);
                if (storage is not null)
                {
                    if (queryParamId is not null)
                    {
                        var item = storage.FirstOrDefault(
                            x =>
                                x.GetProperty(workflowStorage.PropertyIdentifier).ToString()
                                == queryParamId
                        );
                        if (item is not null)
                        {
                            storage.Remove(item);
                        }
                    }
                }
                workflowStorage.Data = JsonSerializer.Serialize(storage);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task ClearDataAsync(int id)
        {
            var workflowStorage = await context.WorkflowStorage.FirstOrDefaultAsync(
                x => x.Id == id
            );
            if (workflowStorage is not null)
            {
                workflowStorage.Data = EMPTY_STORAGE_DATA;
                await context.SaveChangesAsync();
            }
        }
    }
}
