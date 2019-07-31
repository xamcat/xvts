using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Toolbox.Portable.Mvvm
{
    public class Binding : IDisposable
    {
        private Dictionary<INotifyPropertyChanged, PropertyChangedEventHandler> Handlers
        = new Dictionary<INotifyPropertyChanged, PropertyChangedEventHandler>();

        private bool _unbound = false;

        public static Binding Create<T>(Expression<Func<T>> expr, Action<Action> safeRunAction = null)
        {
            if (expr.Body.NodeType != ExpressionType.Equal)
                throw new NotSupportedException("Only equality bindings are supported");

            var b = (BinaryExpression)expr.Body;
            var m1 = b.Left;
            var m2 = b.Right;

            var m1Object = GetParentValue(m1);
            var m1PropName = GetPropertyName(m1);
            var m2Object = GetParentValue(m2);
            var m2PropName = GetPropertyName(m2);

            var binding = new Binding();

            if (m1Object is INotifyPropertyChanged npc1)
            {
                PropertyChangedEventHandler del1 = (object sender, PropertyChangedEventArgs args) =>
                {
                    if (args.PropertyName == m1PropName)
                    {
                        Action updateAction = () =>
                        {
                            var valueToSet = GetValue(sender, m1PropName);
                            var ti = m2Object?.GetType().GetTypeInfo();

                            GetProperty(ti, m2PropName)?.SetValue(m2Object, valueToSet);
                            GetField(ti, m2PropName)?.SetValue(m2Object, valueToSet);
                        };

                        if (safeRunAction != null)
                            safeRunAction.Invoke(updateAction);
                        else
                            updateAction.Invoke();
                    }
                };

                npc1.PropertyChanged += del1;
                binding.Handlers[npc1] = del1;
            }

            if (m2Object is INotifyPropertyChanged npc2)
            {
                PropertyChangedEventHandler del2 = (object sender, PropertyChangedEventArgs args) =>
                {
                    if (args.PropertyName == m2PropName)
                    {
                        Action updateAction = () =>
                        {
                            var valueToSet = GetValue(sender, m2PropName);

                            var ti = m1Object?.GetType().GetTypeInfo();

                            GetProperty(ti, m1PropName)?.SetValue(m1Object, valueToSet);
                            GetField(ti, m1PropName)?.SetValue(m1Object, valueToSet);
                        };

                        if (safeRunAction != null)
                            safeRunAction.Invoke(updateAction);
                        else
                            updateAction.Invoke();
                    }
                };

                npc2.PropertyChanged += del2;
                binding.Handlers[npc2] = del2;
            }

            return binding;
        }

        public static Binding Create<T>(Expression<Func<T>> expr, Action action)
        {
            if (expr.Body.NodeType != ExpressionType.MemberAccess)
                throw new NotSupportedException("Only member access bindings are supported");

            var mObject = GetParentValue(expr.Body);
            var mPropName = GetPropertyName(expr.Body);

            var binding = new Binding();

            if (mObject is INotifyPropertyChanged npc)
            {
                PropertyChangedEventHandler del = (object sender, PropertyChangedEventArgs args) =>
                {
                    if (args.PropertyName == mPropName)
                    {
                        action?.Invoke();
                    }
                };

                npc.PropertyChanged += del;
                binding.Handlers[npc] = del;
            }

            return binding;
        }

        public void Unbind()
        {
            if (_unbound) return;

            foreach (var handler in Handlers)
            {
                handler.Key.PropertyChanged -= handler.Value;
            }

            _unbound = true;
        }

        private static object GetParentValue(Expression prop1)
        {
            var e = ((MemberExpression)prop1).Expression;

            if (e is ConstantExpression eC)
            {
                return eC.Value;
            }

            var v = ((ConstantExpression)((MemberExpression)e).Expression).Value;
            var name = ((MemberExpression)e).Member.Name;

            var ti = v?.GetType().GetTypeInfo();
            return GetProperty(ti, name)?.GetValue(v) ?? GetField(ti, name)?.GetValue(v);
        }

        private static PropertyInfo GetProperty(TypeInfo typeInfo, string propertyName)
        {
            var propertyInfo = typeInfo?.GetDeclaredProperty(propertyName);
            if (propertyInfo == null && typeInfo?.BaseType != null)
            {
                propertyInfo = GetProperty(typeInfo.BaseType.GetTypeInfo(), propertyName);
            }
            return propertyInfo;
        }

        private static FieldInfo GetField(TypeInfo typeInfo, string fieldName)
        {
            var fieldInfo = typeInfo?.GetDeclaredField(fieldName);
            if (fieldInfo == null && typeInfo?.BaseType != null)
            {
                fieldInfo = GetField(typeInfo.BaseType.GetTypeInfo(), fieldName);
            }
            return fieldInfo;
        }

        private static string GetPropertyName(Expression prop1)
        {
            return ((MemberExpression)prop1)?.Member?.Name;
        }

        private static object GetValue<T>(T sender, string propertyName)
        {
            return sender.GetType().GetTypeInfo().GetDeclaredProperty(propertyName)?.GetValue(sender);
        }

        public void Dispose()
        {
            Unbind();
            Handlers = null;
            GC.SuppressFinalize(this);
        }
    }
}
