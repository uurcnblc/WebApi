using System.Collections.Generic;

namespace WebApiAuth.Authorization
{
    public static class StaticPermissions
    {
        public static List<string> ModuleList = new List<string> { "Orders", "Restourants" };
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"{module}.NavButton",
            $"{module}.List",
            $"{module}.Detail",
            $"{module}.Create",
            $"{module}.Edit",
            $"{module}.Delete",
        };
        }
        public static class Restourants
        {
            public const string NavButton = "Restourants.NavButton";
            public const string List = "Restourants.List";
            public const string Detail = "Restourants.Detail";
            public const string Create = "Restourants.Create";
            public const string Edit = "Restourants.Edit";
            public const string Delete = "Restourants.Delete";
        }

        public static class Orders
        {
            public const string NavButton = "Orders.NavButton";
            public const string List = "Orders.List";
            public const string Detail = "Orders.Detail";
            public const string Create = "Orders.Create";
            public const string Edit = "Orders.Edit";
            public const string Delete = "Orders.Delete";
        }
    }
}
