using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Services;
using Elsa.Services.Models;
using Polly;

namespace Elsa.Decorators
{
    public class ResilientWorkflowContextManager : IWorkflowContextManager
    {
        private readonly IWorkflowContextManager _workflowContextManager;
        private readonly IAsyncPolicy _loadContextPolicy;
        private readonly IAsyncPolicy _saveContextPolicy;

        public ResilientWorkflowContextManager(IWorkflowContextManager workflowContextManager, IAsyncPolicy? loadContextPolicy, IAsyncPolicy? saveContextPolicy)
        {
            _workflowContextManager = workflowContextManager;
            _loadContextPolicy = loadContextPolicy ?? Policy.Handle<Exception>().RetryAsync(5);
            _saveContextPolicy = saveContextPolicy ?? Policy.Handle<Exception>().RetryAsync(5);
        }

        public async ValueTask<object?> LoadContext(LoadWorkflowContext context, CancellationToken cancellationToken = default) =>
            await _loadContextPolicy.ExecuteAsync(async ct => await _workflowContextManager.LoadContext(context, ct), cancellationToken);

        public async ValueTask<string?> SaveContextAsync(SaveWorkflowContext context, CancellationToken cancellationToken = default) =>
            await _saveContextPolicy.ExecuteAsync(async ct => await _workflowContextManager.SaveContextAsync(context, ct), cancellationToken);
    }
}