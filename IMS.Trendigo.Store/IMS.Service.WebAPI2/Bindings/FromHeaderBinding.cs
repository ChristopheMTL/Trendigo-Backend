using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace IMS.Service.WebAPI2.Bindings
{
    public class FromHeaderBinding : HttpParameterBinding
    {
        private string name;

        public FromHeaderBinding(HttpParameterDescriptor parameter, string headerName)
            : base(parameter)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException("locale");
            }

            this.name = headerName;
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IEnumerable<string> values;
            if (actionContext.Request.Headers.TryGetValues(this.name, out values))
            {
                actionContext.ActionArguments[this.Descriptor.ParameterName] = values.FirstOrDefault();
            }

            var taskSource = new TaskCompletionSource<object>();
            taskSource.SetResult(null);
            return taskSource.Task;
        }
    }

    public abstract class FromHeaderAttribute : ParameterBindingAttribute
    {
        private string name;

        public FromHeaderAttribute(string headerName)
        {
            this.name = headerName;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new FromHeaderBinding(parameter, this.name);
        }
    }

    public class fromHeaderAttribute : FromHeaderAttribute
    {
        public fromHeaderAttribute()
            : base("locale")
        {
        }
    }
}