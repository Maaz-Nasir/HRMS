#pragma checksum "E:\Hrms\Hrms\Hrms\Views\Subscriptions\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fed59e4b10afdbbbf9ae277bfd408f61b944302d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Subscriptions_Index), @"mvc.1.0.view", @"/Views/Subscriptions/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\Hrms\Hrms\Hrms\Views\_ViewImports.cshtml"
using Hrms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Hrms\Hrms\Hrms\Views\_ViewImports.cshtml"
using Hrms.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fed59e4b10afdbbbf9ae277bfd408f61b944302d", @"/Views/Subscriptions/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"69583a24c156ca7a725f8df82b95060522fc5810", @"/Views/_ViewImports.cshtml")]
    public class Views_Subscriptions_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            DefineSection("LinkScriptSection", async() => {
                WriteLiteral(@"
    <script type=""text/javascript"">
        $(document).ready(function () {
            var columns = [
                { ""data"": ""id"", ""name"": ""ID"" },
                { ""data"": ""code"", ""name"": ""Code"" },
                { ""data"": ""title"", ""name"": ""Title"" },
                { ""data"": ""country"", ""name"": ""Country"" },
                { ""data"": ""timeZoneId"", ""name"": ""TimeZoneId"" },
                { ""data"": ""startDate"", ""name"": ""StartDate"" },
                { ""data"": ""endDate"", ""name"": ""EndDate"" },
                { ""data"": ""noOfMonth"", ""name"": ""NoOfMonth"" },
                { ""data"": ""status"", ""name"": ""Status"" },
                {
                    ""render"": function (data, type, row) { return ActionButton(data, type, row) }
                }
            ];
            ServerSideDataTable(columns);
        });
    </script>
");
            }
            );
            WriteLiteral(@"
<div class=""col-md-12 mb-4"">
    <div class=""card text-left"">
        <div class=""card-header"">
            <div class=""row"">
                <div class=""col-md-8"">
                    <h4 class=""text-left"">Subscriptions</h4>
                </div>
                <div class=""col-md-4"">
                    <a");
            BeginWriteAttribute("href", " href=\"", 1206, "\"", 1233, 2);
#nullable restore
#line 31 "E:\Hrms\Hrms\Hrms\Views\Subscriptions\Index.cshtml"
WriteAttributeValue("", 1213, ViewBag.PageURL, 1213, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1229, "/add", 1229, 4, true);
            EndWriteAttribute();
            WriteLiteral(@" class=""btn btn-primary float-right""><i class=""fa fa-plus""></i> | Add New</a>
                </div>
            </div>
        </div>
        <div class=""card-body"">
            <div class=""table-responsive"">
                <table class=""display table table-striped table-hover table-bordered DataGrid"" style=""width:100%"">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Code</th>
                            <th>Name</th>
                            <th>Country</th>
                            <th>Time Zone</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>No Of Month</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    ");
#nullable restore
#line 52 "E:\Hrms\Hrms\Hrms\Views\Subscriptions\Index.cshtml"
               Write(await Html.PartialAsync("DataTable"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </table>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591