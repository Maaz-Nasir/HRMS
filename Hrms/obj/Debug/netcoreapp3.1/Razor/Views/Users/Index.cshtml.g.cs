#pragma checksum "E:\Hrms\Hrms\Hrms\Views\Users\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "36746ca25df201782cd817e9b5150d6cd2185191"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Users_Index), @"mvc.1.0.view", @"/Views/Users/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"36746ca25df201782cd817e9b5150d6cd2185191", @"/Views/Users/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"69583a24c156ca7a725f8df82b95060522fc5810", @"/Views/_ViewImports.cshtml")]
    public class Views_Users_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
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
                { ""data"": ""role"", ""name"": ""Role"" },
                { ""data"": ""name"", ""name"": ""Name"" },
                { ""data"": ""username"", ""name"": ""Username"" },
                { ""data"": ""email"", ""name"": ""Email"" },
                { ""data"": ""status"", ""name"": ""Status"" },
                { ""data"": ""createdDateTime"", ""name"": ""CreatedDateTime"" },
                { ""data"": ""updatedDateTime"", ""name"": ""UpdatedDateTime"" },
                {
                    ""render"": function (data, type, row) { return ActionButton(data, type, row) }
                }
");
                WriteLiteral("            ];\r\n            ServerSideDataTable(columns);\r\n        })\r\n    </script>\r\n");
            }
            );
            WriteLiteral(@"
<div class=""col-md-12 mb-4"">
    <div class=""card text-left"">
        <div class=""card-header"">
            <div class=""row"">
                <div class=""col-md-8"">
                    <h4 class=""text-left"">Users</h4>
                </div>
                <div class=""col-md-4"">
                    <a");
            BeginWriteAttribute("href", " href=\"", 2068, "\"", 2095, 2);
#nullable restore
#line 45 "E:\Hrms\Hrms\Hrms\Views\Users\Index.cshtml"
WriteAttributeValue("", 2075, ViewBag.PageURL, 2075, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2091, "/add", 2091, 4, true);
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
                            <th>Role</th>
                            <th>Name</th>
                            <th>Username</th>
                            <th>Email</th>
                            <th>Status</th>
                            <th>Created Date</th>
                            <th>Updated Date</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    ");
#nullable restore
#line 65 "E:\Hrms\Hrms\Hrms\Views\Users\Index.cshtml"
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
