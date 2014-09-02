using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using ParseCVREmails.Controllers;

namespace ParseCVREmails
{
    public class MultipartFormDataHttpParameterBinding : HttpParameterBinding
    {
        private readonly Type _type; 

        public MultipartFormDataHttpParameterBinding(HttpParameterDescriptor descriptor, Type type) : base(descriptor)
        {
            _type = type;
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, 
            CancellationToken cancellationToken)
        {
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                return Task.FromResult(0);
            }

            var provider = new MultipartFormDataMemoryStreamProvider();


            return actionContext.Request.Content.ReadAsMultipartAsync(provider,cancellationToken).ContinueWith(t =>
            {

                var value = (MultipartFormData)Activator.CreateInstance(_type);

                // This illustrates how to get the file names.
                foreach (HttpContent file in provider.Files)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                }

                value.Files = provider.Files;
                
                // Show all the key-value pairs.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        Trace.WriteLine(string.Format("{0}: {1}", key, val));
                        var propertyInfo = _type.GetProperty(key, BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(value,val);
                        }
                    }
                }

                actionContext.ActionArguments.Add(Descriptor.ParameterName, value);

                return Task.FromResult(0);
            }, cancellationToken);
        }
    }
}