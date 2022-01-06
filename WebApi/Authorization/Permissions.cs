using System.Collections.Generic;

namespace WebApi.Authorization
{
    public static class  Permissions
    {
        //Modül Listesi
        public static List<string> ModuleList = new List<string>{ "Orders", "Restourants" };
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permissions.{module}.NavButton",
            $"Permissions.{module}.List",
            $"Permissions.{module}.Detail",
            $"Permissions.{module}.Create",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
        }
        public static class Restourants
        {
            public const string NavButton = "Permissions.Restourants.NavButton";
            public const string List = "Permissions.Restourants.List";
            public const string Detail = "Permissions.Restourants.Detail";
            public const string Create = "Permissions.Restourants.Create";
            public const string Edit = "Permissions.Restourants.Edit";
            public const string Delete = "Permissions.Restourants.Delete";
        }

        public static class Orders
        {
            public const string NavButton = "Permissions.Orders.NavButton";
            public const string List = "Permissions.Orders.List";
            public const string Detail = "Permissions.Orders.Detail";
            public const string Create = "Permissions.Orders.Create";
            public const string Edit = "Permissions.Orders.Edit";
            public const string Delete = "Permissions.Orders.Delete";
        }
    }
}
