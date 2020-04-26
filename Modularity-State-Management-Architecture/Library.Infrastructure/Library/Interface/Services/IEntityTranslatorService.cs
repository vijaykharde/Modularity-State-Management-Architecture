using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.Interface.Services
{
    public interface IEntityTranslatorService
    {
        bool CanTranslate(Type targetType, Type sourceType);
        bool CanTranslate<TTarget, TSource>();
        object Translate(Type targetType, object source);
        object Translate(Type targetType, Type sourceType, object source);
        TTarget Translate<TTarget>(object source);
        TTarget Translate<TTarget>(object source, object param);
        TTarget Translate<TTarget, TSource>(object source);
        void RegisterEntityTranslator(IEntityTranslator translator);
        void RemoveEntityTranslator(IEntityTranslator translator);
    }
}
