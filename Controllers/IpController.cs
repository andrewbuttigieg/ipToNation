using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ip_geolocation.Controllers
{
    [Route("api/[controller]")]
    public class IpController : Controller
    {
        private IGeoLoc geoLoc;

        public IpController(IGeoLoc geoLoc){
            this.geoLoc = geoLoc;
        }

        // GET api/values
        [HttpGet]
        [Route("{ipAddress}")]
        public dynamic Get(string ipAddress)
        {
            var ipLong = geoLoc.IpToLong(ipAddress);
            var lookups = geoLoc.FromIp(ipAddress);
            foreach(var lookup in lookups){
                if (lookup.Low < ipLong)
                    return lookup.Country;
            }
            return null;
        }
    }
}
