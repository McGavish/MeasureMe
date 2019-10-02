using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Pmp.Camera.Lib
{
    public static class PropertyHelper
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", nameof(memberExpression));
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", nameof(property));
            }

            return memberExpression.Member.Name;
        }

        public static void RaisePropertyChanged<T, R>(this T src, Expression<Func<R>> propertyExpression, PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged
        {
            if (handler != null)
            {
                handler(src, new PropertyChangedEventArgs(ExtractPropertyName(propertyExpression)));
            }
        }
    }
}
