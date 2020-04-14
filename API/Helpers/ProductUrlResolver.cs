using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    //string --- refer to pictureUrl 
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source,
        ProductToReturnDto destination, string destMember,
        ResolutionContext context)
        {
            //check if pictureUrl is empty or not
            if(!string.IsNullOrEmpty(source.PictureUrl)){
                return _config["ApiUrl"]+source.PictureUrl;
            }

            return null;
        }
    }
}