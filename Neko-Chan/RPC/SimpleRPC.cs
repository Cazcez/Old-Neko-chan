using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.RPC
{
    class RPCModule : Attribute
    {

    }
    class SimpleRPC
    {
        static Dictionary<string, SearchResult> methodCache = new Dictionary<string, SearchResult>();
        static Dictionary<Type, object> registeredClasses = new Dictionary<Type, object>();
        public enum ExecutionStatus
        {
            ArgumentLenghtMismatch,
            Success,
            FailedForSomeReason,
            FunctionNotFound
        }
        public struct SearchResult
        {
            public bool isSuccess;
            public CallerObject[] Methods;
        }
        public struct CallerObject
        {
            public object Parent;
            public MethodInfo Method;
        }

        public static void RegisterClassInstance(object classToRegister)
        {
            registeredClasses.Add(classToRegister.GetType(), classToRegister);
        }
        public static ExecutionStatus ExecuteMethod(string methodName)
        {
            return SimpleRPC.ExecuteMethod(methodName, null);
        }
        public static ExecutionStatus ExecuteMethod(string methodName, params object[] args)
        {
            SearchResult targetMethod = FindMethod(methodName);
            if (!targetMethod.isSuccess)
            {
                return ExecutionStatus.FunctionNotFound;
            }
            int argLenght = args == null ? 0 : args.Length;
            foreach (var method in targetMethod.Methods)
            {
                if (method.Method.GetParameters().Length == argLenght)
                {
                    InvokeMethod(method, args);
                    return ExecutionStatus.Success;
                }
            }
            return ExecutionStatus.ArgumentLenghtMismatch;
            //if (targetMethod.Method != null)
            //{
            //    InvokeMethod(targetMethod, args);
            //}
        }
        private static void InvokeMethod(CallerObject callerObject, params object[] args)
        {
            object obj = null;
            if (callerObject.Parent != null)
                obj = callerObject.Parent;
            callerObject.Method.Invoke(obj, args);
        }
        public static SearchResult FindMethod(string selectedMethodName)
        {
            List<CallerObject> results = new List<CallerObject>();

            CallerObject obj = new CallerObject();

            selectedMethodName = selectedMethodName.ToLower();

            if (methodCache.ContainsKey(selectedMethodName))
            {
                return methodCache[selectedMethodName];
            }

            Type[] allClassesInAssembly = GetAllSubTypesInScripts(typeof(RPCModule));
            foreach (var classItem in allClassesInAssembly)
            {
                //Get all public and non-public and non-static methods and functions in class
                MethodInfo[] methods = classItem.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    if (method.Name.ToLower() != selectedMethodName)
                        continue;


                    if (!method.IsStatic)
                    {
                        Type methodParentType = classItem;
                        if (registeredClasses.ContainsKey(methodParentType))
                        {
                            obj.Parent = registeredClasses[methodParentType];
                        }
                    }
                    obj.Method = method;


                    results.Add(obj);

                }
            }
            SearchResult result = new SearchResult();
            result.Methods = results.ToArray();
            result.isSuccess = result.Methods.Length != 0;
            if (result.isSuccess)
                methodCache.Add(selectedMethodName, result);
            return result;
        }
        private static Type[] GetAllSubTypesInScripts(System.Type aBaseClass)
        {
            List<System.Type> result = new System.Collections.Generic.List<System.Type>();
            Assembly[] appAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (var asm in appAssemblies)
            {
                if (asm.FullName != currentAssembly.FullName)
                {
                    continue;
                }

                System.Type[] types = asm.GetTypes();
                foreach (var T in types)
                {
                    if (T.IsDefined(aBaseClass))
                    {
                        result.Add(T);
                    }
                }

            }
            return result.ToArray();
        }
    }
}
