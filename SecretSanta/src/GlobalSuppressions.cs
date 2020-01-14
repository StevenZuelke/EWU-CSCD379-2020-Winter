// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

//SecretSanta.Business
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "URL NOT URI STRING FINE", Scope = "member", Target = "~P:SecretSanta.Business.Gift.Url")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "URL NOT URI STRING FINE", Scope = "member", Target = "~M:SecretSanta.Business.Gift.#ctor(System.Int32,System.String,System.String,System.String,SecretSanta.Business.User)")]
//SecretSanta.Api
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Doesn't apply", Scope = "member", Target = "~M:SecretSanta.Api.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
//SecretSanta.Web
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Doesn't apply ", Scope = "member", Target = "~M:SecretSanta.Web.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:SecretSanta.Web.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
