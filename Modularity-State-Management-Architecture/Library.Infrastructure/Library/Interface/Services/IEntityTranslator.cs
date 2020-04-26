using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.Interface.Services
{
    public interface IEntityTranslator
    {
        bool CanTranslate(Type targetType, Type sourceType);
        bool CanTranslate<TTarget, TSource>();
        object Translate(IEntityTranslatorService service, Type targetType, object source);
        TTarget Translate<TTarget>(IEntityTranslatorService service, object source);
        object Translate(IEntityTranslatorService service, Type targetType, object source, object param);
    }
}
