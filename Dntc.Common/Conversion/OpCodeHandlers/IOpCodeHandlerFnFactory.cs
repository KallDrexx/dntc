﻿using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal interface IOpCodeHandlerFnFactory
{
    IReadOnlyDictionary<Code, OpCodeHandlerFn> Get();
}