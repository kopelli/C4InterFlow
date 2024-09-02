// <auto-generated/>
using C4InterFlow.Structures;
using C4InterFlow.Structures.Interfaces;

namespace DotNetEShop.SoftwareSystems
{
    public partial class CatalogApi
    {
        public partial class Containers
        {
            public partial class Api
            {
                public partial class Components
                {
                    public partial class CatalogApi
                    {
                        public partial class Interfaces
                        {
                            public partial class UpdateItem : IInterfaceInstance
                            {
                                private static readonly string ALIAS = "DotNetEShop.SoftwareSystems.CatalogApi.Containers.Api.Components.CatalogApi.Interfaces.UpdateItem";
                                public static Interface Instance => new Interface(DotNetEShop.SoftwareSystems.CatalogApi.Containers.Api.Components.CatalogApi.ALIAS, ALIAS, "Update Item")
                                {
                                    Description = "",
                                    Path = "",
                                    IsPrivate = false,
                                    Protocol = "",
                                    Flow = new Flow(ALIAS)
                                    	.If(@"catalogItem == null")
                                    		.Return(@"TypedResults.NotFound")
                                    	.EndIf()
                                    	.If(@"priceEntry.IsModified")
                                    		.Else()
                                    		.Use("DotNetEShop.SoftwareSystems.CatalogApi.Containers.Infrastructure.Components.CatalogContext.Interfaces.SaveChangesAsync")
                                    	.EndIf()
                                    	.Return(@"TypedResults.Created"),
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