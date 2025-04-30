using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DecoratedHandlers.Gen;

public static class DebugEmitter
{
    public static SourceText CreateStatistics(
        IReadOnlyList<HandlerDescription> handlers,
        IReadOnlyList<BehaviorDescription> behaviors)
    {
        var sb = new StringBuilder(256);

        sb.AppendLine(
            $"""
             //Handlers count = {handlers.Count}
             //Behaviors count = {behaviors.Count}
             //
             //Now = {DateTime.Now}
             //
             //Handler list:
             """);
        
        foreach (var handler in handlers)
        {
            sb.AppendLine($"//{handler.HandlerTypeName}");
        }

        sb.AppendLine(
            $"""
             //
             //Behavior list:
             """);

        foreach (var beh in behaviors)
        {
            sb.AppendLine($"//{beh.TypeName}");
        }

        return SourceText.From(sb.ToString(), Encoding.UTF8);
    }
}
