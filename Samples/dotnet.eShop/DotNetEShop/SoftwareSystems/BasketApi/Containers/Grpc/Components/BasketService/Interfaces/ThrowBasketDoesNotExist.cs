// <auto-generated/>
using C4InterFlow;
using C4InterFlow.Structures;
using C4InterFlow.Structures.Interfaces;

namespace DotNetEShop.SoftwareSystems
{
    public partial class BasketApi
    {
        public partial class Containers
        {
            public partial class Grpc
            {
                public partial class Components
                {
                    public partial class BasketService
                    {
                        public partial class Interfaces
                        {
                            public partial class ThrowBasketDoesNotExist : IInterfaceInstance
                            {
                                public static Interface Instance => new Interface(typeof(ThrowBasketDoesNotExist), "Throw Basket Does Not Exist")
                                {
                                    Description = "",
                                    Path = "",
                                    IsPrivate = true,
                                    Protocol = "",
                                    Flow = new Flow(Interface.GetAlias<ThrowBasketDoesNotExist>()),
                                    Input = "",
                                    InputTemplate = "",
                                    Output = "",
                                    OutputTemplate = ""
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}