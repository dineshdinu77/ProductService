using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMicroService.Models;
using ProductMicroService.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProvider prodProvider;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(ProductController));
        public ProductController(IProvider _prodProvider)
        {

            prodProvider = _prodProvider;
        }

        [HttpGet]
        [Route("searchProductById/{prod_id}")]
        public IActionResult SearchProductById(int prod_id)
        {

            try
            {
                _log4net.Info(" Http GET in controller is accesed");

                var result = prodProvider.SearchProductById(prod_id);
                _log4net.Info("method execution in controller completed");

                if (result == null)
                {
                    _log4net.Info("method returns a null value");
                    return NotFound();
                }
                _log4net.Info("product with id " + prod_id + " is " + result);
                return Ok(result);
            }
            catch(Exception e)
            {
                _log4net.Error("Error in getting the product with respective Id"+prod_id+ " As "+e.Message);
                return StatusCode(500);
            }
           


        }
        [HttpGet]
        [Route("searchProductByName/{prod_name}")]
        public IActionResult SearchProductByName(string prod_name)
        {
            try
            {
                _log4net.Info(" Http GET in controller is accesed");

                var result = prodProvider.SearchProductByName(prod_name);
                _log4net.Info("method execution in controller completed");

                if (result == null)
                {
                    _log4net.Info("method returns a null value");
                    return NotFound();
                }
                _log4net.Info("product with id " + prod_name + " is " + result);
                return Ok(result);
            }
            catch(Exception e)
            {
                _log4net.Error("Error in getting the product with respective Name" + prod_name + " As " + e.Message);
                return StatusCode(500);
            }

           


        }
        [HttpPost]
        [Route("AddProductRating")]
        public IActionResult AddProductRating(ProductRating model)
        {
            try
            {
                var res = prodProvider.AddProductRating(model);
                _log4net.Info("method execution in controller completed");
                if (res == null)
                {
                    return BadRequest();
                }


                _log4net.Info("rating " + model.Rating + " for the product with id=" + model.Id + " is assigned");
                return Ok(res);
            }
            catch(Exception e)
            {
                _log4net.Error("Error in Adding Rating for the product with respective Id"+ model.Id + " As " + e.Message);
                return StatusCode(500);
            }
        }
    }
}
