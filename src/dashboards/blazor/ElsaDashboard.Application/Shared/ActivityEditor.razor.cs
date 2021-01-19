using System.Collections.Generic;
using System.Linq;
using Elsa.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ElsaDashboard.Application.Shared
{
    partial class ActivityEditor
    {
        private IDictionary<string, ActivityProperty> _properties = new Dictionary<string, ActivityProperty>();
        
        [Parameter] public ActivityInfo ActivityInfo { get; set; } = default!;

        [Parameter]
        public Variables? Properties
        {
            private get => default;
            set => _properties = value?.Data.ToDictionary(x => x.Key, x => new ActivityProperty
            {
                Value = x.Value?.ToString() ?? ""
            }) ?? new Dictionary<string, ActivityProperty>();
        }

        public Variables ReadProperties()
        {
            return new(_properties.ToDictionary(x => x.Key, x => x.Value.Value));
        }

        private EditContext EditContext { get; set; } = default!;

        protected override void OnInitialized()
        {
            EditContext = new EditContext(_properties);
        }

        protected override void OnParametersSet()
        {
            
        }

        private object? GetPropertyValue(string name) => _properties.ContainsKey(name) ? _properties[name] : default!;
    }

    public class ActivityProperty
    {
        public object Value { get; set; }
    }
}