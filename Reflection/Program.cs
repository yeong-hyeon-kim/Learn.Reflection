using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using Reflection.Models;

namespace Reflection
{
    public class Computer
    {
        static public void Booting()
        {
            Console.WriteLine("Booting..");
        }

        static public void Login(string ID, string Password)
        {
            Console.WriteLine($"ID : {ID}, PW : {Password}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /* 네임스페이스.클래스명으로 인스턴스 생성하기 */
            Console.WriteLine(CreateInstance("Reflection.Models.User"));
            /* 네임스페이스.클래스명으로 특정 프로퍼티 값 가져오기 */
            Console.WriteLine(GetPropertyValue("Reflection.Models.User", "Email"));
            /* 네임스페이스.클래스명으로 프로퍼티 값 설정하기 */
            SetPropretysValue("Reflection.Models.User", new Dictionary<string, object> {
                { "Number", 1},
                { "Name", "New Name" },
                { "Email", "Test@email.com" },
                { "Address", "New Address" },
                { "BirthDate", new DateTime{} },
                { "Contact", "New Contact" },
            });
            /* 네임스페이스.클래스명으로 특정 프로퍼티 값 설정하기 */
            SetPropertyValue("Reflection.Models.User", "Number", 2);
            /* 타입으로 프로퍼티 값 가져오기 */
            User users = new User();
            users.Name = "Names";
            var ListValue = GetPropretysValue("Reflection.Models.User", users);

            foreach (var item in ListValue)
            {
                Console.WriteLine(item);
            }

            /* 타입으로 메서드 호출하기 */
            object obj = CreateInstance("Reflection.Computer");
            // 매개 변수가 없을 경우
            CallMethod(obj, obj.GetType().GetMethod("Booting"), null);
            // 매개 변수가 있을 경우
            // 2개 이상일 경우
            CallMethod(obj, obj.GetType().GetMethod("Login"), new object[] { "ADMIN", "1234" });
        }

        /// <summary>
        /// 타입으로 객체를 생성합니다.
        /// </summary>
        /// <param name="TypeString">네임스페이스(Namespace).클래스(Class)</param>
        static object CreateInstance(string TypeString)
        {
            Type Types = Type.GetType(TypeString);
            return Activator.CreateInstance(Types);
        }

        #region 메소드 Methods
        #nullable enable
        /// <summary>
        /// 타입으로 메소드 호출합니다.
        /// </summary>
        /// <param name="Instance">타입 인스턴스</param>
        /// <param name="Method">메소드</param>
        /// <param name="Parameter">메소드 매개변수</param>
        static void CallMethod(object? Instance, MethodInfo Method, object?[]? Parameter)
        {
            Method.Invoke(Instance, Parameter);
        }

        #endregion

        #region 속성 Propertys

        /// <summary>
        /// 특정 프로퍼티 값을 변경합니다.
        /// </summary>
        /// <param name="TypeString">네임스페이스(Namespace).클래스(Class)</param>
        /// <param name="PropertyName">클래스(Class)에 속한 속성명(Property)</param>
        /// <param name="SetValue">속성 값을 변경할 값</param>
        static void SetPropertyValue(string TypeString, string PropertyName, object SetValue, object TypeInstance = null)
        {
            try
            {
                object Instance;
                Type Types;

                ReturnType(TypeString, TypeInstance, out Types, out Instance);

                PropertyInfo[] Properties = Types.GetProperties();

                foreach (var Property in Properties)
                {
                    if (Property.Name.Equals(PropertyName))
                    {
                        Property.SetValue(Instance, SetValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 특정 프로퍼티 값을 조회합니다.
        /// </summary>
        /// <param name="TypeString">네임스페이스(Namespace).클래스(Class)</param>
        /// <param name="PropertyName">클래스(Class)에 속한 속성명(Property)</param>
        /// <param name="TypeInstance">인스턴스(Instance)</param>
        static object GetPropertyValue(string TypeString, string PropertyName, object TypeInstance = null)
        {
            object Objet = null;

            try
            {
                object Instance;
                Type Types;

                ReturnType(TypeString, TypeInstance, out Types, out Instance);

                PropertyInfo[] Properties = Types.GetProperties();

                foreach (var Property in Properties)
                {
                    if (Property.Name.Equals(PropertyName))
                    {
                        Objet = Property.GetValue(Instance);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Objet;
        }

        /// <summary>
        /// 프로퍼티들의 값을 조회합니다.
        /// </summary>
        /// <param name="TypeString">네임스페이스(Namespace).클래스(Class)</param>
        /// <returns></returns>
        static Dictionary<string, object> GetPropretysValue(string TypeString, object TypeInstance = null)
        {
            Dictionary<string, object> Objet = new Dictionary<string, object>();

            try
            {
                object Instance;
                Type Types;

                ReturnType(TypeString, TypeInstance, out Types, out Instance);

                // 프로퍼티가 속한 클래스의 인스턴스가 필요. 
                PropertyInfo[] Properties = Types.GetProperties();

                foreach (var Property in Properties)
                {
                    Objet.Add(Property.Name, Property.GetValue(Instance));
                }

                return Objet;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Objet;
        }

        /// <summary>
        /// 프로퍼티들의 값을 변경합니다.
        /// </summary>
        /// <param name="TypeString">네임스페이스(Namespace).클래스(Class)</param>
        /// <param name="PropertyNameValuePair">Key : 속성(Property), Value : 값(Value)</param>
        static void SetPropretysValue(string TypeString, Dictionary<string, object> PropertyNameValuePair, object TypeInstance = null)
        {
            try
            {
                object Instance;
                Type Types;

                ReturnType(TypeString, TypeInstance, out Types, out Instance);

                PropertyInfo[] Properties = Types.GetProperties();

                foreach (var Property in Properties)
                {
                    Property.SetValue(Instance, PropertyNameValuePair[Property.Name]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion
        /// <summary>
        /// 클래스 타입과 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="ClassType">네임스페이스(Namespace).클래스(Class)</param>
        /// <param name="InstanceType">인스턴스(Instance)</param>
        /// <param name="FinalClassType">반환 클래스 타입</param>
        /// <param name="FinalInstanceType">반환 인스턴스(Instance)</param>
        static void ReturnType(string ClassType, object InstanceType, out Type FinalClassType, out object FinalInstanceType)
        {
            if (InstanceType == null)
            {
                // 생성된 인스턴스에 접근
                FinalClassType = Type.GetType(ClassType);
                FinalInstanceType = Activator.CreateInstance(FinalClassType);
            }
            else
            {
                // 인스턴스를 새로 생성하여 접근
                FinalClassType = InstanceType.GetType();
                FinalInstanceType = InstanceType;
            }
        }
    }
}