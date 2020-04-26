﻿using Library.Infrastructure.Library.Interface.Services;
using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.EntityTranslators
{
    public abstract class BaseTranslator : IEntityTranslator
    {
        public abstract bool CanTranslate(Type targetType, Type sourceType);

        public bool CanTranslate<TTarget, TSource>()
        {
            return CanTranslate(typeof(TTarget), typeof(TSource));
        }

        public TTarget Translate<TTarget>(IEntityTranslatorService service, object source)
        {
            return (TTarget)Translate(service, typeof(TTarget), source);
        }

        public abstract object Translate(IEntityTranslatorService service, Type targetType, object source);

        public abstract object Translate(IEntityTranslatorService service, Type targetType, object source, object param);
    }
}
