﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slowsharp
{
    internal class OverloadingResolver
    {
        public static SSMethodInfo FindMethodWithArguments(
            TypeResolver resolver, SSMethodInfo[] members, HybInstance[] args)
        {
            foreach (var member in members)
            {
                if (member.target.isCompiled)
                {
                    var genericArgs = new List<HybType>();
                    var method = member.target.compiledMethod;
                    var ps = method.GetParameters();

                    if (args.Length != ps.Length)
                        continue;

                    bool match = true;
                    for (int i = 0; i < ps.Length; i++)
                    {
                        var p = ps[i].ParameterType;

                        if (args[i] == null)
                        {
                            if (p.IsValueType)
                            {
                                match = false;
                                break;
                            }
                            continue;
                        }

                        Type[] genericBound = null;
                        var argType = args[i].GetHybType();
                        if (!p.IsAssignableFromEx(argType, out genericBound))
                        {
                            match = false;
                            break;
                        }

                        if (p.IsGenericType || p.IsGenericTypeDefinition)
                        {
                            if (argType.isCompiledType)
                            {
                                genericArgs.AddRange(
                                    genericBound.Select(x => new HybType(x)));
                            }
                            else
                                genericArgs.Add(new HybType(typeof(HybInstance)));
                        }
                    }
                    if (match == false)
                        continue;

                    if (genericArgs.Count > 0)
                        return member.MakeGenericMethod(genericArgs.ToArray());
                    return member;
                }
                else
                {
                    var ps = member.target.interpretMethod.ParameterList.Parameters;

                    if (member.isVaArg == false &&
                        args.Length > ps.Count)
                        continue;

                    var match = true;
                    var count = 0;
                    foreach (var p in ps)
                    {
                        var paramType = resolver.GetType($"{p.Type}");

                        if (p.Modifiers.IsParams())
                            break;
                        if (args.Length <= count)
                        {
                            match = false;
                            break;
                        }

                        var argType = args[count++].GetHybType();

                        if (paramType.IsAssignableFrom(argType) == false)
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match == false)
                        continue;

                    return member;
                }
            }

            return null;
        }
    }
}