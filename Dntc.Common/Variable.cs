using Dntc.Common.Conversion;

namespace Dntc.Common;

public record Variable (TypeConversionInfo Type, string Name, int PointerDepth);
