namespace Web.Api.Ioc
{
    public class IocAdapter
    {
        public static IIocContainer Container { get; private set; }

        public static void SetContainer(IIocContainer container)
        {
            Container = container;
        }
    }
}