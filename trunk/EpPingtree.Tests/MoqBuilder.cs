using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Moq;

namespace EpPingtree.Tests
{
    public class MoqBuilder
    {
        /// <summary>
        /// This create a mock where all properties call the base class
        /// </summary>
        /// <typeparam name="TInterface">An instance of the concrete class to call</typeparam>
        /// <returns></returns>
        public static Mock<TInterface> CreateMockAllPropsCallBase<TInterface>(TInterface concreteClass) where TInterface : class
        {
            Type type = typeof(TInterface);
            PropertyInfo[] propertyInfos = type.GetProperties();

            Mock<TInterface> mock = new Mock<TInterface>();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType == typeof(string))
                    SetupMockForParam<TInterface, string>(mock, propertyInfo, concreteClass);

                else if (propertyInfo.PropertyType == typeof(bool))
                    SetupMockForParam<TInterface, bool>(mock, propertyInfo, concreteClass);

                else if (propertyInfo.PropertyType == typeof(double))
                    SetupMockForParam<TInterface, double>(mock, propertyInfo, concreteClass);

                else if (propertyInfo.PropertyType == typeof(int))
                    SetupMockForParam<TInterface, int>(mock, propertyInfo, concreteClass);

                else
                    throw new Exception("Add a mock type for " + propertyInfo.PropertyType.Name + " in MoqBuilder as its not currently mocked");
            }

            return mock;
        }


        private static void SetupMockForParam<TInterface, TReturnType>(Mock<TInterface> mock, PropertyInfo propertyInfo, TInterface concreteClass) where TInterface : class
        {
            ParameterExpression objParamForGet = Expression.Parameter(typeof(TInterface));
            MemberExpression exprObject = Expression.Property(objParamForGet, propertyInfo);
            Expression<Func<TInterface, TReturnType>> lambda1 = Expression.Lambda<Func<TInterface, TReturnType>>(exprObject, new ParameterExpression[] { objParamForGet });

            //Create a func that will get the value just in time
            Func<TReturnType> getValue = () => (TReturnType)propertyInfo.GetValue(concreteClass, null);
            mock.SetupGet(lambda1).Returns(getValue);
        }
    }
}

