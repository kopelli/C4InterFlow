// <auto-generated/>
using C4InterFlow.Structures;
using C4InterFlow.Structures.Interfaces;

namespace dotnet.eShop.Architecture.SoftwareSystems
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
                            public partial class GetBasket : IInterfaceInstance
                            {
                                private static readonly string ALIAS = "dotnet.eShop.Architecture.SoftwareSystems.BasketApi.Containers.Grpc.Components.BasketService.Interfaces.GetBasket";
                                public static Interface Instance => new Interface(dotnet.eShop.Architecture.SoftwareSystems.BasketApi.Containers.Grpc.Components.BasketService.ALIAS, ALIAS, "Get Basket")
                                {
                                    Description = "",
                                    Path = "",
                                    IsPrivate = false,
                                    Protocol = "",
                                    Flow = new Flow(ALIAS)
                                    	.Use("dotnet.eShop.Architecture.SoftwareSystems.BasketApi.Containers.Data.Components.RedisBasketRepository.Interfaces.GetBasketAsync")
                                    	.If(@"data is not null")
                                    		.Use("dotnet.eShop.Architecture.SoftwareSystems.BasketApi.Containers.Grpc.Components.BasketService.Interfaces.MapToCustomerBasketResponse")
                                    	.EndIf(),
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