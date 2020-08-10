using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace BlazorAuth0Demo.Shared
{
    public class Auth0UserMetaData
    {
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
    }
}
