using Microsoft.JSInterop;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;

namespace Edux.Web.Stuff.Rare.ReinforcedTypings;

public static class ReinforcedTypingsConfiguration
{
    public static void Configure(Reinforced.Typings.Fluent.ConfigurationBuilder builder)
    {
        builder.Substitute(typeof(Task), new RtSimpleTypeName("Promise<void>"));
        builder.Substitute(typeof(Task<>), new RtSimpleTypeName("Promise"));
        builder.Substitute(typeof(Task<IJSObjectReference>), new RtSimpleTypeName("Promise<any>"));
        builder.SubstituteGeneric(typeof(DotNetObjectReference<>), (_, _) => new RtSimpleTypeName("DotNet.DotNetObject"));
    }
}
