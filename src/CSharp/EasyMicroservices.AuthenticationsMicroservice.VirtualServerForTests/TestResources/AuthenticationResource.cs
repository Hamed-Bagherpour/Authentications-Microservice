﻿using System.Collections.Generic;
using System.Text.Json;

namespace EasyMicroservices.AuthenticationsMicroservice.VirtualServerForTests.TestResources
{
    public static class AuthenticationResource
    {
        public static Dictionary<string, string> GetResources(string microseviceName, Dictionary<string, List<TestServicePermissionContract>> customRoles = default)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (customRoles == null)
                customRoles = new Dictionary<string, List<TestServicePermissionContract>>();
            customRoles.TryAdd("Owner", new List<TestServicePermissionContract>()
            {
                new TestServicePermissionContract()
                {
                     ServiceName = "TestService",
                     MethodName = "TestMethod",
                     MicroserviceName =  microseviceName,
                }
            });
           
            foreach (var item in customRoles)
            {
                ExampleMessageContract response = new ExampleMessageContract()
                {
                    IsSuccess = true,
                    Result = item.Value
                };
                result.Add(@$"POST /api/ServicePermission/GetAllPermissionsBy HTTP/1.1
*RequestSkipBody*

{{""roleName"":""{item.Key}"",""microserviceName"":""TestExample""}}"
                ,
                @$"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{JsonSerializer.Serialize(response)}");
            }


            return result;
        }
    }

    public class TestServicePermissionContract
    {
        public string MicroserviceName { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
    }

    public class ExampleMessageContract
    {
        public bool IsSuccess { get; set; }
        public List<TestServicePermissionContract> Result { get; set; }
    }
}
