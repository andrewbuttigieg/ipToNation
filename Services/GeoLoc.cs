using System;
using System.Collections.Generic;
using System.Linq;

public class GeoLoc: IGeoLoc 
{
    Dictionary<string, List<IpCountry>> Lookup = new Dictionary<string, List<IpCountry>>();
    Dictionary<string, string> Countries = new Dictionary<string, string>();
    public GeoLoc()
    {
        try{
            string[] lines = System.IO.File.ReadAllLines(@"GeoLite2-Country-CSV_20180206/GeoLite2-Country-Blocks-IPv4.csv");
            string[] countryCsv = System.IO.File.ReadAllLines(@"GeoLite2-Country-CSV_20180206/GeoLite2-Country-Locations-en.csv");

            foreach(var line in countryCsv.Skip(1)){
                var countrySplits = line.Split(',');
                Countries.Add(countrySplits[0], countrySplits[5]);
            }

            Console.WriteLine(lines.Length);

            foreach(var line in lines.Skip(1)){
                var splits = line.Split(',');
                var ipLow = splits[0].Split('/')[0];

                var ipStart = splits[0].Split('.')[0] + "." + splits[0].Split('.')[1] + ".0.0";

                if (Lookup.ContainsKey(ipStart) == false)
                    Lookup.Add(ipStart, new List<IpCountry>());

                if (Countries.ContainsKey(splits[1])){
                    Lookup[ipStart].Add(new IpCountry(){
                        Country = Countries[splits[1]],
                        Low = IpToLong(ipLow)
                    });

                    Lookup[ipStart] = Lookup[ipStart].OrderByDescending(x => x.Low).ToList();
                }
            }
            Console.WriteLine(Lookup.Count);
        }
        catch {

        }
    }

    public long IpToLong(string ipAddress){
        var ipAddressSplit = ipAddress.Split('.');
 
        var integer_ip =   ( 16777216 * long.Parse(ipAddressSplit[0]) )
            + (    65536 * long.Parse(ipAddressSplit[1]) )
            + (      256 * long.Parse(ipAddressSplit[2]) )
            +              long.Parse(ipAddressSplit[3]);

        return integer_ip;
    }

    public List<IpCountry> FromIp(string ipAddress){
        var ipAddressSplit = ipAddress.Split('.');
        
        string key;
        
        key = ipAddressSplit[0] + "." + ipAddressSplit[1] + ".0.0";

        if (Lookup.ContainsKey(key)){
            return Lookup[key];
        }
        var second = int.Parse(ipAddressSplit[1]) - 1;
        key = ipAddressSplit[0] + "." + second + ".0.0";

        if (Lookup.ContainsKey(key)){
            return Lookup[key];
        }

        var third = int.Parse(ipAddressSplit[1]) - 2;
        key = ipAddressSplit[0] + "." + third + ".0.0";

        if (Lookup.ContainsKey(key)){
            return Lookup[key];
        }

        return null;

        // key = ipAddressSplit[0] + "." + ipAddressSplit[1] + "." + ipAddressSplit[2] + ".0";

        // if (Lookup.ContainsKey(key)){
        //     return Lookup[key];
        // }

        // key = ipAddressSplit[0] + ".0.0.0";

        // if (Lookup.ContainsKey(key)){
        //     return Lookup[key];
        // }

        // return null;
    }
}