using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ParseCVREmails
{
    public class MultipartFormDataAttribute : ParameterBindingAttribute
    {
        private readonly Type _type;

        public MultipartFormDataAttribute(Type type)
        {
            _type = type;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == _type)
            {
                return new MultipartFormDataHttpParameterBinding(parameter, _type);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}