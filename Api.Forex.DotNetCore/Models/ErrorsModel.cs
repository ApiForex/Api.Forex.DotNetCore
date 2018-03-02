using System;

namespace Api.Forex.DotNetCore.Models
{
    public class ErrorsModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}